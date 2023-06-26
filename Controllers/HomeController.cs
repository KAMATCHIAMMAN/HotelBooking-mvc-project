using KSBookingMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data;

namespace KSBookingMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Uri baseAddress = new Uri("https://localhost:7114/api/Authentication");
        private readonly HttpClient _client;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _client=new HttpClient();
            _client.BaseAddress = baseAddress;
            
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
 
        public async Task<IActionResult> Register(Register userRegistration)
        {
            //try
            //{
            //    string data = JsonConvert.SerializeObject(userRegistration);
            //    StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            //    HttpResponseMessage response =  _client.PostAsync(_client.BaseAddress + "/Authentication/AddUser", content).Result;
            //    Console.WriteLine(response);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        //TempData["successMessage"] = "User Registered successfully";
            //        return RedirectToAction("Index");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return View();
            //}
            //return View();
            if (!ModelState.IsValid)
            {

                return RedirectToAction("Register");
            }

            var registeredUsers = await GetRegisteredUsers();

            if (registeredUsers.Any(u => u.Name == userRegistration.Name))
            {
                ModelState.AddModelError(string.Empty, "Username already exists.");
                return RedirectToAction("Register");
            }
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7114/api/User/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync("AddRegisteredUser", userRegistration);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
        }
        private async Task<List<Register>> GetRegisteredUsers()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7114/api/Admin/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("GetUserDetails");
                if (response.IsSuccessStatusCode)
                {
                    string contentString = await response.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<List<Register>>(contentString);
                    return users;
                }
                else
                {

                    return new List<Register>();
                }
            }
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            if (!ModelState.IsValid)
            {
                //ViewBag.AlertMessage = "This is an alert message!";
                return RedirectToAction("Login");

            }

            var registeredUsers = await GetRegisteredUsers();
            if (IsValidUser(login.Email, login.Password, registeredUsers))
            {
                return RedirectToAction("ListOfHotelsUser");
            }
            else
            {

                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return RedirectToAction("Login");
            }
        }
        [HttpGet]
        public  IActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AdminLogin(Login login)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AlertMessage = "This is an alert message!";
                return RedirectToAction("AdminLogin");

            }

            var registeredUsers = await GetRegisteredUsers();
            if (IsValidUser(login.Email, login.Password, registeredUsers))
            {
                return RedirectToAction("Admin");
            }
            else
            {

                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return RedirectToAction("AdminLogin");
            }
        }

        private bool IsValidUser(string email, string password, List<Register> registeredUsers)
        {
            var user = registeredUsers.FirstOrDefault(u => u.Email == email && u.Password == password);
            return user != null;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Admin API

        public IActionResult Admin()
        {

            return View();
        }
        public IActionResult HotelDetails()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> HotelDetails(ListOfHotels listOfHotels)

        {


            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7114/api/Admin/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync("AddHotels", listOfHotels);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Privacy");
                }

            }
        }
        [HttpGet]
        public async Task<IActionResult> GetHotellist()
        {
            List<ListOfHotels> listOfHotels = new List<ListOfHotels>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7114/api/Admin/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("GetHotels");
                Console.WriteLine(getData);

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    listOfHotels = JsonConvert.DeserializeObject<List<ListOfHotels>>(results);
                }
                else
                {
                    Console.WriteLine("Error Calling web Api");
                }
            }
            return View(listOfHotels);
        }

        [HttpGet]
        public IActionResult GetHotelById(int id)
        {
            ListOfHotels menu = new ListOfHotels();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7114/api/Admin/");
            HttpResponseMessage response = client.GetAsync($"GetHotelById/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                menu = JsonConvert.DeserializeObject<ListOfHotels>(data);
                return View(menu);
            }

            return View();
        }
        [HttpGet]
        public IActionResult UpdateHotels()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UpdateHotels(ListOfHotels listofhotels)
        {
            string data = JsonConvert.SerializeObject(listofhotels);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7114/api/Admin/");
            HttpResponseMessage response = client.PutAsync("UpdateHotels", content).Result;
            Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Admin");
            }
            return View();
        }
        /*
        [HttpGet]
        public async Task<IActionResult> GetRegisterUsers()
        {
            Register dataTable = new Register();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7114/api/Admin/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage getData = await client.GetAsync("GetUserDetails");
                Console.WriteLine(getData);

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    dataTable = JsonConvert.DeserializeObject<Register>(results);
                }
                else
                {
                    Console.WriteLine("Error Calling web Api");
                }
                ViewData.Model = dataTable;

            }
            return View();
        }*/
        [HttpGet]
        public async Task<IActionResult> GetRegisterUsers()
        {
            List<Register> user = new List<Register>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7114/api/Admin/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("GetUserDetails");
                Console.WriteLine(getData);

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<List<Register>>(results);
                }
                else
                {
                    Console.WriteLine("Error Calling web Api");
                }
            }
            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> BookingDetails()
        {
            List<BookingDetails> user = new List<BookingDetails>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7114/api/Admin/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("GetBookings");
                Console.WriteLine(getData);

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<List<BookingDetails>>(results);
                }
                else
                {
                    Console.WriteLine("Error Calling web Api");
                }
            }
            return View(user);
        }

        public async Task<IActionResult> DeleteHotels(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7114/api/Admin/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.DeleteAsync("DeleteHotels?HotelsId="+id.ToString());

                if (response.IsSuccessStatusCode)
                {
                    return View();
                }
                else
                {

                    ModelState.AddModelError(string.Empty, "An error occurred while deleting the item.");
                    return View();
                }
            }
        }
        public async Task<IActionResult> DeleteUser(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7114/api/Admin/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.DeleteAsync("DeleteUser?UserId=" + id.ToString());

                if (response.IsSuccessStatusCode)
                {
                    return View();
                }
                else
                {

                    ModelState.AddModelError(string.Empty, "An error occurred while deleting the item.");
                    return View();
                }
            }
        }
        [HttpGet]
        public IActionResult ListOfHotelsUser()
        {
            List<ListOfHotels> hotels = new List<ListOfHotels>();
            HttpResponseMessage response = _client.GetAsync("https://localhost:7114/api/User/GetHotels").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                hotels = JsonConvert.DeserializeObject<List<ListOfHotels>>(data);
            }
            return View(hotels);
        }
        public IActionResult Reserverooms()
        {
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> Reserverooms(BookingDetails bookingDetails)
        {
            try
            {
                string apiUrl = "https://localhost:7114/api/User/AddBookings";

                string movieJson = JsonConvert.SerializeObject(bookingDetails);
                HttpContent content = new StringContent(movieJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    
                    return RedirectToAction("CardDetails"); 
                }
                else
                {
                    Console.WriteLine("not able to post to hotel");
                    string errorMessage = $"Failed to post movie. StatusCode: {response.StatusCode}";
                    return RedirectToAction("Error", new { message = errorMessage }); 
                }
            }
            catch (Exception ex)
            {
               
                string errorMessage = $"An error occurred while posting the movie: {ex.Message}";
                return RedirectToAction("Error", new { message = errorMessage }); // Replace "Error" with the appropriate action or view
            }
        }



        public IActionResult CardDetails()
        {
            return View();
        }



        //for create new movie

        [HttpPost]
        public async Task<IActionResult> CardDetails(CardDetails cardDetails)
        {
            try
            {
                string apiUrl = "https://localhost:7114/api/User/AddCardDetails";

                string movieJson = JsonConvert.SerializeObject(cardDetails);
                HttpContent content = new StringContent(movieJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    // Movie posted successfully
                    return RedirectToAction("Index"); // Replace "Index" with the appropriate action or view
                }
                else
                {
                    Console.WriteLine("not able to post to movie");
                    // Handle the error case
                    string errorMessage = $"Failed to post movie. StatusCode: {response.StatusCode}";
                    return RedirectToAction("Error", new { message = errorMessage }); // Replace "Error" with the appropriate action or view
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                string errorMessage = $"An error occurred while posting the movie: {ex.Message}";
                return RedirectToAction("Error", new { message = errorMessage }); // Replace "Error" with the appropriate action or view
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SPFWebsitMVC.Models;

namespace SPFWebsitMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private bool ready;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult FAQNonLogin()
        {
            return View();
        }

        public IActionResult Index()
        {
            SetRole();
            while (!ready) ;
            if (GlobalSettings.CurrentUserRole == "Admin")
            {
                return RedirectToAction("Index", "Admins");
            }
            if (GlobalSettings.CurrentUserRole == "Trainer")
            {
                return RedirectToAction("Index", "Trainers");
            }
            if (GlobalSettings.CurrentUserRole == "Client")
            {
                return RedirectToAction("Index", "Clients");
            }

            return View();
        }

        public IActionResult Personel()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> SetRole()
        {
            ready = false;
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Not logged in, stay home.
            if (userId == null)
            {
                GlobalSettings.CurrentUserRole = null;
                ready = true;
                return RedirectToAction("Index", "Home");
            }
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/admins/{userId}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                GlobalSettings.CurrentUserRole = "Admin";
                ready = true;
                return RedirectToAction("Index", "Admins");
            }
            url = $"{GlobalSettings.baseEndpoint}/trainers/{userId}";
            response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                GlobalSettings.CurrentUserRole = "Trainer";
                ready = true;
                return RedirectToAction("Index", "Trainers");
            }
            url = $"{GlobalSettings.baseEndpoint}/clients/{userId}";
            response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                GlobalSettings.CurrentUserRole = "Client";
                ready = true;
                return RedirectToAction("Index", "Clients");
            }

            // Logged in so default to client.
            GlobalSettings.CurrentUserRole = "Client";
            ready = true;
            return RedirectToAction("Index", "Clients");
        }
    }
}

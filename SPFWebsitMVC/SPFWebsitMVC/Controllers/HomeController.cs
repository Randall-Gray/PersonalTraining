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

        private async Task<IActionResult> SetRole()
        {
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/admins/{userId}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                GlobalSettings.CurrentUserRole = "Admin";
                return RedirectToAction(nameof(Index), "Home");
            }
            url = $"{GlobalSettings.baseEndpoint}/trainers/{userId}";
            response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                GlobalSettings.CurrentUserRole = "Trainer";
                return RedirectToAction(nameof(Index), "Home");
            }
            GlobalSettings.CurrentUserRole = "Client";
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}

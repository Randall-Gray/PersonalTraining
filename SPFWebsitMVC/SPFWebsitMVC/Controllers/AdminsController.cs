using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SPFWebsitMVC.Data;
using SPFWebsitMVC.Models;

namespace SPFWebsitMVC.Controllers
{
    public class AdminsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admins
        public async Task<IActionResult> Index()
        {
            List<Admin> Admins = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/admins/";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                Admins = JsonConvert.DeserializeObject<List<Admin>>(jsonResponse);
            }
            return View(Admins);
        }

        // GET: Admins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Admin admin = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/admins/GetAdminById/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                admin = JsonConvert.DeserializeObject<Admin>(jsonResponse);
            }

            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // GET: Admins/Create
        public async Task<IActionResult> Create()
        {
            // Admins can't be created.  They are created as clients and then moved over to Admins.
            return RedirectToAction("Index", "Admins");
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Only called from Clients.Index to convert client to an Admin.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdminId,FirstName,LastName,Email,PhoneNumber,IdentityUserId")] Admin admin)
        {
            string jsonForPost = JsonConvert.SerializeObject(admin);
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/admins";
            HttpResponseMessage response = await httpClient.PostAsync(url, new StringContent(jsonForPost, Encoding.UTF8, "application/json"));
            return RedirectToAction("Index", "Clients");
        }

        // GET: Admins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Admin admin = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/admins/GetAdminById/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                admin = JsonConvert.DeserializeObject<Admin>(jsonResponse);
            }

            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdminId,FirstName,LastName,Email,PhoneNumber,IdentityUserId")] Admin admin)
        {
            if (id != admin.AdminId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string jsonForPost = JsonConvert.SerializeObject(admin);
                    HttpClient httpClient = new HttpClient();
                    string url = $"{GlobalSettings.baseEndpoint}/admins/{id}";
                    HttpResponseMessage response = await httpClient.PutAsync(url, new StringContent(jsonForPost, Encoding.UTF8, "application/json"));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await AdminExists(id) == false)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: Admins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Can't delete last admin.  Check there are more than one.
            List<Admin> Admins = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/admins/";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                Admins = JsonConvert.DeserializeObject<List<Admin>>(jsonResponse);
            }
            if (Admins == null || Admins.Count <= 1)
                return RedirectToAction("Index");

            Admin admin = null;
            url = $"{GlobalSettings.baseEndpoint}/admins/GetAdminById/{id}";
            response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                admin = JsonConvert.DeserializeObject<Admin>(jsonResponse);
            }

            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/admins/{id}";
            HttpResponseMessage response = await httpClient.DeleteAsync(url);
            return RedirectToAction("Index");
        }

        private async Task<bool> AdminExists(int? id)
        {
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/admins/GetAdminById/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}

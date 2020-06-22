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
    public class TrainersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Trainers
        public async Task<IActionResult> Index()
        {
            List<Trainer> Trainers = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/trainers/";
            HttpResponseMessage response;

            // Display all trainers
            if (GlobalSettings.CurrentUserRole == "Admin")
            {
                response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Trainers = JsonConvert.DeserializeObject<List<Trainer>>(jsonResponse);
                }
                return View(Trainers);
            }
            // Only display the logged in Trainer
            Trainer trainer = null;
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            url += "GetTrainerByIdentityValue/" + userId;
            response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                trainer = JsonConvert.DeserializeObject<Trainer>(jsonResponse);
                Trainers = new List<Trainer>();
                Trainers.Add(trainer);
            }
            if (Trainers == null)
                return RedirectToAction("Create");

            return View(Trainers);
        }

        // GET: Trainers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Trainer trainer = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/trainers/GetTrainerById/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                trainer = JsonConvert.DeserializeObject<Trainer>(jsonResponse);
            }

            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // GET: Trainers/Create
        public IActionResult Create()
        {
            // Trainers can't be created.  They are created as clients and then moved over to Trainers.
            return RedirectToAction("Index", "Trainers");
        }

        // POST: Trainers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Only called from Clients.Index to convert client to a Trainer.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainerId,FirstName,LastName,Email,PhoneNumber,IdentityUserId")] Trainer trainer)
        {
            string jsonForPost = JsonConvert.SerializeObject(trainer);
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/trainers";
            HttpResponseMessage response = await httpClient.PostAsync(url, new StringContent(jsonForPost, Encoding.UTF8, "application/json"));
            return RedirectToAction("Index", "Clients");
        }

        // GET: Trainers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Trainer trainer = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/trainers/GetTrainerById/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                trainer = JsonConvert.DeserializeObject<Trainer>(jsonResponse);
            }

            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // POST: Trainers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainerId,FirstName,LastName,Email,PhoneNumber,IdentityUserId")] Trainer trainer)
        {
            if (id != trainer.TrainerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string jsonForPost = JsonConvert.SerializeObject(trainer);
                    HttpClient httpClient = new HttpClient();
                    string url = $"{GlobalSettings.baseEndpoint}/trainers/{id}";
                    HttpResponseMessage response = await httpClient.PutAsync(url, new StringContent(jsonForPost, Encoding.UTF8, "application/json"));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TrainerExists(id) == false)
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
            return View(trainer);
        }

        // GET: Trainers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Trainer trainer = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/trainers/GetTrainerById/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                trainer = JsonConvert.DeserializeObject<Trainer>(jsonResponse);
            }

            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // POST: Trainers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/trainers/{id}";
            HttpResponseMessage response = await httpClient.DeleteAsync(url);
            return RedirectToAction("Index");
        }

        private async Task<bool> TrainerExists(int? id)
        {
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/trainers/GetTrainerById/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}

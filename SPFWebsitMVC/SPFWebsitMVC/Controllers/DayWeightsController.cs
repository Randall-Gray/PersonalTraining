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
    public class DayWeightsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DayWeightsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DayWeights
        public async Task<IActionResult> Index()
        {
            List<DayWeight> DayWeights = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/dayweights/";
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            url += "GetDayWeightsByClientIdentityValue/" + userId;
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                DayWeights = JsonConvert.DeserializeObject<List<DayWeight>>(jsonResponse);
            }
            return View(DayWeights);
        }

        // GET: DayWeights/Details/5
        // Displays the Client's Progress Chart.
        public async Task<IActionResult> Details(int? id)
        {
            DayWeight dayWeight = new DayWeight();

            if (id == null)
            {
                // Get the current client
                Client client = null;
                HttpClient httpClient = new HttpClient();
                string url = $"{GlobalSettings.baseEndpoint}/clients/";
                string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                url += "GetClientByIdentityValue/" + userId;
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    client = JsonConvert.DeserializeObject<Client>(jsonResponse);
                }

                dayWeight.ClientId = client.ClientId;
            }
            else
                dayWeight.ClientId = (int)id;

            return View(dayWeight);
        }

        // GET: DayWeights/Create
        public async Task<IActionResult> Create()
        {
            // Get the current client
            Client client = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/clients/";
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            url += "GetClientByIdentityValue/" + userId;
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                client = JsonConvert.DeserializeObject<Client>(jsonResponse);
            }

            DayWeight dayWeight = new DayWeight();
            dayWeight.ClientId = client.ClientId;
            return View(dayWeight);
        }

        // POST: DayWeights/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DayWeightId,Day,Weight,ClientId")] DayWeight dayWeight)
        {
            if (ModelState.IsValid)
            {
                string jsonForPost = JsonConvert.SerializeObject(dayWeight);
                HttpClient httpClient = new HttpClient();
                string url = $"{GlobalSettings.baseEndpoint}/dayweights";
                HttpResponseMessage response = await httpClient.PostAsync(url, new StringContent(jsonForPost, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "DayWeights");
                }
            }
            return View(dayWeight);
        }

        // GET: DayWeights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DayWeight dayWeight = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/dayweights/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                dayWeight = JsonConvert.DeserializeObject<DayWeight>(jsonResponse);
            }

            if (dayWeight == null)
            {
                return NotFound();
            }
            return View(dayWeight);
        }

        // POST: DayWeights/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DayWeightId,Day,Weight,ClientId")] DayWeight dayWeight)
        {
            if (id != dayWeight.DayWeightId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string jsonForPost = JsonConvert.SerializeObject(dayWeight);
                    HttpClient httpClient = new HttpClient();
                    string url = $"{GlobalSettings.baseEndpoint}/dayWeights/{id}";
                    HttpResponseMessage response = await httpClient.PutAsync(url, new StringContent(jsonForPost, Encoding.UTF8, "application/json"));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await DayWeightExists(id) == false)
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
            return View(dayWeight);
        }

        // GET: DayWeights/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DayWeight dayWeight = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/dayweights/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                dayWeight = JsonConvert.DeserializeObject<DayWeight>(jsonResponse);
            }

            if (dayWeight == null)
            {
                return NotFound();
            }

            return View(dayWeight);
        }

        // POST: DayWeights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/dayweights/{id}";
            HttpResponseMessage response = await httpClient.DeleteAsync(url);
            return RedirectToAction("Index");
        }

        private async Task<bool> DayWeightExists(int? id)
        {
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/dayweights/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    public class BroadcastMessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BroadcastMessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BroadcastMessages
        public async Task<IActionResult> Index()
        {
            List<BroadcastMessage> BroadcastMessages = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/broadcastmessages/";
            HttpResponseMessage response;

            // Display all BroadcastMessages
            if (GlobalSettings.CurrentUserRole == "Admin")
            {
                response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    BroadcastMessages = JsonConvert.DeserializeObject<List<BroadcastMessage>>(jsonResponse);
                }
                return View(BroadcastMessages);
            }
            // Only display the current BroadcastMessages
            url += "GetCurrentBroadcastMessages/";
            response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                BroadcastMessages = JsonConvert.DeserializeObject<List<BroadcastMessage>>(jsonResponse);
            }

            return View(BroadcastMessages);
        }

        // GET: BroadcastMessages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BroadcastMessage message = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/broadcastmessages/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                message = JsonConvert.DeserializeObject<BroadcastMessage>(jsonResponse);
            }

            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // GET: BroadcastMessages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BroadcastMessages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BroadcastMessageId,Message,DatePosted,NumDays,PosterName,Status")] BroadcastMessage broadcastMessage)
        {
            if (ModelState.IsValid)
            {
                string jsonForPost = JsonConvert.SerializeObject(broadcastMessage);
                HttpClient httpClient = new HttpClient();
                string url = $"{GlobalSettings.baseEndpoint}/broadcastmessages";
                HttpResponseMessage response = await httpClient.PostAsync(url, new StringContent(jsonForPost, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "BroadcastMessages");
                }
            }
            return View(broadcastMessage);
        }

        // GET: BroadcastMessages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BroadcastMessage message = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/broadcastmessages/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                message = JsonConvert.DeserializeObject<BroadcastMessage>(jsonResponse);
            }

            if (message == null)
            {
                return NotFound();
            }
            return View(message);
        }

        // POST: BroadcastMessages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BroadcastMessageId,Message,DatePosted,NumDays,PosterName,Status")] BroadcastMessage broadcastMessage)
        {
            if (id != broadcastMessage.BroadcastMessageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string jsonForPost = JsonConvert.SerializeObject(broadcastMessage);
                    HttpClient httpClient = new HttpClient();
                    string url = $"{GlobalSettings.baseEndpoint}/broadcastmessages/{id}";
                    HttpResponseMessage response = await httpClient.PutAsync(url, new StringContent(jsonForPost, Encoding.UTF8, "application/json"));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await BroadcastMessageExists(id) == false)
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
            return View(broadcastMessage);
        }

        // GET: BroadcastMessages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BroadcastMessage message = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/broadcastmessages/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                message = JsonConvert.DeserializeObject<BroadcastMessage>(jsonResponse);
            }

            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: BroadcastMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/broadcastmessages/{id}";
            HttpResponseMessage response = await httpClient.DeleteAsync(url);
            return RedirectToAction("Index");
        }

        private async Task<bool> BroadcastMessageExists(int? id)
        {
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/broadcastmessages/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}

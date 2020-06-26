using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
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
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            List<Client> Clients = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/clients/";
            HttpResponseMessage response;

            // Display all clients
            if (GlobalSettings.CurrentUserRole == "Admin" || GlobalSettings.CurrentUserRole == "Trainer")
            {
                response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Clients = JsonConvert.DeserializeObject<List<Client>>(jsonResponse);
                }
                return View(Clients);
            }
            // Only display the logged in client
            Client client = null;
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            url += "GetClientByIdentityValue/" + userId;
            response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                client = JsonConvert.DeserializeObject<Client>(jsonResponse);
                Clients = new List<Client>();
                Clients.Add(client);
            }
            if (Clients == null)
                return RedirectToAction("Create");

            return View(Clients);
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Client client = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/clients/GetClientById/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                client = JsonConvert.DeserializeObject<Client>(jsonResponse);
            }

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            Client client = new Client();
            client.IdentityUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            client.Email = this.User.Identity.Name;  // Display email but don't allow editing.
            return View(client);
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId,FirstName,LastName,Email,PhoneNumber,BalanceOwed,Goal,FavoriteVideo1,FavoriteVideo2,FavoriteVideo3,IdentityUserId")] Client client)
        {
            if (ModelState.IsValid)
            {
                string jsonForPost = JsonConvert.SerializeObject(client);
                HttpClient httpClient = new HttpClient();
                string url = $"{GlobalSettings.baseEndpoint}/clients";
                HttpResponseMessage response = await httpClient.PostAsync(url, new StringContent(jsonForPost, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Clients");
                }
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Client client = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/clients/GetClientById/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                client = JsonConvert.DeserializeObject<Client>(jsonResponse);
            }

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClientId,FirstName,LastName,Email,PhoneNumber,BalanceOwed,Goal,FavoriteVideo1,FavoriteVideo2,FavoriteVideo3,IdentityUserId")] Client client)
        {
            if (id != client.ClientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string jsonForPost = JsonConvert.SerializeObject(client);
                    HttpClient httpClient = new HttpClient();
                    string url = $"{GlobalSettings.baseEndpoint}/clients/{id}";
                    HttpResponseMessage response = await httpClient.PutAsync(url, new StringContent(jsonForPost, Encoding.UTF8, "application/json"));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await ClientExists(id) == false)
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
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Client client = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/clients/GetClientById/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                client = JsonConvert.DeserializeObject<Client>(jsonResponse);
            }

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Decrease the favorite video counts.
            // Get the client.
            Client client = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/clients/GetClientById/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                client = JsonConvert.DeserializeObject<Client>(jsonResponse);
            }
            int[] favorites = { client.FavoriteVideo1, client.FavoriteVideo2, client.FavoriteVideo3 };
            for (int i = 0; i < 3; i++)
            {
                if (favorites[i] != 0)
                {
                    // Get the video
                    Video video = null;
                    httpClient = new HttpClient();
                    url = $"{GlobalSettings.baseEndpoint}/videos/{favorites[i]}";
                    response = await httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        video = JsonConvert.DeserializeObject<Video>(jsonResponse);
                    }

                    if (video == null)
                    {
                        continue;
                    }

                    video.CurrentUse--;

                    // Put the video back.
                    string jsonForPost = JsonConvert.SerializeObject(video);
                    httpClient = new HttpClient();
                    url = $"{GlobalSettings.baseEndpoint}/videos/{favorites[i]}";
                    response = await httpClient.PutAsync(url, new StringContent(jsonForPost, Encoding.UTF8, "application/json"));
                }
            }

            // Delete the client.
            httpClient = new HttpClient();
            url = $"{GlobalSettings.baseEndpoint}/clients/{id}";
            response = await httpClient.DeleteAsync(url);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MakeAdmin(int? id)
        {
            if (GlobalSettings.CurrentUserRole != "Admin")
                return RedirectToAction("Index");

            if (id == null)
            {
                return NotFound();
            }

            Client client = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/clients/GetClientById/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                client = JsonConvert.DeserializeObject<Client>(jsonResponse);
            }

            if (client == null)
            {
                return NotFound();
            }

            // Add client to admin table.
            Admin admin = new Admin();
            admin.FirstName = client.FirstName;
            admin.LastName = client.LastName;
            admin.Email = client.Email;
            admin.PhoneNumber = client.PhoneNumber;
            admin.IdentityUserId = client.IdentityUserId;

            string jsonForPost = JsonConvert.SerializeObject(admin);
            url = $"{GlobalSettings.baseEndpoint}/admins";
            response = await httpClient.PostAsync(url, new StringContent(jsonForPost, Encoding.UTF8, "application/json"));

            // Delete client from client table.
            url = $"{GlobalSettings.baseEndpoint}/clients/{client.ClientId}";
            response = await httpClient.DeleteAsync(url);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MakeTrainer(int? id)
        {
            if (GlobalSettings.CurrentUserRole != "Admin")
                return RedirectToAction("Index");

            if (id == null)
            {
                return NotFound();
            }

            Client client = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/clients/GetClientById/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                client = JsonConvert.DeserializeObject<Client>(jsonResponse);
            }

            if (client == null)
            {
                return NotFound();
            }

            // Add client to trainer table.
            Trainer trainer = new Trainer();
            trainer.FirstName = client.FirstName;
            trainer.LastName = client.LastName;
            trainer.Email = client.Email;
            trainer.PhoneNumber = client.PhoneNumber;
            trainer.IdentityUserId = client.IdentityUserId;

            string jsonForPost = JsonConvert.SerializeObject(trainer);
            url = $"{GlobalSettings.baseEndpoint}/trainers";
            response = await httpClient.PostAsync(url, new StringContent(jsonForPost, Encoding.UTF8, "application/json"));

            // Delete client from client table.
            url = $"{GlobalSettings.baseEndpoint}/clients/{client.ClientId}";
            response = await httpClient.DeleteAsync(url);
            return RedirectToAction("Index");
        }

        public IActionResult FAQLogin()
        {
            return View();
        }

        public IActionResult Schedule()
        {
            return View();
        }

        private async Task<bool> ClientExists(int? id)
        {
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/clients/GetClientById/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}

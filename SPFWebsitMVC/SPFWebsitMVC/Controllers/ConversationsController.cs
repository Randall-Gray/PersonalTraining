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
    public class ConversationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConversationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Conversations
        public async Task<IActionResult> Index()
        {
            List<Conversation> Conversations = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}";
            HttpResponseMessage response;

            // Display all conversations
            if (GlobalSettings.CurrentUserRole == "Admin" || GlobalSettings.CurrentUserRole == "Trainer")
            {
                url += "/conversations/";
                response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Conversations = JsonConvert.DeserializeObject<List<Conversation>>(jsonResponse);
                }
                return View(Conversations);
            }
            // Only display the conversations of the logged in client
            Client client = null;
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            url += "/clients/GetClientByIdentityValue/" + userId;
            response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                client = JsonConvert.DeserializeObject<Client>(jsonResponse);
            }
            url = $"{GlobalSettings.baseEndpoint}/conversations/GetConversationsByClientId/" + client.ClientId;
            response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                Conversations = JsonConvert.DeserializeObject<List<Conversation>>(jsonResponse);
            }
            return View(Conversations);
        }

        // GET: Conversations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Conversation conversation = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/conversations/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                conversation = JsonConvert.DeserializeObject<Conversation>(jsonResponse);
            }
            if (conversation == null)
            {
                return NotFound();
            }

            return View(conversation);
        }

        // GET: Conversations/Create
        public async Task<IActionResult> Create()
        {
            Conversation conversation = new Conversation();
            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}";
            HttpResponseMessage response;

            if (GlobalSettings.CurrentUserRole == "Client")
            {
                Client client = null;
                url += "/clients/GetClientByIdentityValue/" + userId;
                response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    client = JsonConvert.DeserializeObject<Client>(jsonResponse);
                    conversation.ClientId = client.ClientId;
                }
            }
            else if (GlobalSettings.CurrentUserRole == "Trainer")
            {
                Trainer trainer = null;
                url += "/trainers/GetTrainerByIdentityValue/" + userId;
                response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    trainer = JsonConvert.DeserializeObject<Trainer>(jsonResponse);
                    conversation.TrainerId = trainer.TrainerId;
                }
            }
            return View(conversation);
        }

        // POST: Conversations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConversationId,Question,Answer,FollowUp,DatePosted,ClientId,TrainerId")] Conversation conversation)
        {
            if (ModelState.IsValid)
            {
                string jsonForPost = JsonConvert.SerializeObject(conversation);
                HttpClient httpClient = new HttpClient();
                string url = $"{GlobalSettings.baseEndpoint}/conversations";
                HttpResponseMessage response = await httpClient.PostAsync(url, new StringContent(jsonForPost, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Conversations");
                }
            }
            return View(conversation);
        }

        // GET: Conversations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Conversation conversation = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/conversations/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                conversation = JsonConvert.DeserializeObject<Conversation>(jsonResponse);
            }

            if (conversation == null)
            {
                return NotFound();
            }
            return View(conversation);
        }

        // POST: Conversations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConversationId,Question,Answer,FollowUp,DatePosted,ClientId,TrainerId")] Conversation conversation)
        {
            if (id != conversation.ConversationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string jsonForPost = JsonConvert.SerializeObject(conversation);
                    HttpClient httpClient = new HttpClient();
                    string url = $"{GlobalSettings.baseEndpoint}/conversations/{id}";
                    HttpResponseMessage response = await httpClient.PutAsync(url, new StringContent(jsonForPost, Encoding.UTF8, "application/json"));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await ConversationExists(conversation.ConversationId) == false)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(conversation);
        }

        // GET: Conversations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Conversation conversation = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/conversations/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                conversation = JsonConvert.DeserializeObject<Conversation>(jsonResponse);
            }

            if (conversation == null)
            {
                return NotFound();
            }

            return View(conversation);
        }

        // POST: Conversations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/conversations/{id}";
            HttpResponseMessage response = await httpClient.DeleteAsync(url);
            return RedirectToAction("Index");
        }

        private async Task<bool> ConversationExists(int? id)
        {
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/conversations/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}

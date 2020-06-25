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
    public class ClientVideosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static int? clientId;

        public ClientVideosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Videos
        public async Task<IActionResult> Index(int? id)
        {
            if (id != null)
                clientId = id;

            List<Video> Videos = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/videos/";
            HttpResponseMessage response;

            // Only display the posted Videos
            url += "GetPostedVideos/";
            response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                Videos = JsonConvert.DeserializeObject<List<Video>>(jsonResponse);
            }

            return View(Videos);
        }

        // GET: Videos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Video video = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/videos/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                video = JsonConvert.DeserializeObject<Video>(jsonResponse);
            }

            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // Get: Videos/MakeFavorite/5
        public async Task<IActionResult> MakeFavorite(int? id)
        {
            // Get the client
            Client client = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/clients/GetClientById/{clientId}";
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

            if (client.FavoriteVideo1 == 0)
                client.FavoriteVideo1 = (int)id;
            else if (client.FavoriteVideo2 == 0)
                client.FavoriteVideo2 = (int)id;
            else if (client.FavoriteVideo3 == 0)
                client.FavoriteVideo3 = (int)id;
            else
                client.FavoriteVideo1 = (int)id;  // if all are full, replace #1

            // Put the client back.
            string jsonForPost = JsonConvert.SerializeObject(client);
            httpClient = new HttpClient();
            url = $"{GlobalSettings.baseEndpoint}/clients/{clientId}";
            response = await httpClient.PutAsync(url, new StringContent(jsonForPost, Encoding.UTF8, "application/json"));

            // Get the video
            Video video = null;
            httpClient = new HttpClient();
            url = $"{GlobalSettings.baseEndpoint}/videos/{id}";
            response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                video = JsonConvert.DeserializeObject<Video>(jsonResponse);
            }

            if (video == null)
            {
                return NotFound();
            }

            video.CurrentUse++;
            video.TotalUse++;

            // Put the video back.
            jsonForPost = JsonConvert.SerializeObject(video);
            httpClient = new HttpClient();
            url = $"{GlobalSettings.baseEndpoint}/videos/{id}";
            response = await httpClient.PutAsync(url, new StringContent(jsonForPost, Encoding.UTF8, "application/json"));

            return RedirectToAction("Index", "ClientVideos");
        }

        // GET: Videos/Create
        public IActionResult Create()
        {
            Video video = new Video();
            video.CurrentUse = 0;
            video.TotalUse = 0;
            video.DatePosted = DateTime.Now;
            return View(video);
        }

        // POST: Videos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VideoId,Name,Topic,Link,DatePosted,CurrentUse,TotalUse,Post")] Video video)
        {
            if (ModelState.IsValid)
            {
                string jsonForPost = JsonConvert.SerializeObject(video);
                HttpClient httpClient = new HttpClient();
                string url = $"{GlobalSettings.baseEndpoint}/videos";
                HttpResponseMessage response = await httpClient.PostAsync(url, new StringContent(jsonForPost, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Videos");
                }
            }
            return View(video);
        }

        // GET: Videos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Video video = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/videos/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                video = JsonConvert.DeserializeObject<Video>(jsonResponse);
            }

            if (video == null)
            {
                return NotFound();
            }
            return View(video);
        }

        // POST: Videos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VideoId,Name,Topic,Link,DatePosted,CurrentUse,TotalUse,Post")] Video video)
        {
            if (id != video.VideoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string jsonForPost = JsonConvert.SerializeObject(video);
                    HttpClient httpClient = new HttpClient();
                    string url = $"{GlobalSettings.baseEndpoint}/videos/{id}";
                    HttpResponseMessage response = await httpClient.PutAsync(url, new StringContent(jsonForPost, Encoding.UTF8, "application/json"));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await VideoExists(id) == false)
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
            return View(video);
        }

        // GET: Videos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Video video = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/videos/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                video = JsonConvert.DeserializeObject<Video>(jsonResponse);
            }

            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // POST: Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/videos/{id}";
            HttpResponseMessage response = await httpClient.DeleteAsync(url);
            return RedirectToAction("Index");
        }

        private async Task<bool> VideoExists(int? id)
        {
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/videos/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}

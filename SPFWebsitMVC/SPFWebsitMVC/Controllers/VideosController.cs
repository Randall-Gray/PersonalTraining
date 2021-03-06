﻿using System;
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
    public class VideosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VideosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Videos
        public async Task<IActionResult> Index()
        {
            List<Video> Videos = null;
            HttpClient httpClient = new HttpClient();
            string url = $"{GlobalSettings.baseEndpoint}/videos/";
            HttpResponseMessage response;

            // Display all Videos
            if (GlobalSettings.CurrentUserRole == "Admin" || GlobalSettings.CurrentUserRole == "Trainer")
            {
                response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Videos = JsonConvert.DeserializeObject<List<Video>>(jsonResponse);
                }
                return View(Videos);
            }
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

            // Go through all clients and clear their favorite video if this one.
            List<Client> Clients = null;
            httpClient = new HttpClient();
            url = $"{GlobalSettings.baseEndpoint}/clients/";
            response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                Clients = JsonConvert.DeserializeObject<List<Client>>(jsonResponse);
            }
            foreach(Client client in Clients)
            {
                if (client.FavoriteVideo1 == id)
                    client.FavoriteVideo1 = 0;
                if (client.FavoriteVideo2 == id)
                    client.FavoriteVideo2 = 0;
                if (client.FavoriteVideo3 == id)
                    client.FavoriteVideo3 = 0;

                // Put the client back.
                string jsonForPost = JsonConvert.SerializeObject(client);
                httpClient = new HttpClient();
                url = $"{GlobalSettings.baseEndpoint}/clients/{client.ClientId}";
                response = await httpClient.PutAsync(url, new StringContent(jsonForPost, Encoding.UTF8, "application/json"));
            }

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

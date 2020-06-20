using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SPFWebsitMVC.Models;

namespace SPFWebsitMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<SPFWebsitMVC.Models.Admin> Admin { get; set; }
        public DbSet<SPFWebsitMVC.Models.Client> Client { get; set; }
        public DbSet<SPFWebsitMVC.Models.Trainer> Trainer { get; set; }
    }
}

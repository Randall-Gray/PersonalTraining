using Microsoft.EntityFrameworkCore;
using PersonalTraining.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalTraining.Data
{
    public class ApplicationDBContext : DbContext
    {
        // Member variables
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<BroadcastMessage> BroadcastMessages { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ExerciseClass> ExerciseClasses { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Video> Videos { get; set; }

        // Constructor
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {

        }

        // Member methods
    }
}

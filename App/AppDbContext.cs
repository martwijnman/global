using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace App
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Application> Applications { get; set; }
        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            // building the database
            optionsBuilder.UseMySql(
                "server=localhost;user=root;password=;database=csd_Agenda",
                ServerVersion.Parse("8.0.30"));
        }

        // models are required here
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Type>().HasData(
                new Type { Id = 1, Name = "Web Application" },
                new Type { Id = 2, Name = "Desktop Application" },
                new Type { Id = 3, Name = "Mobile Application" },
                new Type { Id = 4, Name = "API" }
            );

            // CLIENTS
            modelBuilder.Entity<Client>().HasData(
                new Client { Id = 1, Name = "Microsoft" },
                new Client { Id = 2, Name = "Google" },
                new Client { Id = 3, Name = "Amazon" },
                new Client { Id = 4, Name = "Tesla" },
                new Client { Id = 5, Name = "Netflix" }
            );

            // APPLICATIONS
            modelBuilder.Entity<Application>().HasData(
                new Application
                {
                    Id = 1,
                    Name = "Customer Portal",
                    Description = "Web portal for customers",
                    TypeId = 1
                },
                new Application
                {
                    Id = 2,
                    Name = "Inventory Manager",
                    Description = "Desktop tool for managing stock",
                    TypeId = 2
                },
                new Application
                {
                    Id = 3,
                    Name = "Delivery Tracker",
                    Description = "Mobile application for tracking deliveries",
                    TypeId = 3
                },
                new Application
                {
                    Id = 4,
                    Name = "Payment API",
                    Description = "API for processing payments",
                    TypeId = 4
                },
                new Application
                {
                    Id = 5,
                    Name = "Employee Dashboard",
                    Description = "Internal dashboard for employees",
                    TypeId = 1
                }
            );
        }

    }
}


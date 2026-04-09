using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TradeHubApp.Data
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductUser> ProductUsers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // building the database
            optionsBuilder.UseMySql(
                "server=localhost;user=root;password=;database=csd_TradeHubApp",
                ServerVersion.Parse("8.0.30"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>()
        .HasOne(p => p.User)
        .WithMany(u => u.Products)
        .HasForeignKey(p => p.UserId)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Buyer)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany()
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductUser>()
                .HasOne(pu => pu.Product)
                .WithMany()
                .HasForeignKey(pu => pu.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductUser>()
                .HasOne(pu => pu.User)
                .WithMany()
                .HasForeignKey(pu => pu.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // USER SEED
            // =========================

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "admin", Password = BCrypt.Net.BCrypt.HashPassword("1234"), Role = "Admin", Activated = true },
                new User { Id = 2, Name = "moderator", Password = BCrypt.Net.BCrypt.HashPassword("1234"), Role = "Moderator", Activated = true },
                new User { Id = 3, Name = "mart", Password = BCrypt.Net.BCrypt.HashPassword("1234"), Role = "User", Activated = true },
                new User { Id = 4, Name = "yassin", Password = BCrypt.Net.BCrypt.HashPassword("1234"), Role = "User", Activated = true },
                new User { Id = 5, Name = "amina", Password = BCrypt.Net.BCrypt.HashPassword("1234"), Role = "User", Activated = false },
                new User { Id = 6, Name = "samir", Password = BCrypt.Net.BCrypt.HashPassword("1234"), Role = "User", Activated = true },
                new User { Id = 7, Name = "nora", Password = BCrypt.Net.BCrypt.HashPassword("1234"), Role = "User", Activated = true },
                new User { Id = 8, Name = "bilal", Password = BCrypt.Net.BCrypt.HashPassword("1234"), Role = "User", Activated = false }
            );

            // =========================
            // PRODUCT SEED
            // =========================

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Gaming Laptop", Price = 950, Description = "Sterke laptop voor gaming en werk", UserId = 3 },
                new Product { Id = 2, Name = "iPhone 13", Price = 600, Description = "Tweedehands iPhone in nette staat", UserId = 3 },
                new Product { Id = 3, Name = "Monitor 27 inch", Price = 180, Description = "Full HD monitor voor setup", UserId = 4 },
                new Product { Id = 4, Name = "Mechanisch Toetsenbord", Price = 75, Description = "RGB keyboard met blauwe switches", UserId = 4 },
                new Product { Id = 5, Name = "Gaming Muis", Price = 40, Description = "Lichte muis voor snelle reacties", UserId = 5 },
                new Product { Id = 6, Name = "Office Chair", Price = 120, Description = "Comfortabele bureaustoel", UserId = 6 },
                new Product { Id = 7, Name = "PlayStation 5", Price = 500, Description = "PS5 met controller", UserId = 6 },
                new Product { Id = 8, Name = "Bluetooth Speaker", Price = 55, Description = "Draadloze speaker met goed geluid", UserId = 7 },
                new Product { Id = 9, Name = "Smartwatch", Price = 90, Description = "Slim horloge met sportfuncties", UserId = 7 },
                new Product { Id = 10, Name = "Tablet", Price = 210, Description = "Tablet voor films en school", UserId = 8 },
                new Product { Id = 11, Name = "USB Microfoon", Price = 65, Description = "Goede microfoon voor streamen", UserId = 3 },
                new Product { Id = 12, Name = "Webcam HD", Price = 35, Description = "Webcam voor meetings", UserId = 4 }
            );

            // =========================
            // REVIEW SEED
            // =========================

            modelBuilder.Entity<Review>().HasData(
                new Review { Id = 1, Rating = 5, Comment = "Top product, werkt perfect", ProductId = 1 },
                new Review { Id = 2, Rating = 4, Comment = "Ziet er goed uit en werkt prima", ProductId = 1 },
                new Review { Id = 3, Rating = 3, Comment = "Best oke maar batterij minder", ProductId = 2 },
                new Review { Id = 4, Rating = 5, Comment = "Heel scherp beeld", ProductId = 3 },
                new Review { Id = 5, Rating = 4, Comment = "Typt erg lekker", ProductId = 4 },
                new Review { Id = 6, Rating = 2, Comment = "Niet mijn smaak", ProductId = 5 },
                new Review { Id = 7, Rating = 5, Comment = "Super comfortabel", ProductId = 6 },
                new Review { Id = 8, Rating = 5, Comment = "Console werkt als een beest", ProductId = 7 },
                new Review { Id = 9, Rating = 4, Comment = "Goed geluid voor deze prijs", ProductId = 8 },
                new Review { Id = 10, Rating = 3, Comment = "Leuk ding maar batterij snel leeg", ProductId = 9 },
                new Review { Id = 11, Rating = 4, Comment = "Prima tablet voor media", ProductId = 10 },
                new Review { Id = 12, Rating = 5, Comment = "Heel helder geluid", ProductId = 11 }
            );

            // =========================
            // ORDER SEED
            // =========================

            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, BuyerId = 4, ProductId = 1, IsPaid = true },
                new Order { Id = 2, BuyerId = 5, ProductId = 2, IsPaid = false },
                new Order { Id = 3, BuyerId = 3, ProductId = 3, IsPaid = true },
                new Order { Id = 4, BuyerId = 6, ProductId = 4, IsPaid = true },
                new Order { Id = 5, BuyerId = 7, ProductId = 5, IsPaid = false },
                new Order { Id = 6, BuyerId = 8, ProductId = 6, IsPaid = true },
                new Order { Id = 7, BuyerId = 3, ProductId = 7, IsPaid = false },
                new Order { Id = 8, BuyerId = 4, ProductId = 8, IsPaid = true },
                new Order { Id = 9, BuyerId = 5, ProductId = 9, IsPaid = true },
                new Order { Id = 10, BuyerId = 6, ProductId = 10, IsPaid = false },
                new Order { Id = 11, BuyerId = 7, ProductId = 11, IsPaid = true },
                new Order { Id = 12, BuyerId = 8, ProductId = 12, IsPaid = true }
            );

            // =========================
            // PRODUCTUSER SEED
            // =========================

            modelBuilder.Entity<ProductUser>().HasData(
                new ProductUser { Id = 1, ProductId = 1, UserId = 4 },
                new ProductUser { Id = 2, ProductId = 2, UserId = 5 },
                new ProductUser { Id = 3, ProductId = 3, UserId = 3 },
                new ProductUser { Id = 4, ProductId = 4, UserId = 6 },
                new ProductUser { Id = 5, ProductId = 5, UserId = 7 },
                new ProductUser { Id = 6, ProductId = 6, UserId = 8 },
                new ProductUser { Id = 7, ProductId = 7, UserId = 3 },
                new ProductUser { Id = 8, ProductId = 8, UserId = 4 },
                new ProductUser { Id = 9, ProductId = 9, UserId = 5 },
                new ProductUser { Id = 10, ProductId = 10, UserId = 6 },
                new ProductUser { Id = 11, ProductId = 11, UserId = 7 },
                new ProductUser { Id = 12, ProductId = 12, UserId = 8 }
            );
        }
    } 
}

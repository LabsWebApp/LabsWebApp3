using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LabsWebApp3.Models.Domain.Entities;

namespace LabsWebApp3.Models.Domain
{
    public class EFAppDbContext : IdentityDbContext<IdentityUser>
    {
        public EFAppDbContext(DbContextOptions<EFAppDbContext> options) : base(options) { }

        public DbSet<TextField> TextFields { get; set; }
        public DbSet<EventItem> EventItems { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "C3BD297D-2AEC-4582-B679-FDA3AA5164D3",
                Name = "admin",
                NormalizedName = "ADMIN"
            });

            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = "A8B0919E-FA64-4F08-89C5-A37B5F003C00",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@email.com",
                NormalizedEmail = "ADMIN@EMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "password"),
                SecurityStamp = string.Empty
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "C3BD297D-2AEC-4582-B679-FDA3AA5164D3",
                UserId = "A8B0919E-FA64-4F08-89C5-A37B5F003C00"
            });

            modelBuilder.Entity<TextField>().HasData(new TextField { 
                Id = new Guid("A543BCFD-B9EE-4584-A729-54D639A29535"), 
                CodeWord = "HomePage", 
                Title = "Главная"
            });
            modelBuilder.Entity<TextField>().HasData(new TextField
            {
                Id = new Guid("1CBFB4CB-D7C9-4C36-8187-D1A411C643B7"), 
                CodeWord = "EventsPage", 
                Title = "Наши события"
            });
            modelBuilder.Entity<TextField>().HasData(new TextField
            {
                Id = new Guid("7698042D-A1DB-4190-BB09-CC8954954CED"), 
                CodeWord = "ContactsPage", 
                Title = "Контакты"
            });
        }
    }
}

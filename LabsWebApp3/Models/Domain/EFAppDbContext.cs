using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LabsWebApp3.Models.Domain.Entities;

namespace XandCo.Domain
{
    public class EFAppDbContext : IdentityDbContext<IdentityUser>
    {
        public EFAppDbContext(DbContextOptions<EFAppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<TextField> TextFields { get; set; }
        public DbSet<EventItem> EventItems { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TextField>().HasData(new TextField { 
                Id = new Guid("A543BCFD-B9EE-4584-A729-54D639A29535"), 
                CodeWord = "HomePage", 
                Title = "Главная"
            });
            modelBuilder.Entity<TextField>().HasData(new TextField
            {
                Id = new Guid("1CBFB4CB-D7C9-4C36-8187-D1A411C643B7"), 
                CodeWord = "EventsPage", 
                Title = "Наши новости и события"
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

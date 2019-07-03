using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Models
{
    public class PayrollDbContext : DbContext
    {
        public PayrollDbContext(DbContextOptions<PayrollDbContext> options) : base(options){ }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentService> DocumentService { get; set; }
        public DbSet<Assigner> Assigners { get; set; }
        public DbSet<UsdExchangeRate> UsdExchangeRates { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Username);

            modelBuilder.Entity<UserProfile>().HasKey(us => us.VAT);

            modelBuilder.Entity<DocumentService>().HasKey(ds => new { ds.DocumentId, ds.ServiceId });
            modelBuilder.Entity<DocumentService>().HasOne(ds => ds.Document).WithMany(document => document.Services).HasForeignKey(ds => ds.DocumentId);
            modelBuilder.Entity<DocumentService>().HasOne(ds => ds.Service).WithMany(service => service.Documents).HasForeignKey(ds => ds.ServiceId);

            modelBuilder.Entity<UsdExchangeRate>().HasKey(usdExchange => usdExchange.Date);

            base.OnModelCreating(modelBuilder);
        }
    }
}

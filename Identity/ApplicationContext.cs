using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
using MyApiProject.Models; 
using Hastane_Otomasyonu.Identity;
using Microsoft.EntityFrameworkCore;
namespace Hastane_Otomasyonu.Identity
{
    public class ApplicationContext : IdentityDbContext<Users>
    {
        public ApplicationContext(DbContextOptions options)
            : base(options)
            {
                
            }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        }
    public DbSet<Employee> Employees { get; set; }
    }
}
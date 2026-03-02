using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
using MyApiProject.Models; 
using Hastane_Otomasyonu.Identity;
namespace Hastane_Otomasyonu.Identity
{
    public class ApplicationContext : IdentityDbContext<Users>
    {
        
    }
}
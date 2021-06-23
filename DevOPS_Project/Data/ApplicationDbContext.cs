 using DevOPS_Project.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevOPS_Project.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)
        {
         
           
        }

        public DbSet <Building> Building { get; set; }

        public DbSet <Room> Room { get; set; }

        public DbSet<ToolUser> ToolUser { get; set; }

        public DbSet<Reservation> Reservation { get; set; }
    }
}

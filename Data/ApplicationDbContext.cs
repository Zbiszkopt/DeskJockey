using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DeskJockey.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<DeskJockey.Models.User> Users { get; set; }
        public DbSet<DeskJockey.Models.Desk> Desks { get; set; }
        public DbSet<DeskJockey.Models.Reservation> Reservations { get; set; }
    }
}
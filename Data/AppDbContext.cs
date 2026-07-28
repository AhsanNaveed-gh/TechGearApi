using Microsoft.EntityFrameworkCore;
using TechGearAPI.Models;
using TechGearAPI.DTOs;
using TechGearAPI.Controllers;
namespace TechGearAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

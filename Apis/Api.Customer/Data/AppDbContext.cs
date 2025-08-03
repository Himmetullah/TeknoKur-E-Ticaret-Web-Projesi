using Api.Customer.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Customer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 
        }

        public DbSet<Urun> TblUrun { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Urun>().ToTable("TblUrun");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using WoofAdopciones.Shared.Entities;

namespace WoofAdopciones.Backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Pet> Pets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Pet>().HasIndex(c => c.Name).IsUnique();
        }
    }
}

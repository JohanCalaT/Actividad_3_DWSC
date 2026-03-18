using Microsoft.EntityFrameworkCore;
using UniversidadAPI.Models;

namespace UniversidadAPI.Data
{
    public class UniversidadContext : DbContext
    {
        public UniversidadContext(DbContextOptions<UniversidadContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>().ToTable("sampleusers");
        }
    }
}

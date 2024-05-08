using ChallengeAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace ChallengeAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tarjeta> Tarjetas { get; set; }
        public DbSet<Operacion> Operaciones{ get; set; }
    }
}

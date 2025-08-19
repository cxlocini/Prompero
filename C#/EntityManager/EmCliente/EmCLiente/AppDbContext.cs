using Microsoft.EntityFrameworkCore;

namespace ControleDeProdutos
{
    public class AppDbContext : DbContext
    {
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
            var connectionString = "Host=localhost;Database=TOCC8;Username=postgres;Password=1234";
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
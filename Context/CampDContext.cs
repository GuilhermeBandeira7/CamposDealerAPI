using CamposDealerApp.Model;
using Microsoft.EntityFrameworkCore;

namespace CamposDealerApp.Context
{
    public class CampDContext : DbContext
    {
        public CampDContext(DbContextOptions<CampDContext> options) 
            : base(options)
        {
        }

        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Cliente { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=CamposDealerDB;User id=sa;Password=Senha@mtw;Trusted_Connection=False; TrustServerCertificate=True", builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
            base.OnConfiguring(optionsBuilder);
        }
    }
}

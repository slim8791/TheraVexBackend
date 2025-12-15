using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TheravexBackend.Models;

namespace TheravexBackend.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<Article> Articles => Set<Article>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<StockMouvement> StockMouvements => Set<StockMouvement>();
        public DbSet<Facture> Factures => Set<Facture>();
        public DbSet<FactureLigne> FactureLignes => Set<FactureLigne>();
        public DbSet<Tva> Tvas => Set<Tva>();

    }
}
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
        public DbSet<BonCommande> BonCommandes => Set<BonCommande>();
        public DbSet<BonCommandeLigne> BonCommandeLignes => Set<BonCommandeLigne>();
        public DbSet<Tva> Tvas => Set<Tva>();
        public DbSet<Fournisseur> Fournisseur { get; set; } = default!;
        public DbSet<Lot> Lot { get; set; } = default!;
        public DbSet<Vente> Vente { get; set; } = default!;
        public DbSet<LigneVente> LigneVente { get; set; } = default!;
        
    }
}
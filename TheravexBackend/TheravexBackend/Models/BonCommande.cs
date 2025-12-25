using TheravexBackend.DTOs;

namespace TheravexBackend.Models
{
    public class BonCommande
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int FournisseurId { get; set; }
        public Fournisseur Fournisseur { get; set; } = null!;

        public decimal Total { get; set; }
        public ICollection<BonCommandeLigne> Lignes { get; set; } = new List<BonCommandeLigne>();
        public int Numero { get; set; }
        public string FullNumber { get; set; }
        public decimal TotalHT { get; set; }
        public decimal TotalTVA { get; set; }
        public decimal TotalTTC { get; set; }
    }
}

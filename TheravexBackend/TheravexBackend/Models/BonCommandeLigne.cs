using System.Text.Json.Serialization;

namespace TheravexBackend.Models
{
    public class BonCommandeLigne
    {
        public int Id { get; set; }

        public int BonCommandeId { get; set; }
        [JsonIgnore]
        public BonCommande BonCommande { get; set; } = null!;

        public int ArticleId { get; set; }
        public Article Article { get; set; } = null!;

        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }

        // TVA
        public decimal TauxTva { get; set; }
        public decimal MontantTva { get; set; }

        public decimal TotalHT { get; set; }
        public decimal TotalTTC { get; set; }
    }
}

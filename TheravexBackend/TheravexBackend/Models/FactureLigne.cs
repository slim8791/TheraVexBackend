using System.Text.Json.Serialization;

namespace TheravexBackend.Models
{
    public class FactureLigne
    {
        public int Id { get; set; }

        public int FactureId { get; set; }
        [JsonIgnore]
        public Facture Facture { get; set; } = null!;

        public int ArticleId { get; set; }
        public Article Article { get; set; } = null!;

        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }

        // TVA
        public decimal TauxTva { get; set; }
        public decimal MontantTva { get; set; }

        public decimal TotalHT { get; set; }
        public decimal TotalTTC { get; set; }
        public decimal Remise { get; set; }
    }
}

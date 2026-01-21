using System.Text.Json.Serialization;

namespace TheravexBackend.Models
{
    public class Lot
    {
        public int LotId { get; set; }
        public string Numero { get; set; }
        public DateTime? DateProduction { get; set; }
        public DateTime? DateExpiration { get; set; }
        public int? Quantite { get; set; }
        public int? ArticleId { get; set; }
        //[JsonIgnore]
        public Article? Article { get; set; }
        public int? FournisseurId { get; set; }
        [JsonIgnore]
        public Fournisseur? Fournisseur { get; set; }
    }
}

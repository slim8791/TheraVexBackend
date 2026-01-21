using System.Text.Json.Serialization;

namespace TheravexBackend.Models
{
    public class LigneVente
    {
        public int LigneVenteId { get; set; }
        public int ArticleId { get; set; }
        public int SellQuantity { get; set; }
        public decimal Discount { get; set; }
        public int LotId { get; set; }
        public int? VenteId { get; set; }
        [JsonIgnore]
        public virtual Vente? Vente { get; set; }
    }
}

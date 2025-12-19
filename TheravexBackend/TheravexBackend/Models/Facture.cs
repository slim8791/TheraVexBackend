using TheravexBackend.DTOs;

namespace TheravexBackend.Models
{
    public class Facture
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public TypeDocument TypeDocument { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public decimal Total { get; set; }
        public ICollection<FactureLigne> Lignes { get; set; } = new List<FactureLigne>();
        public int Numero { get; set; }
        public string FullNumber { get; set; }
        public decimal TotalHT { get; set; }
        public decimal TotalTVA { get; set; }
        public decimal TotalTTC { get; set; }
    }
}

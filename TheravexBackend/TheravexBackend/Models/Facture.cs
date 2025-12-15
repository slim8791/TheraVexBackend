namespace TheravexBackend.Models
{
    public class Facture
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public decimal Total { get; set; }
        public ICollection<FactureLigne> Lignes { get; set; } = new List<FactureLigne>();
        public string Numero { get; set; }
    }
}

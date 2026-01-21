namespace TheravexBackend.Models
{
    public class Vente
    {
        public int VenteId { get; set; }
        public int ClientId { get; set; }
        public List<LigneVente> Lines { get; set; }
        public DateTime Date { get; set; }
    }
}

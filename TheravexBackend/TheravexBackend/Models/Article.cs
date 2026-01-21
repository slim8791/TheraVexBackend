namespace TheravexBackend.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Nom { get; set; } = null!;
        public decimal PrixAchat { get; set; }
        public decimal PrixVente { get; set; }
        public int Stock { get; set; }

        // 🔗 TVA
        public int TvaId { get; set; }
        public Tva? Tva { get; set; } = null!;

        public ICollection<Lot> Lots { get; set; } = new List<Lot>();
    }
}

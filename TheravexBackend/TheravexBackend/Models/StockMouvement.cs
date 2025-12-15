namespace TheravexBackend.Models
{
    public enum TypeMouvement
    {
        Entree = 1,
        Sortie = 2
    }

    public class StockMouvement
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; } = null!;
        public int Quantite { get; set; }
        public TypeMouvement Type { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}

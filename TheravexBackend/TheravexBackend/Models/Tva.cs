namespace TheravexBackend.Models
{
    public class Tva
    {
        public int Id { get; set; }

        // Ex: 0, 7, 13, 19
        public decimal Taux { get; set; }

        public string Libelle { get; set; } = null!;

        public bool Actif { get; set; } = true;
    }
}

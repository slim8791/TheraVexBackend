namespace TheravexBackend.Models
{
    public class Fournisseur
    {
        public int FournisseurId { get; set; }
        public string RaisonSociale { get; set; }
        public string? MatriculeFiscale { get; set; }
        public string? Adresse { get; set; }
        public string? Telephone { get; set; }
        public string? Telephone2 { get; set; }
        public string? Email { get; set; }
    }
}

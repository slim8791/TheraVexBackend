namespace TheravexBackend.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Nom { get; set; } = null!;
        public string Telephone { get; set; } = null!;
        public string? Adresse { get; set; }
        public string? MatriculeFiscale { get; set; }
    }
}

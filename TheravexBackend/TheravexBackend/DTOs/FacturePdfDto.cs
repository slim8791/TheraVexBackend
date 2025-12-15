namespace TheravexBackend.DTOs
{
    public class FacturePdfDto
    {
        public string Numero { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Client { get; set; } = null!;
        public List<FacturePdfLigneDto> Lignes { get; set; } = new();
        public decimal TotalHT { get; set; }
        public decimal TotalTVA { get; set; }
        public decimal TotalTTC { get; set; }
    }

    public class FacturePdfLigneDto
    {
        public string Article { get; set; } = null!;
        public int Quantite { get; set; }
        public decimal Remise { get; set; }
        public decimal PrixUnitaire { get; set; }
        public decimal TotalHT { get; set; }
        public decimal TVA { get; set; }
        public decimal TotalTTC { get; set; }
    }
}

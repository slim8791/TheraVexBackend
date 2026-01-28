namespace TheravexBackend.DTOs
{
    public class FacturePdfDto
    {
        public TypeDocument TypeDocument { get; set; }
        public int? Numero { get; set; } = 1!;
        public string? FullNumber { get; set; }
        public DateTime Date { get; set; }
        public int ClientId { get; set; } 
        public List<FacturePdfLigneDto> Lignes { get; set; }
        public decimal? TotalHT { get; set; }
        public decimal? TotalTVA { get; set; }
        public decimal? TotalTTC { get; set; }
    }

    public class FacturePdfLigneDto
    {
        public int ArticleId { get; set; } 
        public int Quantite { get; set; }
        public decimal Remise { get; set; }
        public decimal PrixUnitaire { get; set; }
        public decimal TotalHT { get; set; }
        public decimal TVA { get; set; }
        public decimal TotalTTC { get; set; }
    }


    public class BonCommandeePdfDto
    {
        public int? Numero { get; set; } = 1!;
        public string? FullNumber { get; set; }
        public DateTime Date { get; set; }
        public string Fournisseur { get; set; } = null!;
        public List<BonCommandePdfLigneDto> Lignes { get; set; }
        public decimal TotalHT { get; set; }
        public decimal TotalTVA { get; set; }
        public decimal TotalTTC { get; set; }
    }

    public class BonCommandePdfLigneDto
    {
        public string Article { get; set; } = null!;
        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }
        public decimal TotalHT { get; set; }
        public decimal TVA { get; set; }
        public decimal TotalTTC { get; set; }
    }

    public enum TypeDocument
    {
        BL,
        FCT,
        BS,
        Avoir
    }
}

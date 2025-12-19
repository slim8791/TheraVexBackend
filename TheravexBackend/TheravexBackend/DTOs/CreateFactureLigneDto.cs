using System.ComponentModel.DataAnnotations;

namespace TheravexBackend.DTOs
{
    public class CreateFactureLigneDto
    {
        [Required] public int ArticleId { get; set; }
        [Required, Range(1, int.MaxValue)] public int Quantite { get; set; }
        public decimal? PrixUnitaire { get; set; }
        public decimal Remise { get; set; } = 0;
    }
}

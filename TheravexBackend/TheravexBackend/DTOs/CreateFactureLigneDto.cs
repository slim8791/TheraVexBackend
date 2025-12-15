using System.ComponentModel.DataAnnotations;

namespace TheravexBackend.DTOs
{
    public class CreateFactureLigneDto
    {
        [Required] public int ArticleId { get; set; }
        [Required, Range(1, int.MaxValue)] public int Quantite { get; set; }
        [Range(0, double.MaxValue)] public decimal? PrixUnitaire { get; set; }
        [Range(0, 100)] public decimal Remise { get; set; } = 0;
    }
}

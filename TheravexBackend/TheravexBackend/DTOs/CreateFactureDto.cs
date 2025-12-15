using System.ComponentModel.DataAnnotations;

namespace TheravexBackend.DTOs
{
    public class CreateFactureDto
    {
        [Required] public int ClientId { get; set; }
        [Range(0, 100)] public decimal Remise { get; set; } = 0;
        [Required, MinLength(1)] public List<CreateFactureLigneDto> Lignes { get; set; } = new();
    }
}

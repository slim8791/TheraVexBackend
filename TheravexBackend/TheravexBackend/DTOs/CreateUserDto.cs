using System.ComponentModel.DataAnnotations;

namespace TheravexBackend.DTOs
{
    public class CreateUserDto
    {
        [Required, MinLength(3)] public string UserName { get; set; } = null!;
        [Required, EmailAddress] public string Email { get; set; } = null!;
        [Required, MinLength(6)] public string Password { get; set; } = null!;
        [Required] public string Role { get; set; } = null!;
    }

}

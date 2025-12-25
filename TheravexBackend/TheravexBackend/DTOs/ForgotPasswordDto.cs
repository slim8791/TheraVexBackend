namespace TheravexBackend.DTOs
{
    public class ForgotPasswordDto
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
}

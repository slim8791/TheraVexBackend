namespace TheravexBackend.DTOs
{
    public class ChangeUserRoleDto
    {
        public string UserId { get; set; } = null!;
        public string NewRole { get; set; } = null!;
    }
}

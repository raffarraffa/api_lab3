
namespace api.Models;
public class LoginDto
{
    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    //public string? Id { get; set; } = null!;
}
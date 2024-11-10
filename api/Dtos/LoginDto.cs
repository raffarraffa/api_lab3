
namespace api.Models;
public class LoginDto
{
    [Required]
    public string Email { get; set; } = null!;

    //[Required]
    public string? Password { get; set; }
    public string? Token { get; set; }

}
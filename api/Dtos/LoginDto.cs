
namespace api.Models;
public class LoginDto
{
    public string? Email { get; set; }    
    public string? Password { get; set; }
    public string? Token { get; set; }
    public int? Otp { get ; set;}
    public override string ToString()
        {
            return $"Email: {Email}, Password: {Password}, Token: {Token}, Otp: {Otp}";
        }

}
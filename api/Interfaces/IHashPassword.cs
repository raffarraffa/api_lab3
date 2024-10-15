namespace tpi.Interfaces;

public interface IHashPassword
{
    string HashingPassword(string password);
    bool VerifyHashedPassword(string Password, string hashedPassword);
}
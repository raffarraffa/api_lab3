using BCrypt.Net;
namespace api.Services;

public static class HashPassword
    {
        public static string HashingPassword(string password)
        {
                return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool isValidPassword(string password, string hashedPassword)
        {       
            try
            {
                return BCrypt.Net.BCrypt.Verify(password.Trim(), hashedPassword);
            }   
            catch (BCrypt.Net.SaltParseException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
         }
}

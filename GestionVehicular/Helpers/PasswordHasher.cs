using System.Security.Cryptography;
using System.Text;

namespace GestionVehicular.Helpers;

public class PasswordHasher
{
    // Método para generar el hash de una contraseña
    public static string HashPassword(string password)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2")); // Convertir a formato hexadecimal
            }
            return builder.ToString();
        }
    }

    // Método para comparar una contraseña con su hash guardado
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        string hashedInput = HashPassword(password);
        return string.Compare(hashedInput, hashedPassword, StringComparison.OrdinalIgnoreCase) == 0;
    }
}
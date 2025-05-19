using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace BackEnd.Helpers
{
    public class Helper
    {
        public Helper() { }

        // Constante para el factor de trabajo (Work Factor) de BCrypt
        private const int WorkFactor = 12;

        public static string HashearPassword(string passwordUsuario)
        {
            if (string.IsNullOrWhiteSpace(passwordUsuario))
                return string.Empty;
            return BCrypt.Net.BCrypt.HashPassword(passwordUsuario, WorkFactor);
        }

        public static bool VerificarPassword(string passwordUsuario, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordUsuario) || string.IsNullOrWhiteSpace(passwordHash))
                return false;
            return BCrypt.Net.BCrypt.Verify(passwordUsuario, passwordHash);
        }

        public static string GenerarPassword(int longitud)
        {
            const string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+[]{}|;:,.<>?";
            StringBuilder password = new StringBuilder();
            Random random = new Random();
            password.Append(caracteres[random.Next(26, 52)]);
            password.Append(caracteres[random.Next(62, 72)]);
            password.Append(random.Next(10));
            for (int i = 3; i < longitud; i++)
            {
                password.Append(caracteres[random.Next(caracteres.Length)]);
            }
            return password.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace ItilPaymentFlow.Application.Security
{
    public static class PasswordHasher
    {
        public static string Hash(string password)
        {
            byte[] salt = new byte[128 / 8];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);

            byte[] hashed = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100_000,
                numBytesRequested: 256 / 8);

            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hashed)}";
        }

        public static bool Verify(string hashedWithSalt, string password)
        {
            var parts = hashedWithSalt.Split('.', 2);
            if (parts.Length != 2) return false;

            var salt = Convert.FromBase64String(parts[0]);
            var expectedHash = parts[1];

            byte[] hashed = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100_000,
                numBytesRequested: 256 / 8);

            var hashedBase64 = Convert.ToBase64String(hashed);
            return CryptographicOperations.FixedTimeEquals(Convert.FromBase64String(hashedBase64), Convert.FromBase64String(expectedHash));
        }
    }
}
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography;

namespace HomeBanking.Helpers
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 10000;
        private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
        private readonly char Delimiter = ';';

        public string Hash(string password)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var salt = new byte[SaltSize];
                rng.GetBytes(salt);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, _hashAlgorithmName))
                {
                    var hash = pbkdf2.GetBytes(KeySize);

                    return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
                }
            }
        }

        public bool Verify(string passwordHashed, string inputPassword)
        {
            var elements = passwordHashed.Split(Delimiter);
            var salt = Convert.FromBase64String(elements[0]);
            var hash = Convert.FromBase64String(elements[1]);

            using (var pbkdf2 = new Rfc2898DeriveBytes(inputPassword, salt, Iterations, _hashAlgorithmName))
            {
                var hashedInput = pbkdf2.GetBytes(KeySize);
                return CryptographicOperations.FixedTimeEquals(hash, hashedInput);
            }
        }
    }
}

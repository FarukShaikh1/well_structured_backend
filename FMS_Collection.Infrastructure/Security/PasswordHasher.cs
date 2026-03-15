using FMS_Collection.Core.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace FMS_Collection.Infrastructure.Security
{
    /// <summary>
    /// PBKDF2-SHA512 password hasher. Registered as <see cref="IPasswordHasher"/> in DI.
    /// Hash format: <c>HEX(salt):HEX(hash):iterations</c>
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        private const int Iterations = 350_000;
        private const int SaltSize  = 32;
        private const int HashSize  = 64;

        public string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                Iterations,
                HashAlgorithmName.SHA512,
                HashSize);

            return $"{Convert.ToHexString(salt)}:{Convert.ToHexString(hash)}:{Iterations}";
        }

        public bool Verify(string password, string storedHash)
        {
            if (string.IsNullOrWhiteSpace(storedHash)) return false;

            var parts = storedHash.Split(':');

            // Detect legacy SHA1 hash (40 hex chars, no colons)
            if (parts.Length == 1 && parts[0].Length == 40)
                return VerifyLegacySha1(password, parts[0]);

            if (parts.Length != 3) return false;

            var salt       = Convert.FromHexString(parts[0]);
            var hash       = Convert.FromHexString(parts[1]);
            var iterations = int.Parse(parts[2]);

            var inputHash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                HashAlgorithmName.SHA512,
                HashSize);

            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }

        /// <summary>
        /// Verifies against old SHA1 hashes during migration period.
        /// Remove once all passwords have been re-hashed to PBKDF2.
        /// </summary>
        public bool VerifyLegacySha1(string password, string sha1Hash)
        {
            using var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
            var computed = Convert.ToHexString(hash).ToLowerInvariant();
            return string.Equals(computed, sha1Hash, StringComparison.OrdinalIgnoreCase);
        }
    }
}

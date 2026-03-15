namespace FMS_Collection.Core.Interfaces
{
    /// <summary>Abstracts PBKDF2 password hashing so the Application layer has no dependency on Infrastructure.</summary>
    public interface IPasswordHasher
    {
        /// <summary>Returns a PBKDF2-SHA512 hash in the format <c>HEX(salt):HEX(hash):iterations</c>.</summary>
        string Hash(string password);

        /// <summary>Verifies a plain-text password against a stored PBKDF2 hash.</summary>
        bool Verify(string password, string storedHash);

        /// <summary>
        /// Verifies against the legacy SHA1 hash format.
        /// Used only during the migration window; remove once all passwords are re-hashed.
        /// </summary>
        bool VerifyLegacySha1(string password, string sha1Hash);
    }
}

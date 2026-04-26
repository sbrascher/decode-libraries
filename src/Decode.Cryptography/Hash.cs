using System.Security.Cryptography;
using System.Text;

namespace Decode.Cryptography;

/// <summary>
/// Provides secure methods for password hashing, HMAC, and SHA256 operations.
/// </summary>
public static class Hash
{
    /// <summary> Default size for the salt (36 bytes). </summary>
    public const int DEFAULT_SALT_SIZE = 36;
    /// <summary> Default size for the hash output (36 bytes). </summary>
    public const int DEFAULT_HASH_SIZE = 36;
    /// <summary> 
    /// Default number of iterations for PBKDF2 (100,000). 
    /// This value follows modern security recommendations to resist GPU brute-force attacks.
    /// </summary>
    public const int DEFAULT_ITERATIONS = 100_000;

    /// <summary>
    /// Generates a cryptographically secure random salt and returns it as a Base64 string.
    /// </summary>
    public static string GenerateSaltToBase64String(int size = DEFAULT_SALT_SIZE)
    {
        byte[] salt = new byte[size];
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetNonZeroBytes(salt);
        return Convert.ToBase64String(salt);
    }

    /// <summary>
    /// Hashes a password using PBKDF2 (HMAC-SHA256) and returns it as a Base64 string.
    /// This is the recommended method for storing passwords.
    /// </summary>
    public static string HashPasswordToBase64String(string password, string base64Salt, int iterations = DEFAULT_ITERATIONS, int hashSize = DEFAULT_HASH_SIZE)
    {
        byte[] salt = Convert.FromBase64String(base64Salt);
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

#if NET7_0_OR_GREATER
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(passwordBytes, salt, iterations, HashAlgorithmName.SHA256, hashSize);
        return Convert.ToBase64String(hash);
#else
        using Rfc2898DeriveBytes pbkdf2 = new(passwordBytes, salt, iterations, HashAlgorithmName.SHA256);
        return Convert.ToBase64String(pbkdf2.GetBytes(hashSize));
#endif
    }

    /// <summary>
    /// Computes a SHA256 hash and returns it as a hexadecimal string.
    /// </summary>
    public static string ComputeSha256ToHexString(string content, int iterations = 1)
    {
        return ComputeSha256ToHexString(Encoding.UTF8.GetBytes(content), iterations);
    }

    /// <summary>
    /// Computes a SHA256 hash and returns it as a hexadecimal string.
    /// </summary>
    public static string ComputeSha256ToHexString(byte[] content, int iterations = 1)
    {
        byte[] hash = ComputeSha256ToByteArray(content, iterations);
        return Utils.ToHexString(hash);
    }

    /// <summary>
    /// Computes a SHA256 hash and returns it as a byte array.
    /// </summary>
    public static byte[] ComputeSha256ToByteArray(byte[] content, int iterations = 1)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] hash = content;
        for (int i = 0; i < iterations; i++)
        {
            hash = sha256.ComputeHash(hash);
        }
        return hash;
    }

    /// <summary>
    /// Computes an HMAC-SHA256 and returns it as a Base64 string.
    /// </summary>
    public static string ComputeHmacSha256ToBase64String(string hexKey, string content)
    {
        byte[] key = Utils.FromHexString(hexKey);
        byte[] contentBytes = Encoding.UTF8.GetBytes(content);
        return Convert.ToBase64String(ComputeHmacSha256ToByteArray(key, contentBytes));
    }

    /// <summary>
    /// Computes an HMAC-SHA256 and returns it as a byte array.
    /// </summary>
    public static byte[] ComputeHmacSha256ToByteArray(byte[] key, byte[] content)
    {
        using HMACSHA256 hmac = new(key);
        return hmac.ComputeHash(content);
    }

    /// <summary>
    /// Verifies if a SHA256 hash matches the provided content using constant-time comparison.
    /// </summary>
    public static bool VerifySha256(string content, string hashToVerify, int iterations = 1)
    {
        return VerifySha256(Encoding.UTF8.GetBytes(content), hashToVerify, iterations);
    }

    /// <summary>
    /// Verifies if a SHA256 hash matches the provided content using constant-time comparison.
    /// </summary>
    public static bool VerifySha256(byte[] content, string hashToVerify, int iterations = 1)
    {
        byte[] computedHash = ComputeSha256ToByteArray(content, iterations);
        byte[] hashToVerifyBytes = Utils.FromHexString(hashToVerify);

        return CryptographicOperations.FixedTimeEquals(computedHash, hashToVerifyBytes);
    }

    /// <summary>
    /// Verifies if an HMAC-SHA256 matches the provided content using constant-time comparison.
    /// </summary>
    public static bool VerifyHmacSha256(string hexKey, string content, string base64HashToVerify)
    {
        byte[] key = Utils.FromHexString(hexKey);
        byte[] contentBytes = Encoding.UTF8.GetBytes(content);
        byte[] computedHash = ComputeHmacSha256ToByteArray(key, contentBytes);
        byte[] hashToVerifyBytes = Convert.FromBase64String(base64HashToVerify);

        return CryptographicOperations.FixedTimeEquals(computedHash, hashToVerifyBytes);
    }
}

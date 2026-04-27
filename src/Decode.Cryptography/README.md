# Decode.Cryptography

Modern and secure cryptography utilities for .NET, providing a clean API for common cryptographic operations.

## 🚀 Features

- **Secure Password Hashing (PBKDF2):** Key derivation using HMAC-SHA256 with 100,000 iterations by default (OWASP recommended).
- **Timing Attack Protection:** Uses `CryptographicOperations.FixedTimeEquals` for all verification methods.
- **HMAC:** Message authentication using SHA256.
- **SHA256:** Standard hashing with support for multiple iterations.
- **HEX & Base64:** Optimized conversions for cryptographic data using modern .NET primitives.
- **Modern .NET:** Fully optimized for .NET 8/9 using `Span<byte>` and native HEX conversions.

## 📖 Usage

### Hashing a Password (PBKDF2)

This is the recommended way to store user passwords.

```csharp
using Decode.Cryptography;

// Generate a unique salt for the user
string salt = Hash.GenerateSaltToBase64String();

// Hash the password with the salt
string hash = Hash.HashPasswordToBase64String("user-password", salt);

// Store both 'salt' and 'hash' in your database
```

### Computing HMAC-SHA256

```csharp
string hmac = Hash.ComputeHmacSha256ToBase64String(hexKey, "content");
```

### HEX Conversions

```csharp
byte[] data = [0xDE, 0xAD, 0xBE, 0xEF];

// Convert to HEX string
string hex = Utils.ToHexString(data);

// Convert back to byte array
byte[] back = Utils.FromHexString(hex);
```

## 🔒 Security Note

This library follows modern security standards:
- **No MD5/SHA1:** Insecure algorithms are not included.
- **Constant-Time Comparison:** Prevents side-channel timing attacks during hash verification.
- **High Iteration Count:** Protects against modern GPU-based brute-force attacks.

## 📄 License
MIT License.

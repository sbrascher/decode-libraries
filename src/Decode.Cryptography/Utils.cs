using System.Text;

namespace Decode.Cryptography;

/// <summary>
/// Utility methods for cryptographic operations.
/// </summary>
public static class Utils
{
    /// <summary>
    /// Converts a byte array to its hexadecimal string representation.
    /// </summary>
    public static string ToHexString(byte[] bytes)
    {
#if NET5_0_OR_GREATER
        return Convert.ToHexString(bytes);
#else
        StringBuilder stringBuilder = new(bytes.Length * 2);
        foreach (byte b in bytes)
        {
            stringBuilder.Append(b.ToString("X2"));
        }
        return stringBuilder.ToString();
#endif
    }

    /// <summary>
    /// Converts a hexadecimal string to its byte array representation.
    /// </summary>
    public static byte[] FromHexString(string hexString)
    {
#if NET5_0_OR_GREATER
        return Convert.FromHexString(hexString);
#else
        if (hexString.Length % 2 != 0)
        {
            throw new ArgumentException("Invalid hex string length.");
        }

        byte[] array = new byte[hexString.Length / 2];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        }
        return array;
#endif
    }

    /// <summary>
    /// Combines two byte arrays into a single one.
    /// </summary>
    public static byte[] CombineArray(ReadOnlySpan<byte> first, ReadOnlySpan<byte> second)
    {
        byte[] result = new byte[first.Length + second.Length];
        first.CopyTo(result);
        second.CopyTo(result.AsSpan(first.Length));
        return result;
    }

    /// <summary>
    /// Splits a byte array into two at the specified length.
    /// </summary>
    public static void SplitArray(ReadOnlySpan<byte> content, int length, out byte[] first, out byte[] second)
    {
        if (content.IsEmpty)
        {
            throw new ArgumentException("Content cannot be empty.", nameof(content));
        }

        if (length > content.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Length exceeds content size.");
        }

        first = content[..length].ToArray();
        second = content[length..].ToArray();
    }
}

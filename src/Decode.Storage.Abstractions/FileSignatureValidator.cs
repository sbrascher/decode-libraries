namespace Decode.Storage.Abstractions;

/// <summary>
/// Professional implementation of <see cref="IFileValidator"/> that inspects file magic numbers (signatures)
/// and detects executable blocklists to prevent MIME-spoofing attacks.
/// </summary>
public class FileSignatureValidator : IFileValidator
{
    // Common file signatures database
    private static readonly Dictionary<string, byte[][]> FileSignatures = new(StringComparer.OrdinalIgnoreCase)
    {
        { ".png",  [[0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A]] },
        { ".jpeg", [[0xFF, 0xD8, 0xFF]] },
        { ".jpg",  [[0xFF, 0xD8, 0xFF]] },
        { ".gif",  [[0x47, 0x49, 0x46, 0x38]] },
        { ".pdf",  [[0x25, 0x50, 0x44, 0x46]] },
        { ".zip",  [[0x50, 0x4B, 0x03, 0x04], [0x50, 0x4B, 0x05, 0x06], [0x50, 0x4B, 0x07, 0x08]] },
        { ".docx", [[0x50, 0x4B, 0x03, 0x04]] }, // OpenXML Word (technically a zip)
        { ".xlsx", [[0x50, 0x4B, 0x03, 0x04]] }, // OpenXML Excel
        { ".pptx", [[0x50, 0x4B, 0x03, 0x04]] }, // OpenXML PowerPoint
        { ".rar",  [[0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00], [0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x01, 0x00]] },
        { ".7z",   [[0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C]] },
        { ".bmp",  [[0x42, 0x4D]] },
        { ".mp3",  [[0x49, 0x44, 0x33], [0xFF, 0xFB], [0xFF, 0xF3], [0xFF, 0xF2]] },
        { ".wav",  [[0x52, 0x49, 0x46, 0x46]] },
        { ".xml",  [[0x3C, 0x3F, 0x78, 0x6D, 0x6C]] } // <?xml
    };

    // Blocklist of dangerous executable signatures (regardless of allowed extension)
    private static readonly byte[][] BlockedSignatures =
    [
        [0x4D, 0x5A],             // Windows PE Executable/DLL (MZ)
        [0x7F, 0x45, 0x4C, 0x46],       // Linux ELF Executable
        [0x23, 0x21],             // Script Shebang (#!)
        [0xCA, 0xFE, 0xBA, 0xBE],       // Java Class / Mach-O Fat Binary
        [0xFE, 0xED, 0xFA, 0xCE],       // Mach-O Executable (32-bit)
        [0xFE, 0xED, 0xFA, 0xCF],       // Mach-O Executable (64-bit)
        [0xCE, 0xFA, 0xED, 0xFE],       // Mach-O Executable (reverse byte order 32-bit)
        [0xCF, 0xFA, 0xED, 0xFE]        // Mach-O Executable (reverse byte order 64-bit)
    ];

    /// <inheritdoc />
    public async Task<bool> IsValidAsync(
        Stream stream,
        string fileName,
        IEnumerable<string> allowedExtensions,
        CancellationToken cancellationToken = default)
    {
        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentNullException(nameof(fileName));
        }

        if (allowedExtensions == null)
        {
            throw new ArgumentNullException(nameof(allowedExtensions));
        }

        // 1. Basic Extension Check (prevent processing unallowed extensions)
        string extension = Path.GetExtension(fileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
        {
            return false;
        }

        // 2. Validate Stream and Seeking Compatibility
        if (!stream.CanSeek)
        {
            throw new InvalidOperationException("The file validation requires a seekable stream to avoid consuming the content stream permanently.");
        }

        if (stream.Length == 0)
        {
            return false; // Reject empty files as a security baseline
        }

        // Read the first 16 bytes (enough to match our signatures database)
        byte[] header = new byte[16];
        long originalPosition = stream.Position;

        int bytesRead = 0;
        int totalRead = 0;

        try
        {
            while (totalRead < header.Length)
            {
                bytesRead = await stream.ReadAsync(header, totalRead, header.Length - totalRead, cancellationToken);
                if (bytesRead == 0)
                {
                    break;
                }
                totalRead += bytesRead;
            }
        }
        finally
        {
            // Always return the stream back to its original position
            stream.Position = originalPosition;
        }

        // 3. Prevent Executable Spoofing (Universal Blocklist check)
        foreach (byte[] blockedSig in BlockedSignatures)
        {
            if (MatchesSignature(header, totalRead, blockedSig))
            {
                return false; // Executable signature detected - instant rejection
            }
        }

        // 4. Validate extension signature matches Magic Numbers
        if (FileSignatures.TryGetValue(extension, out byte[][]? signatures))
        {
            foreach (byte[] sig in signatures)
            {
                if (MatchesSignature(header, totalRead, sig))
                {
                    return true; // Match found
                }
            }

            // The extension exists in our verification base but the signature doesn't match
            return false;
        }

        // If the extension is allowed but we don't have its signature database mapping,
        // we allow it since it has cleared the executable blocklist validation.
        return true;
    }

    private static bool MatchesSignature(byte[] header, int bytesRead, byte[] signature)
    {
        if (bytesRead < signature.Length)
        {
            return false;
        }

        for (int i = 0; i < signature.Length; i++)
        {
            if (header[i] != signature[i])
            {
                return false;
            }
        }

        return true;
    }
}

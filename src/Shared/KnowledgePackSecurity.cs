using System.Security.Cryptography;
using System.Text;

namespace DotnetExpertAdvisor.Shared;

public sealed record KnowledgePackEnvelope(string Version, string ContentHash, string Signature, string Payload);

public static class KnowledgePackSecurity
{
    public static KnowledgePackEnvelope Create(string version, string payload, ReadOnlySpan<byte> key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(version);
        ArgumentNullException.ThrowIfNull(payload);
        var bytes = Encoding.UTF8.GetBytes(payload);
        var hash = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
        var signature = Convert.ToBase64String(HMACSHA256.HashData(key,
            Encoding.UTF8.GetBytes(version + "\n" + hash)));
        return new(version, hash, signature, payload);
    }

    public static bool Verify(KnowledgePackEnvelope pack, ReadOnlySpan<byte> key)
    {
        ArgumentNullException.ThrowIfNull(pack);
        var bytes = Encoding.UTF8.GetBytes(pack.Payload);
        var hash = Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
        var signature = Convert.ToBase64String(HMACSHA256.HashData(key,
            Encoding.UTF8.GetBytes(pack.Version + "\n" + hash)));
        return CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(hash), Encoding.UTF8.GetBytes(pack.ContentHash)) &&
            CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(signature), Encoding.UTF8.GetBytes(pack.Signature));
    }
}

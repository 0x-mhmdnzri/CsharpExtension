using System.Text.Json;

namespace DotnetExpertAdvisor.Shared;

public sealed record KnowledgePackContent(int SchemaVersion, string MinimumEngineVersion,
    KnowledgeConcept[] Concepts, KnowledgeSource[] Sources, KnowledgeClaim[] Claims, KnowledgeRule[] Rules);
public sealed record KnowledgeUpdateState(KnowledgePackEnvelope? Current, KnowledgePackEnvelope? Previous);

// shortcut: pack versions are numeric releases; add SemVer prerelease ordering before preview pack distribution.
public sealed class KnowledgeUpdateClient(string statePath, Version engineVersion, byte[] key)
{
    private readonly string statePath = Path.GetFullPath(statePath);
    private readonly byte[] key = key.ToArray();

    public KnowledgeUpdateState ReadState()
        => File.Exists(statePath)
            ? JsonSerializer.Deserialize<KnowledgeUpdateState>(File.ReadAllText(statePath))
                ?? throw new InvalidDataException("Empty update state.")
            : new(null, null);

    public void Install(KnowledgePackEnvelope pack)
    {
        Validate(pack);
        var state = ReadState();
        if (state.Current is not null)
        {
            Validate(state.Current);
            if (Version.Parse(pack.Version) <= Version.Parse(state.Current.Version))
                throw new InvalidDataException("Updates must have a newer version; use rollback to restore the previous pack.");
        }
        Save(new(pack, state.Current));
    }

    public void Rollback()
    {
        var state = ReadState();
        if (state.Previous is null) throw new InvalidOperationException("No previous pack is available.");
        Validate(state.Previous);
        Save(new(state.Previous, null));
    }

    public async Task UpdateAsync(HttpClient client, Uri url, CancellationToken cancellationToken = default)
    {
        if (url.Scheme != Uri.UriSchemeHttps) throw new ArgumentException("Update URL must use HTTPS.", nameof(url));
        using var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();
        await response.Content.LoadIntoBufferAsync(8 * 1024 * 1024, cancellationToken);
        var pack = JsonSerializer.Deserialize<KnowledgePackEnvelope>(await response.Content.ReadAsStringAsync(cancellationToken))
            ?? throw new InvalidDataException("Empty update response.");
        cancellationToken.ThrowIfCancellationRequested();
        Install(pack);
    }

    private void Validate(KnowledgePackEnvelope pack)
    {
        if (!KnowledgePackSecurity.Verify(pack, key)) throw new InvalidDataException("Knowledge pack authentication failed.");
        if (!Version.TryParse(pack.Version, out _)) throw new InvalidDataException("Invalid pack version.");
        var content = JsonSerializer.Deserialize<KnowledgePackContent>(pack.Payload)
            ?? throw new InvalidDataException("Empty knowledge pack.");
        if (content.SchemaVersion != 1 || !Version.TryParse(content.MinimumEngineVersion, out var minimum) || engineVersion < minimum)
            throw new InvalidDataException("Knowledge pack is incompatible with this engine.");
        _ = new KnowledgeRegistry(content.Concepts, content.Sources, content.Claims, content.Rules);
    }

    private void Save(KnowledgeUpdateState state)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(statePath)!);
        var temporary = statePath + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            File.WriteAllText(temporary, JsonSerializer.Serialize(state));
            File.Move(temporary, statePath, overwrite: true);
        }
        finally { if (File.Exists(temporary)) File.Delete(temporary); }
    }
}

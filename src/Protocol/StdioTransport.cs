using System.Buffers;
using System.Text;
using System.Text.Json;

namespace DotnetExpertAdvisor.Protocol;

public static class StdioTransport
{
    public static async ValueTask<JsonDocument?> ReadAsync(Stream input, CancellationToken cancellationToken = default)
    {
        var headers = new StringBuilder();
        var one = new byte[1];
        while (true)
        {
            var read = await input.ReadAsync(one, cancellationToken);
            if (read == 0) return null;
            headers.Append((char)one[0]);
            if (headers.Length >= 4 && headers.ToString().EndsWith("\r\n\r\n", StringComparison.Ordinal)) break;
            if (headers.Length > 8192) throw new InvalidDataException("LSP headers exceed the limit.");
        }
        var length = headers.ToString().Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault(line => line.StartsWith("Content-Length:", StringComparison.OrdinalIgnoreCase));
        if (length is null || !int.TryParse(length[15..].Trim(), out var contentLength) || contentLength < 0)
            throw new InvalidDataException("Missing or invalid Content-Length.");
        var payload = ArrayPool<byte>.Shared.Rent(contentLength);
        try
        {
            var offset = 0;
            while (offset < contentLength)
            {
                var count = await input.ReadAsync(payload.AsMemory(offset, contentLength - offset), cancellationToken);
                if (count == 0) throw new EndOfStreamException("LSP payload ended early.");
                offset += count;
            }
            // JsonDocument retains the input bytes; do not return the pooled buffer while it is still referenced.
            return JsonDocument.Parse(payload.AsMemory(0, contentLength).ToArray());
        }
        finally { ArrayPool<byte>.Shared.Return(payload); }
    }

    public static async ValueTask WriteAsync(Stream output, object message,
        CancellationToken cancellationToken = default)
    {
        var payload = JsonSerializer.SerializeToUtf8Bytes(message);
        var header = Encoding.ASCII.GetBytes($"Content-Length: {payload.Length}\r\n\r\n");
        await output.WriteAsync(header, cancellationToken);
        await output.WriteAsync(payload, cancellationToken);
        await output.FlushAsync(cancellationToken);
    }
}

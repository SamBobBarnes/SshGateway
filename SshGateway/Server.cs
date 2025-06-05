using System.Text.Json.Serialization;

namespace SshGateway;

public record Server
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }
    [JsonPropertyName("host")]
    public required string Host { get; init; }
    [JsonPropertyName("port")]
    public int Port { get; init; } = 22;
    [JsonPropertyName("user")]
    public required string User { get; init; }
    [JsonPropertyName("pathToKey")]
    public string? PathToKey { get; init; }
}
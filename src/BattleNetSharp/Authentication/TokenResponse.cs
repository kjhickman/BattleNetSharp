using System.Text.Json.Serialization;

namespace BattleNetSharp.Authentication;

internal sealed class TokenResponse
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("expires_in")]
    public long SecondsUntilTokenExpiration { get; set; }
}

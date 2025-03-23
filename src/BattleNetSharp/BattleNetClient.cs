using System.Net.Http.Json;
using BattleNetSharp.Authentication;

namespace BattleNetSharp;

public class BattleNetClient : IBattleNetClient
{
    private readonly HttpClient _httpClient;
    private readonly HttpContent _authRequestContent;
    private string? _accessToken;
    private DateTimeOffset? _tokenExpiration;

    public BattleNetClient(string clientId, string clientSecret)
    {
        ArgumentNullException.ThrowIfNull(clientId, nameof(clientId));
        ArgumentNullException.ThrowIfNull(clientSecret, nameof(clientSecret));

        _authRequestContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret
        });
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(Constants.BattleNetBaseUrl)
        };
    }

    public async ValueTask AuthenticateAsync(bool force = false, CancellationToken token = default)
    {
        var now = DateTimeOffset.UtcNow;
        if (_tokenExpiration > now && !force)
        {
            return;
        }

        var request = new HttpRequestMessage(HttpMethod.Post, Constants.BattleNetOAuthUrl)
        {
            Content = _authRequestContent
        };

        var response = await _httpClient.SendAsync(request, token);
        response.EnsureSuccessStatusCode();

        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken: token);
        if (tokenResponse != null)
        {
            _accessToken = tokenResponse.AccessToken;
            _tokenExpiration = now.AddSeconds(tokenResponse.SecondsUntilTokenExpiration);
        }
    }
}

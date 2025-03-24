namespace BattleNetSharp.Apis.ClassicWow;

public class ClassicWowApi
{
    private readonly HttpClient _httpClient;

    public ClassicWowApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetAuctions(CancellationToken token = default)
    {
        var uri = new Uri("data/wow/connected-realm/100?namespace=dynamic-us&locale=en_US");

        var httpResponse = await _httpClient.GetAsync(uri, token);
        httpResponse.EnsureSuccessStatusCode();
        return await httpResponse.Content.ReadAsStringAsync(token);
    }
}

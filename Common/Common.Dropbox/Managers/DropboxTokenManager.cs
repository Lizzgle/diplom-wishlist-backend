using System.Text;
using System.Text.Json;
using Common.Dropbox.Models;
using Common.Dropbox.Options;
using Microsoft.Extensions.Options;

namespace Common.Dropbox.Managers;

public class DropboxTokenManager(IOptions<DropboxOptions> dropboxOptions)
{ 
    private readonly DropboxOptions _dropboxOptions = dropboxOptions.Value;
    private string? _accessToken;
    private DateTime _expiresAt;

    public async Task<string> GetAccessTokenAsync()
    {
        if (string.IsNullOrEmpty(_accessToken) || DateTime.UtcNow >= _expiresAt)
        {
            await RefreshAccessTokenAsync();
        }

        return _accessToken!;
    }

    private async Task RefreshAccessTokenAsync()
    {
        using var client = new HttpClient();

        var request = CreateRefreshTokenRequest();

        var tokenResponse = await FetchTokenResponseAsync(client, request);

        if (tokenResponse is null)
        {
            throw new InvalidOperationException("Token response is null. The operation could not be completed.");
        }

        _accessToken = tokenResponse.AccessToken;

        _expiresAt = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);
    }

    private HttpRequestMessage CreateRefreshTokenRequest()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, _dropboxOptions.TokenUrl);

        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_dropboxOptions.ClientId}:{_dropboxOptions.ClientSecret}"));

        request.Headers.Add("Authorization", $"Basic {credentials}");

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("refresh_token", _dropboxOptions.RefreshToken)
        });

        request.Content = content;

        return request;
    }

    private async Task<DropboxTokenResponse?> FetchTokenResponseAsync(HttpClient client, HttpRequestMessage request)
    {
        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<DropboxTokenResponse>(jsonResponse);
    }
}
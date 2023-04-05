using PianoLessons.Auth0;
using System.Net.Http.Headers;

namespace PianoLessons;

class TokenHandler : DelegatingHandler
{
    private readonly Auth0Client auth0Client;

    public TokenHandler(Auth0Client auth0client)
    {
        auth0Client = auth0client;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await auth0Client.GetToken();
        var refreshToken = await auth0Client.GetRefreshToken();

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        var responseMessage = await base.SendAsync(request, cancellationToken);

        if (responseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized
            && refreshToken != null)
        {
            var refreshResult = await auth0Client.RefreshTokenAsync(refreshToken);

            if (!refreshResult.IsError)
            {
                request.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", refreshResult.AccessToken);

                responseMessage = await base.SendAsync(request, cancellationToken);
            }
        }

        return responseMessage;
    }
}

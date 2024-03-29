﻿using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser; 
using IdentityModel.Client;
using System.Security.Claims;
using IdentityModel.OidcClient.Results;

namespace PianoLessons.Auth0;

public class Auth0Client
{
    private readonly OidcClient oidcClient;
    private string audience;

    public Auth0Client(Auth0ClientOptions options)
    {
        oidcClient = new OidcClient(new OidcClientOptions
        {
            Authority = $"https://{options.Domain}",
            ClientId = options.ClientId,
            Scope = options.Scope,
            RedirectUri = options.RedirectUri,
            Browser = options.Browser
        });

        audience = options.Audience;
    }

    public IdentityModel.OidcClient.Browser.IBrowser Browser
    {
        get
        {
            return oidcClient.Options.Browser;
        }
        set
        {
            oidcClient.Options.Browser = value;
        }
    }

    public async Task<LoginResult> LoginAsync()
    {
        var loginRequest = new LoginRequest
        {
            FrontChannelExtraParameters = new Parameters(new Dictionary<string, string>()
            {
                {"audience", audience}
            })
        };

        var loginResult = await oidcClient.LoginAsync(loginRequest);

        if (!loginResult.IsError)
        {
            await SecureStorage.Default.SetAsync("access_token", loginResult.AccessToken);
            await SecureStorage.Default.SetAsync("id_token", loginResult.IdentityToken);

            if (loginResult.RefreshToken != null)
            {
                await SecureStorage.Default.SetAsync("refresh_token", loginResult.RefreshToken);
            }
        }

        return loginResult;
    }

    public async Task<BrowserResult> LogoutAsync()
    {
        var logoutParameters = new Dictionary<string, string>
    {
      {"client_id", oidcClient.Options.ClientId },
      {"returnTo", oidcClient.Options.RedirectUri }
    };

        var logoutRequest = new LogoutRequest();
        var endSessionUrl = new RequestUrl($"{oidcClient.Options.Authority}/v2/logout")
          .Create(new Parameters(logoutParameters));
        var browserOptions = new BrowserOptions(endSessionUrl, oidcClient.Options.RedirectUri)
        {
            Timeout = TimeSpan.FromSeconds(logoutRequest.BrowserTimeout),
            DisplayMode = logoutRequest.BrowserDisplayMode
        };

        var browserResult = await oidcClient.Options.Browser.InvokeAsync(browserOptions);

        SecureStorage.Default.RemoveAll();

        return browserResult;
    }

    public async Task<ClaimsPrincipal> GetAuthenticatedUser()
    {
        ClaimsPrincipal user = null;

        var refreshToken = await GetRefreshToken();
        if (refreshToken != null)
        {
            await RefreshTokenAsync(await GetRefreshToken());
        }

        var idToken = await SecureStorage.Default.GetAsync("id_token");
        if (idToken != null)
        {
            var doc = await new HttpClient().GetDiscoveryDocumentAsync(oidcClient.Options.Authority);
            var validator = new JwtHandlerIdentityTokenValidator();
            var options = new OidcClientOptions
            {
                ClientId = oidcClient.Options.ClientId,
                ProviderInformation = new ProviderInformation
                {
                    IssuerName = doc.Issuer,
                    KeySet = doc.KeySet
                }
            };

            var validationResult = await validator.ValidateAsync(idToken, options);

            if (!validationResult.IsError) user = validationResult.User;
        }

        return user;
    }

    public async Task<RefreshTokenResult> RefreshTokenAsync(string refreshToken)
    {
        var refreshResult = await oidcClient.RefreshTokenAsync(refreshToken);

        if (!refreshResult.IsError)
        {
            await SecureStorage.Default.SetAsync("access_token", refreshResult.AccessToken);
            await SecureStorage.Default.SetAsync("id_token", refreshResult.IdentityToken);

            if (refreshResult.RefreshToken != null)
            {
                await SecureStorage.Default.SetAsync("refresh_token", refreshResult.RefreshToken);
            }
        }

        return refreshResult;
    }

    public async Task<string> GetToken()
    {
        return await SecureStorage.Default.GetAsync("access_token");
    }

    public async Task<string> GetRefreshToken()
    {
        return await SecureStorage.Default.GetAsync("refresh_token");
    }
}
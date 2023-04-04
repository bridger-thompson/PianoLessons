using PianoLessons.Auth0;
using PianoLessons.Shared.Data;
using System.Security.Claims;

namespace PianoLessons.Services;

public enum LoginResult
{
    Success,
    Error,
    UserNotRegistered
}

public class AuthService
{
    private readonly Auth0Client auth0Client;
    private readonly PianoLessonsService service;
    private ClaimsPrincipal identityUser;

    public PianoLessonsUser User { get; private set; }

    public IdentityModel.OidcClient.Browser.IBrowser Browser
    {
        set => auth0Client.Browser = value;
    }

    public AuthService(Auth0Client auth0Client, PianoLessonsService service)
    {
        this.auth0Client = auth0Client;
        this.service = service;
    }

    public async Task<LoginResult> LoginAsync()
    {
        var results = await auth0Client.LoginAsync();
        identityUser = results.User;
        var userId = identityUser.Claims.FirstOrDefault(c => c.Type == "sub").Value;
        User = await service.GetUser(userId);

        if (User == null)
        {
            return LoginResult.UserNotRegistered;
        }

        return LoginResult.Success;
    }

    public async Task RegisterUser(bool isTeacher, string name)
    {
        var userId = identityUser.Claims.FirstOrDefault(c => c.Type == "sub").Value;
        var user = new PianoLessonsUser(userId, name, isTeacher);
        await service.RegisterUser(user);
        User = await service.GetUser(userId);
    }

    public async Task Logout()
    {
        await auth0Client.LogoutAsync();
        User = null;
    }

    public async Task<LoginResult> SilentLogin()
    {
        identityUser = await auth0Client.GetAuthenticatedUser();

        if (identityUser != null)
        {
            var userId = identityUser.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            User = await service.GetUser(userId);

            return User == null ? LoginResult.UserNotRegistered : LoginResult.Success;
        }

        return LoginResult.Error;
    }
}

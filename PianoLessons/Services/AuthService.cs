using IdentityModel.OidcClient;
using PianoLessons.Auth0;
using PianoLessons.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
}

using PianoLessons.Auth0;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoLessons.Services;

public class AuthService
{
    private readonly Auth0Client auth0Client;

    public IdentityModel.OidcClient.Browser.IBrowser Browser 
    {
        set => auth0Client.Browser = value;
    }

    public AuthService(Auth0Client auth0Client) 
    {
        this.auth0Client = auth0Client;
    }

    public async Task<IdentityModel.OidcClient.LoginResult> LoginAsync()
    {
        return await auth0Client.LoginAsync();  
    }
}

using PianoLessons.Auth0;
using PianoLessons.Services;
using PianoLessons.ViewModels;

namespace PianoLessons.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModel vm, IAuthService auth)
	{
		InitializeComponent();
#if WINDOWS
	auth.Browser = new WebViewBrowserAuthenticator(WebViewInstance);
#endif
        BindingContext = vm;	
	}
}
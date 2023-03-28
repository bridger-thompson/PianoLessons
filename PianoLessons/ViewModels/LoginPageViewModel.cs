using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Auth0;
using PianoLessons.Pages;
using PianoLessons.Services;

namespace PianoLessons.ViewModels
{
    public partial class LoginPageViewModel : ObservableObject
    {

        [ObservableProperty]
        private bool loginViewIsVisible;

        [ObservableProperty]
        private bool homeViewIsVisible;

        private readonly AuthService auth;
        private readonly INavigationService nav;

        public LoginPageViewModel(AuthService auth, INavigationService nav)
        {
            this.auth = auth;
            this.nav = nav;
        }

        [RelayCommand]
        public async void Login()
        {
            var loginResult = await auth.LoginAsync();

            if (!loginResult.IsError)
            {
                LoginViewIsVisible = false;
                HomeViewIsVisible = true;

                await nav.NavigateToAsync($"///{nameof(SchedulePage)}");
            }
        }
    }
}

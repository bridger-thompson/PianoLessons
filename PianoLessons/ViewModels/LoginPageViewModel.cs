using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Pages;
using PianoLessons.Services;

namespace PianoLessons.ViewModels
{
    public partial class LoginPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool loginViewIsVisible;

        [ObservableProperty]
        private bool registrationViewIsVisible;

        [ObservableProperty]
        private bool isTeacher;

        [ObservableProperty]
        private string userName;

        private readonly AuthService auth;
        private readonly INavigationService nav;
        private readonly PianoLessonsService service;

        public LoginPageViewModel(AuthService auth, INavigationService nav, PianoLessonsService service)
        {
            this.auth = auth;
            this.nav = nav;
            this.service = service;
            LoginViewIsVisible = true;
        }

        [RelayCommand]
        public async Task Login()
        {
            var loginResult = await auth.LoginAsync();

            if (loginResult == LoginResult.Success)
            {
                await nav.NavigateToAsync($"///{nameof(SchedulePage)}");
            }
            if (loginResult == LoginResult.UserNotRegistered)
            {
                LoginViewIsVisible = false;
                RegistrationViewIsVisible = true;
            }
        }

        [RelayCommand]
        public async Task SubmitRegistration()
        {
            await auth.RegisterUser(IsTeacher, UserName);
            LoginViewIsVisible = true;
            RegistrationViewIsVisible = false;
            await nav.NavigateToAsync($"///{nameof(SchedulePage)}");
        }
    }
}

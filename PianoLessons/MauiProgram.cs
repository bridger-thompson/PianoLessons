using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PianoLessons.Pages;
using PianoLessons.Services;
using PianoLessons.ViewModels;
using PianoLessons.Auth0;
using Syncfusion.Maui.Core.Hosting;
using PianoLessons.Interfaces;
using System.Security.Principal;
using static System.Net.WebRequestMethods;

#if ANDROID || IOS
using PianoLessons.Platforms.Service;
#endif

namespace PianoLessons;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.ConfigureSyncfusionCore();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.UseMauiCommunityToolkitMediaElement()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		RegisterPagesAndViewModels(builder);
#if ANDROID || IOS
		builder.Services.AddTransient<IAudioPlayer, AudioPlayerService>();
		builder.Services.AddTransient<IRecordAudio, RecordAudioService>();
#endif
		builder.Services.AddSingleton<INavigationService, ShellNavigationService>();

		builder.Services.AddScoped<TokenHandler>();

		builder.Services.AddHttpClient("v1", c =>
			{
				c.BaseAddress = new Uri("https://pianolessonsapi.azurewebsites.net/");
				//c.BaseAddress = new Uri("https://localhost:7085");
				c.DefaultRequestHeaders.Add("version", "1.0");
			}
		).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler())
		 .AddHttpMessageHandler<TokenHandler>();

		builder.Services.AddHttpClient("v2", c =>
			{
				c.BaseAddress = new Uri("https://pianolessonsapi.azurewebsites.net/");
				//c.BaseAddress = new Uri("https://localhost:7085");
				c.DefaultRequestHeaders.Add("version", "2.0");
			}
		).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler())
		 .AddHttpMessageHandler<TokenHandler>();

		builder.Services.AddSingleton<PianoLessonsService>(provider =>
		{
			var clientV1 = provider.GetRequiredService<IHttpClientFactory>().CreateClient("v1");
			var clientV2 = provider.GetRequiredService<IHttpClientFactory>().CreateClient("v2");

			return new PianoLessonsService(clientV1, clientV2);
		});

		builder.Services.AddTransient(
			sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Api")
		);
		builder.Services.AddSingleton(new Auth0Client(new()
		{
			Domain = "dev-djtfumdg4bnzmj45.us.auth0.com",
			ClientId = "1JnFZheOsQFlyGigeF0MWjwKCLlfRnSu",
			Scope = "openid profile offline_access",
			Audience = "https://pianolessons/api",
#if WINDOWS
            RedirectUri = "http://localhost/callback"
#else
			RedirectUri = "myapp://callback"
#endif
		}));

		builder.Services.AddSingleton<AuthService>();

		builder.UseMauiCommunityToolkit();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	private static void RegisterPagesAndViewModels(MauiAppBuilder builder)
	{
		builder.Services.AddSingleton<SchedulePage>();
		builder.Services.AddSingleton<SchedulePageViewModel>();
		builder.Services.AddSingleton<AddLogPage>();
		builder.Services.AddSingleton<AddLogPageViewModel>();
		builder.Services.AddSingleton<PracticeLogPage>();
		builder.Services.AddSingleton<PracticeLogPageViewModel>();
		builder.Services.AddSingleton<ScoreboardPage>();
		builder.Services.AddSingleton<ScoreboardPageViewModel>();
		builder.Services.AddSingleton<AddScheduleItemPage>();
		builder.Services.AddSingleton<AddScheduleItemPageViewModel>();
		builder.Services.AddSingleton<ManageCoursesPage>();
		builder.Services.AddSingleton<ManageCoursesPageViewModel>();
		builder.Services.AddSingleton<CourseDetailPage>();
		builder.Services.AddSingleton<CourseDetailPageViewModel>();
		builder.Services.AddSingleton<RecordingPage>();
		builder.Services.AddSingleton<RecordingPageViewModel>();
		builder.Services.AddSingleton<LoginPage>();
		builder.Services.AddSingleton<LoginPageViewModel>();
	}
}

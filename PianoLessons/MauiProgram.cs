using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PianoLessons.Pages;
using PianoLessons.Services;
using PianoLessons.ViewModels;
using PianoLessons.Auth0;
using Syncfusion.Maui.Core.Hosting;
using PianoLessons.Interfaces;

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
		builder.Services.AddTransient<IRecordAudio, RecordAudioService>();
#endif
		builder.Services.AddSingleton<INavigationService, ShellNavigationService>();
		RegisterHttpClients(builder);

		builder.Services.AddSingleton<IPianoLessonsService, PianoLessonsService>(provider =>
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

		builder.Services.AddSingleton<IAuthService, AuthService>();

		builder.UseMauiCommunityToolkit();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();

	}

	private static void RegisterHttpClients(MauiAppBuilder builder)
	{
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
	}

	private static void RegisterPagesAndViewModels(MauiAppBuilder builder)
	{
		builder.Services.AddScoped<SchedulePage>();
		builder.Services.AddScoped<SchedulePageViewModel>();
		builder.Services.AddScoped<AddLogPage>();
		builder.Services.AddScoped<AddLogPageViewModel>();
		builder.Services.AddScoped<PracticeLogPage>();
		builder.Services.AddScoped<PracticeLogPageViewModel>();
		builder.Services.AddScoped<ScoreboardPage>();
		builder.Services.AddScoped<ScoreboardPageViewModel>();
		builder.Services.AddScoped<AddScheduleItemPage>();
		builder.Services.AddScoped<AddScheduleItemPageViewModel>();
		builder.Services.AddScoped<ManageCoursesPage>();
		builder.Services.AddScoped<ManageCoursesPageViewModel>();
		builder.Services.AddScoped<CourseDetailPage>();
		builder.Services.AddScoped<CourseDetailPageViewModel>();
		builder.Services.AddScoped<RecordingPage>();
		builder.Services.AddScoped<RecordingPageViewModel>();
		builder.Services.AddScoped<LoginPage>();
		builder.Services.AddScoped<LoginPageViewModel>();
	}
}

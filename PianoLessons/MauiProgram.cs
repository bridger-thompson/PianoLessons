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
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt/QHRqVVhkVFpFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF5jS35adkVmX3xccHZSQQ==;Mgo+DSMBPh8sVXJ0S0J+XE9AflRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS31TdEVqWX9bdndXRmRbVg==;ORg4AjUWIQA/Gnt2VVhkQlFacldJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxQdkdjUH9edXJWRGBaVUM=;MTA5NDYwOUAzMjMwMmUzNDJlMzBNTkthSzl0NEZEUzhYeEJhanBHYm1ibWllREFUbU40Q0s1L3ZhODN1V0JVPQ==;MTA5NDYxMEAzMjMwMmUzNDJlMzBQWG1SSmplSEFEQ28xQUhpcFhrcjNWT0V4YWhyc2lPcmpZQXF0UEt3L3U0PQ==;NRAiBiAaIQQuGjN/V0Z+WE9EaFtKVmJLYVB3WmpQdldgdVRMZVVbQX9PIiBoS35RdUVhWXZecHFTRWJdVEJ+;MTA5NDYxMkAzMjMwMmUzNDJlMzBkb1NqSEtPY0k1UHNCbkdvT1U3UXV3Mzd0akh5UW92S0Q3Vi9LNFhucWRVPQ==;MTA5NDYxM0AzMjMwMmUzNDJlMzBFTWVnN1BkMmJMM2U3RlY5SjM0OGxXeFZkQlpaMWVvaFF5cUJqMmlTMTFjPQ==;Mgo+DSMBMAY9C3t2VVhkQlFacldJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxQdkdjUH9edXJWRGNeV0Y=;MTA5NDYxNUAzMjMwMmUzNDJlMzBhRmFXWXhRU21PSmgybTZTa2JmRzlNbG05WFRUMEl5UzhQQzlaaGtDeEl3PQ==;MTA5NDYxNkAzMjMwMmUzNDJlMzBneEVlM0ROblIvaDdsMHVXc0pTQVRQWXlvbklwTUl6RFRNMWtmZXZkRWg0PQ==;MTA5NDYxN0AzMjMwMmUzNDJlMzBkb1NqSEtPY0k1UHNCbkdvT1U3UXV3Mzd0akh5UW92S0Q3Vi9LNFhucWRVPQ==");
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

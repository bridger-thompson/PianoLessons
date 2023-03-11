using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PianoLessons.Pages;
using PianoLessons.Services;
using PianoLessons.ViewModels;
using Syncfusion.Maui.Core.Hosting;

namespace PianoLessons;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.ConfigureSyncfusionCore();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

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
		builder.Services.AddSingleton<INavigationService, ShellNavigationService>();

		builder.UseMauiCommunityToolkit();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

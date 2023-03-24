using CommunityToolkit.Mvvm.Input;
using PianoLessons.Pages;
using PianoLessons.Services;

namespace PianoLessons;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(AddLogPage), typeof(AddLogPage));
		Routing.RegisterRoute(nameof(AddScheduleItemPage), typeof(AddScheduleItemPage));
		Routing.RegisterRoute(nameof(CourseDetailPage), typeof(CourseDetailPage));
	}
}

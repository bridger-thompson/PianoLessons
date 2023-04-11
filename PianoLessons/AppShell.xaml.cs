using PianoLessons.Pages;

namespace PianoLessons;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		if (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.Android)
		{
			recording.IsVisible = true;

		}
		else
		{
			recording.IsVisible = false;
		}

		Routing.RegisterRoute(nameof(AddLogPage), typeof(AddLogPage));
		Routing.RegisterRoute(nameof(AddScheduleItemPage), typeof(AddScheduleItemPage));
		Routing.RegisterRoute(nameof(CourseDetailPage), typeof(CourseDetailPage));
	}
}

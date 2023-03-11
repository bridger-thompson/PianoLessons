using PianoLessons.Pages;

namespace PianoLessons;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(AddLogPage), typeof(AddLogPage));
		Routing.RegisterRoute(nameof(AddScheduleItemPage), typeof(AddScheduleItemPage));
	}
}

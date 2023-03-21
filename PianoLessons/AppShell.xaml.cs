using CommunityToolkit.Mvvm.Input;
using PianoLessons.Pages;
using PianoLessons.Services;

namespace PianoLessons;

public partial class AppShell : Shell
{
	public bool IsTeacher;

	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(AddLogPage), typeof(AddLogPage));
		Routing.RegisterRoute(nameof(AddScheduleItemPage), typeof(AddScheduleItemPage));
		Routing.RegisterRoute(nameof(ManageStudentsPage), typeof(ManageStudentsPage));
	}

	public async Task Loaded()
	{
		var service = new PianoLessonsService(
			new HttpClient
			{
				BaseAddress = new Uri("http://localhost:5050")
			}
		);
		//user id
		IsTeacher = await service.IsTeacher(10);
	}
}

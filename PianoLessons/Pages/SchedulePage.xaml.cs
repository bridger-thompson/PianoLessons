using PianoLessons.ViewModels;
using Syncfusion.Maui.Scheduler;

namespace PianoLessons.Pages;

public partial class SchedulePage : ContentPage
{
	public SchedulePage(SchedulePageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}


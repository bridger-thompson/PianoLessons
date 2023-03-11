using PianoLessons.ViewModels;

namespace PianoLessons.Pages;

public partial class AddScheduleItemPage : ContentPage
{
	public AddScheduleItemPage(AddScheduleItemPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
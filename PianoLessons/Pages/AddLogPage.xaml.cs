using PianoLessons.ViewModels;

namespace PianoLessons.Pages;

public partial class AddLogPage : ContentPage
{
	public AddLogPage(AddLogPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
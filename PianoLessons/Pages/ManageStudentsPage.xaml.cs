using PianoLessons.ViewModels;

namespace PianoLessons.Pages;

public partial class ManageStudentsPage : ContentPage
{
	public ManageStudentsPage(ManageStudentsPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
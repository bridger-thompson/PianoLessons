using PianoLessons.ViewModels;

namespace PianoLessons.Pages;

public partial class ManageCoursesPage : ContentPage
{
	public ManageCoursesPage(ManageCoursesPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
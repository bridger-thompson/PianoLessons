using PianoLessons.ViewModels;

namespace PianoLessons.Pages;

public partial class CourseDetailPage : ContentPage
{
	public CourseDetailPage(CourseDetailPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
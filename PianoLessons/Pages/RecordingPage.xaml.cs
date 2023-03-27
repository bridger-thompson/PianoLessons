using PianoLessons.ViewModels;

namespace PianoLessons.Pages;

public partial class RecordingPage : ContentPage
{
	public RecordingPage(RecordingPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
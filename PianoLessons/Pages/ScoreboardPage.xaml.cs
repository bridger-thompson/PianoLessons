using PianoLessons.ViewModels;

namespace PianoLessons.Pages;

public partial class ScoreboardPage : ContentPage
{
	public ScoreboardPage(ScoreboardPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
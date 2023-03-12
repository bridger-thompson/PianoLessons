using PianoLessons.ViewModels;

namespace PianoLessons.Pages;

public partial class PracticeLogPage : ContentPage
{
	public PracticeLogPage(PracticeLogPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	private void CollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
	{

    }
}
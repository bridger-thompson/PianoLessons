using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Services;

namespace PianoLessons.ViewModels;

public partial class AddLogPageViewModel : ObservableObject
{
	private readonly INavigationService navService;

	[ObservableProperty]
	private DateTime date;

	[ObservableProperty, NotifyPropertyChangedFor(nameof(Total))]
	private int hours;

	[ObservableProperty, NotifyPropertyChangedFor(nameof(Total))]
	private int minutes;

	public string Total => $"{Hours} hour(s) {Minutes} minute(s)";

	public AddLogPageViewModel(INavigationService navService)
	{
		this.navService = navService;
		Date = DateTime.Today;
	}

	[RelayCommand]
	public async Task Submit()
	{
		await navService.NavigateToAsync("..");
	}
}

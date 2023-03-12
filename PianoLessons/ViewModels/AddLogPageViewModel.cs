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
	[NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
	private int hours;

	[ObservableProperty, NotifyPropertyChangedFor(nameof(Total))]
	[NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
	private int minutes;

	public string Total => $"{Hours} hour(s) {Minutes} minute(s)";

	public AddLogPageViewModel(INavigationService navService)
	{
		this.navService = navService;
		Date = DateTime.Today;
	}

	[RelayCommand(CanExecute = nameof(canSubmit))]
	public async Task Submit()
	{
		await navService.NavigateToAsync("..");
	}

	private bool canSubmit()
	{
		return Hours > 0 || Minutes > 0;
	}
}

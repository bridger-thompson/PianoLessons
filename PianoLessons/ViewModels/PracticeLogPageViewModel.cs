using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Models;
using PianoLessons.Pages;
using PianoLessons.Services;
using System.Collections.ObjectModel;

namespace PianoLessons.ViewModels;

public partial class PracticeLogPageViewModel : ObservableObject
{
	private readonly INavigationService navService;

	[ObservableProperty]
	private ObservableCollection<PracticeLog> logs;

	public PracticeLogPageViewModel(INavigationService navService)
	{
		Logs = new ObservableCollection<PracticeLog>()
		{
			new PracticeLog()
			{
				Name = "Bridger",
				Date = DateTime.Now,
				Duration = TimeSpan.FromHours(1),
			},
			new PracticeLog()
			{
				Name = "Bridger",
				Date = DateTime.Now,
				Duration = TimeSpan.FromMinutes(90),
			},
		};
		this.navService = navService;
	}

	[RelayCommand]
	public async Task ToAddLog()
	{
		await navService.NavigateToAsync($"{nameof(AddLogPage)}");
	}
}

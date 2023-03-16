using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Services;
using PianoLessons.Shared.Data;

namespace PianoLessons.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class AddLogPageViewModel : ObservableObject
{
	private readonly INavigationService navService;
	private readonly PianoLessonsService service;
	[ObservableProperty]
	private int id;

	[ObservableProperty]
	private DateTime date;

	[ObservableProperty, NotifyPropertyChangedFor(nameof(Total))]
	[NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
	private int hours;

	[ObservableProperty, NotifyPropertyChangedFor(nameof(Total))]
	[NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
	private int minutes;

	public string Total => $"{Hours} hour(s) {Minutes} minute(s)";

	public AddLogPageViewModel(INavigationService navService, PianoLessonsService service)
	{
		this.navService = navService;
		this.service = service;
		Date = DateTime.Today;
	}

	[RelayCommand(CanExecute = nameof(CanSubmit))]
	public async Task Submit()
	{
		if (Id != -1)
		{
			PracticeLog newLog = new()
			{
				Id = Id,
				LogDate = Date,
				Duration = new TimeSpan(Hours, Minutes, 0),
				Notes = "Need notes section",
				AssignmentId = 1,
				StudentId = 1
			};
			await service.UpdateLog(newLog);
		}
		else
		{
			var log = new PracticeLog
			{
				LogDate = Date,
				Duration = new TimeSpan(Hours, Minutes, 0),
				Notes = "Need notes section",
				AssignmentId = 1,
				StudentId = 1
			};
			await service.AddLog(log);
		}
		await navService.NavigateToAsync("..");
	}

	[RelayCommand]
	public async Task Loaded()
	{
		Date = DateTime.Today;
		Hours = 0;
		Minutes = 0;
		if (Id != -1)
		{
			var log = await service.GetLog(Id);
			Date = log.LogDate;
			Hours = log.Duration.Hours;
			Minutes = log.Duration.Minutes;
		}
	}

	private bool CanSubmit()
	{
		return Hours > 0 || Minutes > 0;
	}
}

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

	[ObservableProperty]
	private List<string> students;

	[ObservableProperty]
	private string selectedStudent;

	private readonly List<PracticeLog> tempLogs = new()
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
		new PracticeLog()
		{
			Name = "Bob",
			Date = DateTime.Now,
			Duration = TimeSpan.FromMinutes(90),
		},
	};

	[ObservableProperty]
	private bool isTeacher;

	[ObservableProperty, NotifyPropertyChangedFor(nameof(NoLogs))]
	private bool hasLogs;

	public bool NoLogs { get => !HasLogs; }

	public PracticeLogPageViewModel(INavigationService navService)
	{
		this.navService = navService;
	}

	[RelayCommand]
	public async Task ToAddLog()
	{
		await navService.NavigateToAsync($"{nameof(AddLogPage)}");
	}

	[RelayCommand]
	public async Task Loaded()
	{
		Students = new() {
			"All", "Bob", "Steve", "Bridger"
		}; ;
		if (Students.Count > 0)
		{
			SelectedStudent = Students[0];
		}
		Logs = new();
		FilterLogsCommand.Execute(this);
		IsTeacher = true;
	}

	[RelayCommand]
	public async Task FilterLogs()
	{
		Logs = new();
		//replace with request to db for selected students logs
		foreach (var log in tempLogs)
		{
			if (SelectedStudent == "All" || log.Name == SelectedStudent)
			{
				Logs.Add(log);
			}
		}
		HasLogs = Logs.Count > 0;
	}
}

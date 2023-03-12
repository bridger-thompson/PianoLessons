using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Pages;
using PianoLessons.Services;
using PianoLessons.Shared.Data;
using System.Collections.ObjectModel;

namespace PianoLessons.ViewModels;

public partial class PracticeLogPageViewModel : ObservableObject
{
	private readonly INavigationService navService;
	private readonly PianoLessonsService service;

	[ObservableProperty]
	private ObservableCollection<PracticeLog> logs;

	[ObservableProperty]
	private ObservableCollection<string> studentNames;

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

	public PracticeLogPageViewModel(INavigationService navService, PianoLessonsService service)
	{
		this.navService = navService;
		this.service = service;
	}

	[RelayCommand]
	public async Task ToAddLog()
	{
		await navService.NavigateToAsync($"{nameof(AddLogPage)}");
	}

	[RelayCommand]
	public async Task Loaded()
	{
		StudentNames = new()
		{
			"All"
		};
		var students = await service.GetStudentsForTeacher("1");
		foreach (var student in students)
		{
			StudentNames.Add(student.Name);
		}

		if (StudentNames.Count > 0)
		{
			SelectedStudent = StudentNames[0];
		}

		//authentication/db
		IsTeacher = true;
	}

	[RelayCommand]
	public async Task GetLogs()
	{
		Logs = new();
		var ls = await service.GetStudentLogs("1", SelectedStudent);
		foreach (var log in ls)
		{
			Logs.Add(log);
		}
		HasLogs = Logs.Count > 0;
	}
}

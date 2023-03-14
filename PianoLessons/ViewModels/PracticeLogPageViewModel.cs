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
	private string selectedStudentName;

	[ObservableProperty]
	private bool isTeacher;

	[ObservableProperty, NotifyPropertyChangedFor(nameof(NoLogs))]
	private bool hasLogs;

	private List<Student> students = new();

	public bool NoLogs { get => !HasLogs; }

	public PracticeLogPageViewModel(INavigationService navService, PianoLessonsService service)
	{
		this.navService = navService;
		this.service = service;
	}

	[RelayCommand]
	public async Task ToAddLog()
	{
		await navService.NavigateToAsync($"{nameof(AddLogPage)}?Id=-1");
	}

	[RelayCommand]
	public async Task Loaded()
	{
		StudentNames = new()
		{
			"All"
		};
		students = await service.GetStudentsForTeacher(1);
		foreach (var student in students)
		{
			StudentNames.Add(student.Name);
		}

		SelectedStudentName = StudentNames[0];

		//authentication/db
		IsTeacher = true;
	}

	[RelayCommand]
	public async Task GetLogs()
	{
		Logs = new();
		SelectedStudentName ??= "All";
		List<PracticeLog> ls = new();
		if (SelectedStudentName == "All")
		{
			ls = await service.GetAllStudentLogsForTeacher(1);
		}
		else
		{
			var selectedStudent = students.Where(s => s.Name == SelectedStudentName)
				.FirstOrDefault();
			ls = await service.GetLogsForStudent(selectedStudent.Id);
		}
		foreach (var log in ls)
		{
			Logs.Add(log);
		}
		HasLogs = Logs.Count > 0;
	}

	[RelayCommand]
	public async Task DeleteLog(int logId)
	{
		await service.DeleteLog(logId);
		GetLogsCommand.Execute(this);
	}

	[RelayCommand]
	public async Task EditLog(int logId)
	{
		await navService.NavigateToAsync($"{nameof(AddLogPage)}?Id={logId}");
	}
}

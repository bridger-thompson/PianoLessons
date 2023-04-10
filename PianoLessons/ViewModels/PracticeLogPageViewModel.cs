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
    private readonly AuthService auth;
    [ObservableProperty]
	private ObservableCollection<PracticeLog> logs;

	[ObservableProperty]
	private PracticeLog selectedLog;

	[ObservableProperty]
	private ObservableCollection<string> studentNames;

	[ObservableProperty]
	private string selectedStudentName;

	[ObservableProperty]
	private bool isTeacher;

	[ObservableProperty]
	private bool hasLogs;

	private List<Student> students = new();

	public PracticeLogPageViewModel(INavigationService navService, PianoLessonsService service, AuthService auth)
	{
		this.navService = navService;
		this.service = service;
        this.auth = auth;
    }

	[RelayCommand]
	public async Task ToAddLog()
	{
		await navService.NavigateToAsync($"{nameof(AddLogPage)}?Id=-1");
	}

	[RelayCommand]
	public async Task Loaded()
	{
		IsTeacher = auth.User.IsTeacher;
		if (IsTeacher)
		{
			students = new();
			students = await service.GetStudentsForTeacher(auth.User.Id);
		}
		StudentNames = new()
		{
			"All"
		};
		foreach (var student in students)
		{
			StudentNames.Add(student.Name);
		}

		SelectedStudentName = StudentNames[0];
	}

	[RelayCommand]
	public async Task GetLogs()
	{
		SelectedStudentName ??= "All";
		List<PracticeLog> ls = new();
		if (SelectedStudentName == "All" && IsTeacher)
		{
			ls = await service.GetAllStudentLogsForTeacher(auth.User.Id);
		}
		else if (!IsTeacher)
		{
			ls = await service.GetLogsForStudent(auth.User.Id);
		}
		else
		{
			var selectedStudent = students.Where(s => s.Name == SelectedStudentName)
				.FirstOrDefault();
			ls = await service.GetLogsForStudentAndTeacher(selectedStudent.Id, auth.User.Id);
		}
		Logs = new();
		foreach (var log in ls)
		{
			Logs.Add(log);
		}
		HasLogs = Logs.Count > 0;
	}

	[RelayCommand]
	public async Task EditLog()
	{
		if (!IsTeacher)
		{
			await navService.NavigateToAsync($"{nameof(AddLogPage)}?Id={SelectedLog.Id}");
		}
	}
}

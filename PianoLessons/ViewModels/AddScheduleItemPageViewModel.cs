using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Services;
using PianoLessons.Shared.Data;
using System.Collections.ObjectModel;

namespace PianoLessons.ViewModels;

public partial class AddScheduleItemPageViewModel : ObservableObject
{
	private readonly INavigationService navService;
	private readonly PianoLessonsService service;
    private readonly AuthService auth;
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
	private string title;

	[ObservableProperty]
	private DateTime start;

	[ObservableProperty]
	private DateTime end;

	[ObservableProperty]
	private TimeSpan startTime;

	[ObservableProperty]
	private TimeSpan endTime;

	[ObservableProperty]
	private ObservableCollection<Student> students;

	[ObservableProperty]
	private ObservableCollection<string> studentNames;

	[ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
	private string selectedStudentName;

	public AddScheduleItemPageViewModel(INavigationService navService, PianoLessonsService service, AuthService auth)
	{
		this.navService = navService;
		this.service = service;
        this.auth = auth;
        Title = string.Empty;
		SelectedStudentName = string.Empty;
	}

	[RelayCommand]
	public async Task Loaded()
	{
		Students = new();
		StudentNames = new();
		var s = await service.GetStudentsForTeacher(auth.User.Id);
		foreach (var student in s)
		{
			Students.Add(student);
			StudentNames.Add(student.Name);
		}

		if (StudentNames.Count > 0) { SelectedStudentName = StudentNames[0]; }
		else { SelectedStudentName = string.Empty; }

		Start = DateTime.Today;
		StartTime = Start.AddHours(9).TimeOfDay;
		End = DateTime.Today;
		EndTime = End.AddHours(10).TimeOfDay;
		Title = string.Empty;
	}

	[RelayCommand(CanExecute = nameof(CanAddItem))]
	public async Task AddItem()
	{
		var selectedStudent = Students.Where(s => s.Name == SelectedStudentName).FirstOrDefault();
		Appointment appointment = new()
		{
			Id = 0,
			Subject = Title,
			StartAt = new DateTime(Start.Year, Start.Month, Start.Day, StartTime.Hours, StartTime.Minutes, StartTime.Seconds),
			EndAt = new DateTime(End.Year, End.Month, End.Day, EndTime.Hours, EndTime.Minutes, EndTime.Seconds),
			TeacherId = auth.User.Id,
			StudentId = selectedStudent.Id,
		};
		var success = await service.AddAppointment(appointment);
		if (success)
		{
			await navService.NavigateToAsync("..");
		}
		else
		{
			await Application.Current.MainPage.DisplayAlert("Uh Oh!", $"Failed to add appointment", "OK");
		}
	}

	private bool CanAddItem()
	{
		return !string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(SelectedStudentName);
	}
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Services;
using PianoLessons.Shared.Data;
using System.Collections.ObjectModel;

namespace PianoLessons.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class AddLogPageViewModel : ObservableObject
{
	private readonly INavigationService navService;
	private readonly PianoLessonsService service;
	private readonly AuthService auth;
	[ObservableProperty]
	private int id;

	[ObservableProperty]
	private DateTime logDate;

	[ObservableProperty, NotifyPropertyChangedFor(nameof(Total))]
	private TimeSpan startTime;

	[ObservableProperty, NotifyPropertyChangedFor(nameof(Total))]
	private TimeSpan endTime;

	[ObservableProperty]
	private string notes;

	[ObservableProperty]
	private string pageTitle;

	[ObservableProperty]
	private ObservableCollection<Course> courses;

	[ObservableProperty]
	private ObservableCollection<string> courseNames;

	[ObservableProperty]
	private string selectedCourseName;

	[ObservableProperty]
	private bool timerStopped;

	[ObservableProperty]
	private bool isEditing;

    public string Total => $"{(EndTime - StartTime).Hours} hour(s) {(EndTime - StartTime).Minutes} minute(s)";

	public AddLogPageViewModel(INavigationService navService, PianoLessonsService service, AuthService auth)
	{
		this.navService = navService;
		this.service = service;
		this.auth = auth;
		TimerStopped = true;
		StartTime = DateTime.Now.TimeOfDay;
		EndTime = DateTime.Now.AddHours(1).TimeOfDay;
	}

	[RelayCommand]
	public async Task Submit()
	{
        var selectedCourse = Courses.Where(c => c.Name == SelectedCourseName)
            .FirstOrDefault();
		if (auth.User.IsTeacher)
		{
			await Application.Current.MainPage.DisplayAlert("How'd you get here?", $"Only students can add practice logs", "OK");
			await navService.NavigateToAsync("..");
		}
		PracticeLog log = new()
		{
			Id = Id,
			StartTime = new DateTime(LogDate.Year, LogDate.Month, LogDate.Day, StartTime.Hours, StartTime.Minutes, StartTime.Seconds),
			EndTime = new DateTime(LogDate.Year, LogDate.Month, LogDate.Day, EndTime.Hours, EndTime.Minutes, EndTime.Seconds),
			Notes = Notes,
			StudentId = auth.User.Id,
			CourseId = selectedCourse.Id,
		};
        if (IsEditing)
		{
			await service.UpdateLog(log);
		}
		else
		{
			log.Id = 0;
			await service.AddLog(log);
		}
		await navService.NavigateToAsync("..");
	}

	[RelayCommand]
	public async Task Loaded()
	{
		Courses = new();
		CourseNames = new();
		IsEditing = Id != -1;

		if (IsEditing)
		{
			var log = await service.GetLog(Id);
			LogDate = log.StartTime;
			StartTime = log.StartTime.TimeOfDay;
			EndTime = log.EndTime.TimeOfDay;
			Notes = log.Notes;
			PageTitle = "Edit Practice Log";
		}
		else
		{
            PageTitle = "New Practice Log";
			LogDate = DateTime.Today;
            Notes = null;
        }
		var c = await service.GetStudentCourses(auth.User.Id);
		foreach (var course in c)
		{
			Courses.Add(course);
			CourseNames.Add(course.Name);
		}
		if (CourseNames.Count > 0) { SelectedCourseName = CourseNames[0]; }
	}

	[RelayCommand]
	public void ToggleTimer()
	{
		if (TimerStopped)
		{
			StartTime = DateTime.Now.TimeOfDay;
			EndTime = DateTime.Now.TimeOfDay;
		}
		else EndTime = DateTime.Now.TimeOfDay;
		TimerStopped = !TimerStopped;
	}
}

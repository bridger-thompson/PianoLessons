using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Services;
using PianoLessons.Shared.Data;
using System.Collections.ObjectModel;

namespace PianoLessons.ViewModels;

public partial class ScoreboardPageViewModel : ObservableObject
{
	private readonly IPianoLessonsService service;
    private readonly IAuthService auth;
    [ObservableProperty]
	private ObservableCollection<StudentScore> studentScores;

	[ObservableProperty]
	private List<string> time;

	[ObservableProperty]
	private string selectedTime;

	private ObservableCollection<Course> courses;

	[ObservableProperty]
	private ObservableCollection<string> courseNames;

	[ObservableProperty]
	private string selectedCourseName;

	[ObservableProperty]
	private bool isLoading;

	private bool isTeacher;

	public ScoreboardPageViewModel(IPianoLessonsService service, IAuthService auth)
	{
		StudentScores = new();
		courses = new();
		SelectedCourseName = "No Courses";
		Time = new() { "Today", "Week", "Month", "Year", "Ever" };
		SelectedTime = Time[1];
		this.service = service;
        this.auth = auth;
    }

	[RelayCommand]
	public async Task GetCourses()
	{
		IsLoading = true;
		isTeacher = auth.User.IsTeacher;
		List<Course> c = new();
		if (isTeacher)
		{
			c = await service.GetTeacherCourses(auth.User.Id);
		}
		else
		{
			c = await service.GetStudentCourses(auth.User.Id);
		}
		courses = new();
		CourseNames = new();
		foreach (var course in c)
		{
			courses.Add(course);
			CourseNames.Add(course.Name);
		}
		if (CourseNames.Count > 0) { SelectedCourseName = CourseNames[0]; }
		IsLoading = false;
	}

	[RelayCommand]
	public async Task GetScores()
	{
		IsLoading = true;
		var selectedCourse = courses.Where(c => c.Name == SelectedCourseName)
			.FirstOrDefault();
		if (selectedCourse != null)
		{
			var scores = await service.GetScoresForCourseAndTime(selectedCourse.Id, SelectedTime);
			StudentScores = new();
			foreach (var score in scores)
			{
				StudentScores.Add(score);
			}
		}
		IsLoading = false;
	}
}

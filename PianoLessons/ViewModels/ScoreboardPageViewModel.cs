using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Services;
using PianoLessons.Shared.Data;
using System.Collections.ObjectModel;

namespace PianoLessons.ViewModels;

public partial class ScoreboardPageViewModel : ObservableObject
{
	private readonly PianoLessonsService service;

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

	public ScoreboardPageViewModel(PianoLessonsService service)
	{
		StudentScores = new();
		courses = new();
		SelectedCourseName = "No Courses";
		Time = new() { "Week", "Month", "Year", "Ever" };
		SelectedTime = Time[0];
		this.service = service;
	}

	[RelayCommand]
	public async Task GetTeacherCourses()
	{
		courses = new();
		CourseNames = new();
		var c = await service.GetTeacherCourses(1);
		foreach (var course in c)
		{
			courses.Add(course);
			CourseNames.Add(course.Name);
		}
		if (CourseNames.Count > 0) { SelectedCourseName = CourseNames[0]; }
	}

	[RelayCommand]
	public async Task GetScores()
	{
		StudentScores = new();
		
		var selectedCourse = courses.Where(c => c.Name == SelectedCourseName)
			.FirstOrDefault();

		if (selectedCourse != null)
		{
			var scores = await service.GetScoresForCourseAndTime(1, SelectedTime);
			foreach (var score in scores)
			{
				StudentScores.Add(score);
			}
		}

	}
}

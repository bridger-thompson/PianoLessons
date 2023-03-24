using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Pages;
using PianoLessons.Services;
using PianoLessons.Shared.Data;
using System.Collections.ObjectModel;

namespace PianoLessons.ViewModels;

public partial class ManageCoursesPageViewModel : ObservableObject
{
	private readonly PianoLessonsService service;
	private readonly INavigationService navService;
	[ObservableProperty]
	private ObservableCollection<Course> courses;

	[ObservableProperty]
	private string newCourseName;

	[ObservableProperty]
	private Course selectedCourse;

    public ManageCoursesPageViewModel(PianoLessonsService service, INavigationService navService)
	{
		this.service = service;
		this.navService = navService;
	}

	[RelayCommand]
	public async Task Loaded()
	{
		Courses = new();
		var c = await service.GetTeacherCourses(1);
		foreach (var course in c)
		{
			Courses.Add(course);
		}
		NewCourseName = "";
	}

	[RelayCommand]
	public async Task AddCourse()
	{
		Course newCourse = new()
		{
			Name = NewCourseName,
			TeacherId = 1,
		};
		await service.AddCourse(newCourse);
		LoadedCommand.Execute(this);
	}

	[RelayCommand]
	public async Task RemoveCourse(int courseId)
	{
		await service.DeleteCourse(courseId);
		LoadedCommand.Execute(this);
	}

	[RelayCommand]
	public async Task ToCourse()
	{
		await navService.NavigateToAsync($"{nameof(CourseDetailPage)}?Id={SelectedCourse.Id}");
	}
}

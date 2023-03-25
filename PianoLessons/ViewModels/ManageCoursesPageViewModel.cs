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

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(JoinCourseCommand))]
    private string newCode;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(NotTeacher))]
    private bool isTeacher;

    public bool NotTeacher { get => !IsTeacher; }

	[ObservableProperty]
	private bool noCourses;

    public ManageCoursesPageViewModel(PianoLessonsService service, INavigationService navService)
	{
		this.service = service;
		this.navService = navService;
		NewCode = string.Empty;
	}

	[RelayCommand]
	public async Task Loaded()
    {
        IsTeacher = await service.IsTeacher(10);
        Courses = new();
		List<Course> c = new();
		if (IsTeacher)
		{
			c = await service.GetTeacherCourses(1);
		}
		else
		{
			c = await service.GetStudentCourses(1);
		}
		foreach (var course in c)
		{
			Courses.Add(course);
		}
		NewCourseName = string.Empty;
		NewCode = string.Empty;
		NoCourses = Courses.Count == 0;
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

    [RelayCommand(CanExecute = nameof(CanJoin))]
    public async Task JoinCourse()
    {
        var success = await service.JoinCourse(1, NewCode.ToUpper());
        if (!success)
        {
            await Application.Current.MainPage.DisplayAlert("Invalid Code", $"Code was invalid: {NewCode}", "OK");
        }
        else
        {
			LoadedCommand.Execute(this);
        }
    }

	private bool CanJoin()
	{
		return NewCode.Length == 4;
	}
}

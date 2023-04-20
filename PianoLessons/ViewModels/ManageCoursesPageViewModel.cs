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
    private readonly AuthService auth;
    [ObservableProperty]
	private ObservableCollection<Course> courses;

	[ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddCourseCommand))]
	private string newCourseName;

	[ObservableProperty]
	private Course selectedCourse;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(JoinCourseCommand))]
    private string newCode;

    [ObservableProperty]
    private bool isTeacher;

	[ObservableProperty]
	private bool noCourses;

	[ObservableProperty]
	private bool isLoading;

    public ManageCoursesPageViewModel(PianoLessonsService service, INavigationService navService, AuthService auth)
	{
		this.service = service;
		this.navService = navService;
        this.auth = auth;
        NewCode = string.Empty;
	}

	[RelayCommand]
	public async Task Loaded()
    {
		IsLoading = true;
        IsTeacher = auth.User.IsTeacher;
		List<Course> c = new();
		if (IsTeacher)
		{
			c = await service.GetTeacherCourses(auth.User.Id);
		}
		else
		{
			c = await service.GetStudentCourses(auth.User.Id);
		}
        Courses = new();
		foreach (var course in c)
		{
			Courses.Add(course);
		}
		NewCourseName = string.Empty;
		NewCode = string.Empty;
		NoCourses = Courses.Count == 0 && !IsTeacher;
		IsLoading = false;
	}

	[RelayCommand(CanExecute = nameof(CanAdd))]
	public async Task AddCourse()
	{
		Course newCourse = new()
		{
			Name = NewCourseName,
			TeacherId = auth.User.Id,
			Teacher = new Teacher
			{
				Id = auth.User.Id,
				Name = auth.User.Name
			}
		};
		await service.AddCourse(newCourse);
		LoadedCommand.Execute(this);
	}

	private bool CanAdd()
	{
		return NewCourseName != string.Empty;
	}

	[RelayCommand]
	public async Task RemoveCourse(int courseId)
	{
		bool confirmDelete = await Application.Current.MainPage.DisplayAlert("Are you sure?", "Do you want to delete this recording?", "Yes", "No");
		if (confirmDelete)
		{
			await service.DeleteCourse(courseId);
			LoadedCommand.Execute(this);
		}
	}

	[RelayCommand]
	public async Task ToCourse()
	{
		await navService.NavigateToAsync($"{nameof(CourseDetailPage)}?Id={SelectedCourse.Id}");
	}

    [RelayCommand(CanExecute = nameof(CanJoin))]
    public async Task JoinCourse()
    {
        var success = await service.JoinCourse(auth.User.Id, NewCode.ToUpper());
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

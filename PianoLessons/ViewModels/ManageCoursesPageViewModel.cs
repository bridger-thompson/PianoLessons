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
        IsTeacher = auth.User.IsTeacher;
        Courses = new();
		List<Course> c = new();
		if (IsTeacher)
		{
			c = await service.GetTeacherCourses(auth.User.Id);
		}
		else
		{
			c = await service.GetStudentCourses(auth.User.Id);
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

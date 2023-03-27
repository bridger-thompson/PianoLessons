using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Services;
using PianoLessons.Shared.Data;
using System.Collections.ObjectModel;

namespace PianoLessons.ViewModels;

public partial class RecordingPageViewModel : ObservableObject
{
    private readonly PianoLessonsService service;

    [ObservableProperty]
    private ObservableCollection<Recording> recordings;

    [ObservableProperty]
    private ObservableCollection<Course> courses;

    [ObservableProperty]
    private ObservableCollection<string> courseNames;

    [ObservableProperty]
    private string selectedCourseName;

    [ObservableProperty, NotifyPropertyChangedFor(nameof(NotTeacher))]
    private bool isTeacher;

    public bool NotTeacher { get => !IsTeacher; }

    public RecordingPageViewModel(PianoLessonsService service)
    {
        this.service = service;
    }

    [RelayCommand]
    public async Task Loaded()
    {
        IsTeacher = await service.IsTeacher(10);
        await GetCourses(IsTeacher);
        await GetRecordings(IsTeacher);
    }

    private async Task GetCourses(bool isTeacher)
    {
        Courses = new();
        CourseNames = new();
        List<Course> c = new();
        if (IsTeacher) c = await service.GetTeacherCourses(1);
        else c = await service.GetStudentCourses(1);
        foreach (var course in c)
        {
            Courses.Add(course);
            CourseNames.Add(course.Name);
        }
        if (CourseNames.Count > 0) { SelectedCourseName = CourseNames[0]; }
    }

    private async Task GetRecordings(bool isTeacher)
    {
        Recordings = new();
        List<Recording> r = new();
        if (IsTeacher)
        {
            //get course recordings
        }
        else r = await service.GetStudentCourseRecordings(1, 1);
        foreach (var recording in r)
        {
            Recordings.Add(recording);
        }
    }

    [RelayCommand]
    public async Task DeleteRecording(int id)
    {
        //delete
    }
}

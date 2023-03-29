using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Interfaces;
using PianoLessons.Models;
using PianoLessons.Services;
using PianoLessons.Shared.Data;
using System.Collections.ObjectModel;

namespace PianoLessons.ViewModels;

public partial class RecordingPageViewModel : ObservableObject
{
    private readonly PianoLessonsService service;

    [ObservableProperty]
    private string recentAudioFilePath;
    [ObservableProperty]
    private bool isRecordingAudio;
    [ObservableProperty]
    private TimeSpan timerValue;
    [ObservableProperty]
    private string timerLabel;
    [ObservableProperty]
    private bool isRecord;
    [ObservableProperty]
    private Audio audioFile;

    [ObservableProperty]
    private bool isRecordButtonVisible;
    [ObservableProperty]
    private bool isPauseButtonVisible;
    [ObservableProperty]
    private bool isResumeButtonVisible;

    IRecordAudio recordAudioService;
    IAudioPlayer audioPlayerService;
    IDispatcherTimer recordTimer;
    IDispatcherTimer playTimer;

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

    public RecordingPageViewModel(PianoLessonsService service, IAudioPlayer audioPlayerService, IRecordAudio recordAudioService)
    {
        this.service = service;
        this.audioPlayerService = audioPlayerService;
        this.recordAudioService = recordAudioService;
        IsRecordButtonVisible = true;
        IsRecordingAudio = false;
        IsResumeButtonVisible = false;
    }

    [RelayCommand]
    public async Task Loaded()
    {
        IsTeacher = await service.IsTeacher("10");
        await GetCourses(IsTeacher);
        await GetRecordings(IsTeacher);
    }

    [RelayCommand]
    public async Task GetCourses(bool isTeacher)
    {
        Courses = new();
        CourseNames = new();
        List<Course> c = new();
        if (IsTeacher) c = await service.GetTeacherCourses("1");
        else c = await service.GetStudentCourses("1");
        foreach (var course in c)
        {
            Courses.Add(course);
            CourseNames.Add(course.Name);
        }
        if (CourseNames.Count > 0) { SelectedCourseName = CourseNames[0]; }
    }

    [RelayCommand]
    public async Task GetRecordings(bool isTeacher)
    {
        Recordings = new();
        List<Recording> r = new();
        if (IsTeacher)
        {
            //get course recordings
        }
        else r = await service.GetStudentCourseRecordings("1", 1);
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

    [RelayCommand]
    public void CreateTimer()
    {
        recordTimer = Application.Current.Dispatcher.CreateTimer();

        //timer start
        recordTimer.Interval = new TimeSpan(0, 0, 1);
        recordTimer.Tick += (s, e) =>
        {
            if (IsRecord)
            {
                TimerValue += new TimeSpan(0, 0, 1);
                TimerLabel = string.Format("{0:mm\\:ss}", TimerValue);
            }
        };
    }
    [RelayCommand]
    public async void StartRecording()
    {
        if (!IsRecordingAudio)
        {
            var permissionStatus = await RequestandCheckPermission();
            if (permissionStatus == PermissionStatus.Granted)
            {
                IsRecordingAudio = true;
                IsPauseButtonVisible = true;
                recordAudioService.StartRecord();
                IsRecordButtonVisible = false;
                IsRecord = true;
                TimerValue = new TimeSpan(0, 0, -1);
                if (recordTimer == null)
                    CreateTimer();
                recordTimer.Start();
            }
            else
            {
                IsRecordingAudio = false;
                IsPauseButtonVisible = false;
            }
        }
        else
        {
            ResumeRecording();
        }
    }

    [RelayCommand]
    public void PauseRecording()
    {
        IsRecord = false;
        IsPauseButtonVisible = false;
        IsResumeButtonVisible = true;
        recordAudioService.PauseRecord();
    }

    [RelayCommand]
    public void ResumeRecording()
    {
        recordAudioService.StartRecord();
        IsResumeButtonVisible = false;
        IsPauseButtonVisible = true;
        IsRecord = true;
    }

    [RelayCommand]
    public void ResetRecording()
    {
        recordAudioService.ResetRecord();
        TimerValue = new TimeSpan();
        TimerLabel = string.Format("{0:mm\\:ss}", TimerValue);
        IsRecordingAudio = false;
        IsPauseButtonVisible = false;
        IsResumeButtonVisible = false;
        StartRecording();
    }

    [RelayCommand]
    public async void StopRecording()
    {
        IsPauseButtonVisible = false;
        IsResumeButtonVisible = false;
        IsRecordingAudio = false;
        IsRecordButtonVisible = true;
        TimerValue = new TimeSpan();
        recordTimer.Stop();
        RecentAudioFilePath = recordAudioService.StopRecord();
        await App.Current.MainPage.DisplayAlert("Alert", "Audio has been recorded", "OK");
        TimerLabel = string.Format("{0:mm\\:ss}", TimerValue);
        SendRecording();
    }

    [RelayCommand]
    public void SendRecording()
    {
        //Audio recordedFile = new Audio() { AudioURL = RecentAudioFilePath };
        //if (recordedFile != null)
        //{
        //    recordedFile.AudioName = Path.GetFileName(RecentAudioFilePath);
        //    Audios.Insert(0, recordedFile);
        //}
    }

    [RelayCommand]
    public async void DeleteAudio(Audio obj)
    {
        //var alert = await App.Current.MainPage.DisplayAlert("Alert", "Are you sure you want to delete the audio?", "Yes", "No");
        //if (alert)
        //    Audios.Remove(obj);
    }

    public async Task<PermissionStatus> RequestandCheckPermission()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
        if (status != PermissionStatus.Granted)
            await Permissions.RequestAsync<Permissions.StorageWrite>();

        status = await Permissions.CheckStatusAsync<Permissions.Microphone>();
        if (status != PermissionStatus.Granted)
            await Permissions.RequestAsync<Permissions.Microphone>();

        PermissionStatus storagePermission = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
        PermissionStatus microPhonePermission = await Permissions.CheckStatusAsync<Permissions.Microphone>();
        if (storagePermission == PermissionStatus.Granted && microPhonePermission == PermissionStatus.Granted)
        {
            return PermissionStatus.Granted;
        }
        return PermissionStatus.Denied;
    }
}

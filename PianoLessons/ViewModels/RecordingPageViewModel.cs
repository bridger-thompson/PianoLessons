using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
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

	private string recentAudioFilePath;
	private TimeSpan timerValue;
	private string timerLabel;
	private bool isRecord;
	private Audio audioFile;

	[ObservableProperty]
	private bool isRecordingAudio;
	[ObservableProperty]
	private bool isRecordButtonVisible;
	[ObservableProperty]
	private bool isPauseButtonVisible;
	[ObservableProperty]
	private bool isResumeButtonVisible;

	IRecordAudio recordAudioService;
	private readonly AuthService auth;
	IDispatcherTimer recordTimer;

	[ObservableProperty]
	private ObservableCollection<Recording> recordings;

	[ObservableProperty]
	private ObservableCollection<Course> courses;

	[ObservableProperty]
	private ObservableCollection<string> courseNames;

	[ObservableProperty]
	private string selectedCourseName;

	[ObservableProperty]
	private bool isTeacher;

	[ObservableProperty]
	private ObservableCollection<Student> students;

	[ObservableProperty]
	private ObservableCollection<string> studentNames;

	[ObservableProperty]
	private string selectedStudentName;

	[ObservableProperty]
	private bool hasRecordings;

	private Course selectedCourse;


	public RecordingPageViewModel(PianoLessonsService service, IRecordAudio recordAudioService, AuthService auth)
	{
		this.service = service;
		this.recordAudioService = recordAudioService;
		this.auth = auth;
		IsRecordingAudio = false;
		IsResumeButtonVisible = false;
		Recordings = new();
	}

	[RelayCommand]
	public async Task Loaded()
	{
		IsRecordButtonVisible = auth.User.IsStudent;
		IsTeacher = auth.User.IsTeacher;
		await GetCoursesCommand.ExecuteAsync(this);
	}

	[RelayCommand]
	public async Task GetCourses()
	{
		Courses = new();
		CourseNames = new();
		List<Course> c = new();
		if (IsTeacher) c = await service.GetTeacherCourses(auth.User.Id);
		else c = await service.GetStudentCourses(auth.User.Id);
		foreach (var course in c)
		{
			Courses.Add(course);
			CourseNames.Add(course.Name);
		}
		if (CourseNames.Count > 0) { SelectedCourseName = CourseNames[0]; }
		else { SelectedCourseName = ""; }
		await GetStudentsCommand.ExecuteAsync(this);
	}

	public async Task GetRecordings()
	{
		var studentId = auth.User.Id;
		if (IsTeacher)
		{
			var selectedStudent = Students
				.Where(s => s.Name == SelectedStudentName)
				.FirstOrDefault();
			if (selectedStudent != null)
			{
				studentId = selectedStudent.Id;
			}
			else
			{
				studentId = "-1";
			}
		}
		List<Recording> r = await service.GetStudentCourseRecordings(studentId, selectedCourse.Id);
		Recordings = new();
		foreach (var recording in r)
		{
			Recordings.Add(recording);
		}
		HasRecordings = Recordings.Count > 0;
	}

	[RelayCommand]
	public async Task GetStudents()
	{
		selectedCourse = Courses
			.Where(c => c.Name == SelectedCourseName)
			.FirstOrDefault();
		if (selectedCourse != null)
		{
			var s = await service.GetCourseStudents(selectedCourse.Id);
			Students = new();
			StudentNames = new();
			foreach (var student in s)
			{
				Students.Add(student);
				StudentNames.Add(student.Name);
			}
			if (StudentNames.Count > 0) SelectedStudentName = StudentNames[0];
			await GetRecordings();
		}
	}

	[RelayCommand]
	public async Task DeleteRecording(int id)
	{
		if (auth.User.IsStudent)
		{
			var recording = Recordings.Where(r => r.Id == id).FirstOrDefault();
			var fileName = recording.FilePath.Split('/')[4];
			await service.DeleteRecording(auth.User.Id, id, fileName);
			await GetRecordings();
		}
	}

	[RelayCommand]
	public void CreateTimer()
	{
		recordTimer = Application.Current.Dispatcher.CreateTimer();

		//timer start
		recordTimer.Interval = new TimeSpan(0, 0, 1);
		recordTimer.Tick += (s, e) =>
		{
			if (isRecord)
			{
				timerValue += new TimeSpan(0, 0, 1);
				timerLabel = string.Format("{0:mm\\:ss}", timerValue);
			}
		};
	}
	[RelayCommand]
	public async void StartRecording()
	{
		if (!IsRecordingAudio)
		{
			var permissionStatus = await RecordingPageViewModel.RequestandCheckPermission();
			if (permissionStatus == PermissionStatus.Granted)
			{
				IsRecordingAudio = true;
				IsPauseButtonVisible = true;
				recordAudioService.StartRecord();
				IsRecordButtonVisible = false;
				isRecord = true;
				timerValue = new TimeSpan(0, 0, -1);
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
		isRecord = false;
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
		isRecord = true;
	}

	[RelayCommand]
	public void ResetRecording()
	{
		recordAudioService.ResetRecord();
		timerValue = new TimeSpan();
		timerLabel = string.Format("{0:mm\\:ss}", timerValue);
		IsRecordingAudio = false;
		IsPauseButtonVisible = false;
		IsResumeButtonVisible = false;
		StartRecording();
	}

	[RelayCommand]
	public void StopRecording()
	{
		IsPauseButtonVisible = false;
		IsResumeButtonVisible = false;
		IsRecordingAudio = false;
		IsRecordButtonVisible = true;
		timerValue = new TimeSpan();
		recordTimer.Stop();
		recentAudioFilePath = recordAudioService.StopRecord();
		timerLabel = string.Format("{0:mm\\:ss}", timerValue);
		SendRecording();
	}

	public async void SendRecording()
	{
		Audio recordedFile = new() { AudioURL = recentAudioFilePath };

		if (recordedFile != null)
		{
			recordedFile.AudioName = Path.GetFileName(recentAudioFilePath);
			var fileBytes = File.ReadAllBytes(recordedFile.AudioURL);
			var fileData = new FileData { FileName = recordedFile.AudioName, Data = fileBytes };
			var selectedCourse = Courses.Where(c => c.Name == SelectedCourseName).FirstOrDefault();
			if (selectedCourse != null)
			{
				await service.AddRecording(fileData, auth.User.Id, selectedCourse.Id);
				await GetRecordings();
			}
		}
	}

	public static async Task<PermissionStatus> RequestandCheckPermission()
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

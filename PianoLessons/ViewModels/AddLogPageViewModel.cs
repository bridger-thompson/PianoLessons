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
	private ObservableCollection<PracticeAssignment> assignments;

	[ObservableProperty]
	private ObservableCollection<string> assignmentNames;

	[ObservableProperty]
	private string selectedAssignmentName;

    public string Total => $"{(EndTime - StartTime).Hours} hour(s) {(EndTime - StartTime).Minutes} minute(s)";

	public AddLogPageViewModel(INavigationService navService, PianoLessonsService service)
	{
		this.navService = navService;
		this.service = service;
    }

	[RelayCommand]
	public async Task Submit()
	{
        var selectedAssignment = Assignments.Where(c => c.Name == SelectedAssignmentName)
            .FirstOrDefault();
		PracticeLog log = new()
		{
			Id = Id,
			StartTime = new DateTime(LogDate.Year, LogDate.Month, LogDate.Day, StartTime.Hours, StartTime.Minutes, StartTime.Seconds),
			EndTime = new DateTime(LogDate.Year, LogDate.Month, LogDate.Day, EndTime.Hours, EndTime.Minutes, EndTime.Seconds),
			Notes = Notes,
			AssignmentId = selectedAssignment.Id,
			StudentId = "1"
		};
        if (Id != -1)
		{
			await service.UpdateLog(log);
		}
		else
		{
			await service.AddLog(log);
		}
		await navService.NavigateToAsync("..");
	}

	[RelayCommand]
	public async Task Loaded()
	{
		Assignments = new();
		AssignmentNames = new();

		if (Id != -1)
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
			StartTime = DateTime.Now.TimeOfDay;
			EndTime = DateTime.Now.AddHours(1).TimeOfDay;
            Notes = string.Empty;
        }
		var studentAssignments = await service.GetStudentAssignments(1);
		foreach (var assignment in studentAssignments)
		{
			Assignments.Add(assignment);
			AssignmentNames.Add(assignment.Name);
		}
		if (AssignmentNames.Count > 0) { SelectedAssignmentName = AssignmentNames[0]; }
	}
}

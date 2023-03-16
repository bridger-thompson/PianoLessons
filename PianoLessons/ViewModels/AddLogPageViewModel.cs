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
	private DateTime date;

	[ObservableProperty, NotifyPropertyChangedFor(nameof(Total))]
	[NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
	private int hours;

	[ObservableProperty, NotifyPropertyChangedFor(nameof(Total))]
	[NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
	private int minutes;

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

    public string Total => $"{Hours} hour(s) {Minutes} minute(s)";

	public AddLogPageViewModel(INavigationService navService, PianoLessonsService service)
	{
		this.navService = navService;
		this.service = service;
		Date = DateTime.Today;
        Assignments = new();
        AssignmentNames = new();
    }

	[RelayCommand(CanExecute = nameof(CanSubmit))]
	public async Task Submit()
	{
        var selectedAssignment = Assignments.Where(c => c.Name == SelectedAssignmentName)
            .FirstOrDefault();
        if (Id != -1)
		{
			PracticeLog newLog = new()
			{
				Id = Id,
				Duration = new TimeSpan(Hours, Minutes, 0),
				Notes = Notes,
				AssignmentId = selectedAssignment.Id,
				StudentId = 1
			};
			await service.UpdateLog(newLog);
		}
		else
		{
			var log = new PracticeLog
			{
				Duration = new TimeSpan(Hours, Minutes, 0),
				Notes = Notes,
				AssignmentId = selectedAssignment.Id,
				StudentId = 1
			};
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
			Hours = log.Duration.Hours;
			Minutes = log.Duration.Minutes;
			Notes = log.Notes;
			PageTitle = "Edit Practice Log";
		}
		else
		{
            PageTitle = "New Practice Log";
            Date = DateTime.Today;
            Hours = 0;
            Minutes = 0;
            Notes = "";
        }
		var studentAssignments = await service.GetStudentAssignments(1);
		foreach (var assignment in studentAssignments)
		{
			Assignments.Add(assignment);
			AssignmentNames.Add(assignment.Name);
		}
		if (AssignmentNames.Count > 0) { SelectedAssignmentName = AssignmentNames[0]; }
	}

	private bool CanSubmit()
	{
		return (Hours > 0 || Minutes > 0) && Hours < 24;
	}
}

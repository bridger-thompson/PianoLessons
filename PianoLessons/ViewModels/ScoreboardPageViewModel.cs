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
	private ObservableCollection<Student> students;

	[ObservableProperty]
	private List<string> time;

	[ObservableProperty]
	private string selectedTime;

	public ScoreboardPageViewModel(PianoLessonsService service)
	{
		Students = new();
		Time = new() { "Week", "Month", "Year", "Ever" };
		SelectedTime = Time[0];
		this.service = service;
	}

	[RelayCommand]
	public async Task GetScores()
	{
		Students = new();
		var studs = await service.GetStudentsScoresForTeacher("1", SelectedTime);
		foreach (var student in studs)
		{
			Students.Add(student);
		}
	}
}

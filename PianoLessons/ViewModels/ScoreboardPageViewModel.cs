using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Models;
using System.Collections.ObjectModel;

namespace PianoLessons.ViewModels;

public partial class ScoreboardPageViewModel : ObservableObject
{
	[ObservableProperty]
	private ObservableCollection<Student> students;

	[ObservableProperty]
	private List<string> time;

	[ObservableProperty]
	private string selectedTime;

	public ScoreboardPageViewModel()
	{
		Students = new();
		Time = new() { "Week", "Month", "Year", "Ever" };
		SelectedTime = Time[0];
	}

	[RelayCommand]
	public async Task GetScores()
	{
		//get from db
		Students = new()
		{
			new()
			{
				Name = "Bob",
				Id = 1,
				Score = 500
			},
			new()
			{
				Name = "John",
				Id = 2,
			},
			new()
			{
				Name = "Some weirdo with a super long name",
				Id = 3,
				Score = 100000
			}
		};
	}
}

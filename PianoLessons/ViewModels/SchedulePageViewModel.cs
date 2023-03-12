using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Pages;
using PianoLessons.Services;
using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;

namespace PianoLessons.ViewModels;

public partial class SchedulePageViewModel : ObservableObject
{
	private readonly INavigationService navService;

	[ObservableProperty]
	private ObservableCollection<SchedulerAppointment> events;

	public SchedulePageViewModel(INavigationService navService)
	{
		this.navService = navService;
	}

	[RelayCommand]
	public async Task ToAddScheduleItem()
	{
		//nav
		await navService.NavigateToAsync($"{nameof(AddScheduleItemPage)}");
	}

	[RelayCommand]
	public async Task Loaded()
	{
		DateTime today = DateTime.Today;
		DateTime yesterday = new(today.Year, today.Month, today.Day - 1);
		//get all from db
		var appointment = new ObservableCollection<SchedulerAppointment>
		{
			new SchedulerAppointment()
			{
				StartTime = today.AddHours(9),
				EndTime = today.AddHours(11),
				Subject = "Client Meeting",
			},
			new SchedulerAppointment()
			{
				StartTime = yesterday.AddHours(9),
				EndTime = today.AddHours(10),
				Subject = "Long Meeting",
			}
		};
		Events = appointment;

	}
}

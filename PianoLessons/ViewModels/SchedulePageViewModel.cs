using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Pages;
using PianoLessons.Services;
using Syncfusion.Maui.Scheduler;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		//get all from db
		var appointment = new ObservableCollection<SchedulerAppointment>
		{
			new SchedulerAppointment()
			{
				StartTime = DateTime.Today.AddHours(9),
				EndTime = DateTime.Today.AddHours(11),
				Subject = "Client Meeting",
			}
		};
		Events = appointment;

	}
}

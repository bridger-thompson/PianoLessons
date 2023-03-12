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
	private readonly PianoLessonsService service;
	[ObservableProperty]
	private ObservableCollection<SchedulerAppointment> events;

	public SchedulePageViewModel(INavigationService navService, PianoLessonsService service)
	{
		this.navService = navService;
		this.service = service;
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
		Events = new();
		var appointments = await service.GetAppointmentsForUser("1");
		foreach (var appointment in appointments)
		{
			Events.Add(new SchedulerAppointment()
			{
				StartTime = appointment.StartTime,
				EndTime = appointment.EndTime,
				Subject = appointment.Subject,
			});
		}
	}
}

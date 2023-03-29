using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Pages;
using PianoLessons.Services;
using PianoLessons.Shared.Data;
using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;

namespace PianoLessons.ViewModels;

public partial class SchedulePageViewModel : ObservableObject
{
	private readonly INavigationService navService;
	private readonly PianoLessonsService service;
    private readonly AuthService auth;

    [ObservableProperty]
	private ObservableCollection<SchedulerAppointment> events;

	[ObservableProperty]
	private bool isTeacher;

	public SchedulePageViewModel(INavigationService navService, PianoLessonsService service, AuthService auth)
	{
		this.navService = navService;
		this.service = service;
        this.auth = auth;
    }

	[RelayCommand]
	public async Task ToAddScheduleItem()
	{
		await navService.NavigateToAsync($"{nameof(AddScheduleItemPage)}");
	}

	[RelayCommand]
	public async Task Loaded()
	{
		IsTeacher = auth.User.IsTeacher;
		Events = new();
		List<Appointment> appointments = new();
		if (IsTeacher)
		{
			//user id (teacher)
			appointments = await service.GetAppointmentsForTeacher(auth.User.Id);
		}
		else
		{
			//user id (student)
			appointments = await service.GetAppointmentsForStudent(auth.User.Id);
		}
		foreach (var appointment in appointments)
		{
			Events.Add(new SchedulerAppointment()
			{
				StartTime = appointment.StartAt,
				EndTime = appointment.EndAt,
				Subject = appointment.Subject,
			});
		}
	}
}

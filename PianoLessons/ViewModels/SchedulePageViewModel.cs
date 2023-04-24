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
	private readonly IPianoLessonsService service;
    private readonly IAuthService auth;

    [ObservableProperty]
	private ObservableCollection<SchedulerAppointment> events;

	[ObservableProperty]
	private bool isTeacher;

	[ObservableProperty]
	private bool isLoading;

	public SchedulePageViewModel(INavigationService navService, IPianoLessonsService service, IAuthService auth)
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
		IsLoading = true;
		IsTeacher = auth.User.IsTeacher;
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
		Events = new();
		foreach (var appointment in appointments)
		{
			Events.Add(new SchedulerAppointment()
			{
				StartTime = appointment.StartAt,
				EndTime = appointment.EndAt,
				Subject = appointment.Subject,
			});
		}
		IsLoading = false;
	}

	[RelayCommand]
	public async Task Logout()
	{
		await auth.Logout();
		await navService.NavigateToAsync($"///{nameof(LoginPage)}");
	}
}

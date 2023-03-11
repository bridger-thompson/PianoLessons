using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Pages;
using PianoLessons.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoLessons.ViewModels;

public partial class SchedulePageViewModel : ObservableObject
{
	private readonly INavigationService navService;

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
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoLessons.ViewModels;

public partial class AddScheduleItemPageViewModel : ObservableObject
{
	private readonly INavigationService navService;
	[ObservableProperty, NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
	private string title;

	[ObservableProperty]
	private DateTime start;

	[ObservableProperty]
	private DateTime end;

	public AddScheduleItemPageViewModel(INavigationService navService)
	{
		Start = DateTime.Today;
		Start = Start.AddHours(9);
		End = DateTime.Today;
		End = End.AddHours(10);
		this.navService = navService;
	}

	[RelayCommand(CanExecute = nameof(CanAddItem))]
	public async Task AddItem()
	{
		//add to db
		await navService.NavigateToAsync("..");
	}

	private bool CanAddItem()
	{
		return Title != null && Title.Length > 0;
	}
}

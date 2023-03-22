using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Services;
using PianoLessons.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoLessons.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class CourseDetailPageViewModel : ObservableObject
{
	private readonly PianoLessonsService service;

	[ObservableProperty]
	private int id;

	[ObservableProperty]
	private Course currentCourse;

	public CourseDetailPageViewModel(PianoLessonsService service)
	{
		this.service = service;
	}

	[RelayCommand]
	public async Task Loaded()
	{
        CurrentCourse = await service.GetCourse(Id);
	}
}

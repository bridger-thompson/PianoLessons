using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Services;
using PianoLessons.Shared.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

	[ObservableProperty]
	private string newName;

	[ObservableProperty]
	private string email;

	[ObservableProperty]
	private ObservableCollection<Student> students;

	[ObservableProperty, NotifyPropertyChangedFor(nameof(NotEditing))]
	private bool isEditing;

	public bool NotEditing { get => !IsEditing; }

	public CourseDetailPageViewModel(PianoLessonsService service)
	{
		this.service = service;
	}

	[RelayCommand]
	public async Task Loaded()
	{
		Students = new();
        CurrentCourse = await service.GetCourse(Id);
		var s = await service.GetCourseStudents(Id);
		foreach (var student in s)
		{
			Students.Add(student);
		}
		NewName = CurrentCourse.Name;
	}

	[RelayCommand]
	public void StartEdit()
	{
		IsEditing = true;
	}

	[RelayCommand]
	public async Task EditName()
	{
		await service.UpdateCourse(Id, NewName);
        CurrentCourse = await service.GetCourse(Id);
        IsEditing = false;
	}

	[RelayCommand]
	public async Task Invite()
	{

	}
}

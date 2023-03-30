﻿using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Services;
using PianoLessons.Shared.Data;
using System.Collections.ObjectModel;

namespace PianoLessons.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class CourseDetailPageViewModel : ObservableObject
{
	private readonly PianoLessonsService service;
	private readonly AuthService auth;
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

	[ObservableProperty]
	private bool isEditing;

    [ObservableProperty]
    private bool isTeacher;

    public CourseDetailPageViewModel(PianoLessonsService service, AuthService auth)
	{
		this.service = service;
		this.auth = auth;
	}

	[RelayCommand]
	public async Task Loaded()
	{
		IsTeacher = auth.User.IsTeacher;
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
		var code = await service.GenerateCourseInvite(Id);
		await Application.Current.MainPage.DisplayAlert("Generated Code!", $"Code: {code}", "OK");
    }

	[RelayCommand]
	public async Task RemoveStudent(string studentId)
	{
		await service.RemoveStudent(Id, studentId);
		LoadedCommand.Execute(this);
	}
}

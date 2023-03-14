﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PianoLessons.Pages;
using PianoLessons.Services;
using PianoLessons.Shared.Data;
using System.Collections.ObjectModel;

namespace PianoLessons.ViewModels;

public partial class PracticeLogPageViewModel : ObservableObject
{
	private readonly INavigationService navService;
	private readonly PianoLessonsService service;

	[ObservableProperty]
	private ObservableCollection<PracticeLog> logs;

	[ObservableProperty]
	private ObservableCollection<string> studentNames;

	[ObservableProperty]
	private string selectedStudent;

	[ObservableProperty]
	private bool isTeacher;

	[ObservableProperty, NotifyPropertyChangedFor(nameof(NoLogs))]
	private bool hasLogs;

	public bool NoLogs { get => !HasLogs; }

	public PracticeLogPageViewModel(INavigationService navService, PianoLessonsService service)
	{
		this.navService = navService;
		this.service = service;
	}

	[RelayCommand]
	public async Task ToAddLog()
	{
		await navService.NavigateToAsync($"{nameof(AddLogPage)}");
	}

	[RelayCommand]
	public async Task Loaded()
	{
		StudentNames = new()
		{
			"All"
		};
		var students = await service.GetStudentsForTeacher(1);
		foreach (var student in students)
		{
			StudentNames.Add(student.Name);
		}

		SelectedStudent = StudentNames[0];

		//authentication/db
		IsTeacher = true;
	}

	[RelayCommand]
	public async Task GetLogs()
	{
		Logs = new();
		if (SelectedStudent == null) { SelectedStudent = "All"; }
		var ls = await service.GetAllStudentLogsForTeacher(1);
		foreach (var log in ls)
		{
			Logs.Add(log);
		}
		HasLogs = Logs.Count > 0;
	}
}

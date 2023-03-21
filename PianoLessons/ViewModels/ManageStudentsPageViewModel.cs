﻿using CommunityToolkit.Mvvm.ComponentModel;
using PianoLessons.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoLessons.ViewModels;

public partial class ManageStudentsPageViewModel : ObservableObject
{
	private readonly PianoLessonsService service;

	public ManageStudentsPageViewModel(PianoLessonsService service)
	{
		this.service = service;
	}
}

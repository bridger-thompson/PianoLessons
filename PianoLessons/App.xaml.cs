﻿using PianoLessons.Pages;

namespace PianoLessons;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}

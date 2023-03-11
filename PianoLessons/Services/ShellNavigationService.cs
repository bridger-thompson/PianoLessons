using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoLessons.Services;

public interface INavigationService
{
	Task NavigateToAsync(string destination);
}

public class ShellNavigationService : INavigationService
{
	public async Task NavigateToAsync(string destination)
	{
		await Shell.Current.GoToAsync(destination);
	}
}

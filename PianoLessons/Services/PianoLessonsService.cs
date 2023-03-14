using PianoLessons.Shared.Data;
using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace PianoLessons.Services;

public class PianoLessonsService
{
	private readonly HttpClient client;

	public PianoLessonsService(HttpClient client)
	{
		this.client = client;
	}

	public async Task<List<Appointment>> GetAppointmentsForTeacher(int teacherId)
	{
		var appointments = await client.GetFromJsonAsync<List<Appointment>>($"api/PianoLessons/appointments/{teacherId}");
		return appointments;
	}

	public async Task<List<Student>> GetStudentsScoresForTeacher(int teacherId, string time)
	{
		return await client.GetFromJsonAsync<List<Student>>($"api/PianoLessons/students/{teacherId}/{time}");
	}

	public async Task<List<Student>> GetStudentsForTeacher(int teacherId)
	{
		return await client.GetFromJsonAsync<List<Student>>($"api/PianoLessons/students/{teacherId}");
	}

	public async Task<List<PracticeLog>> GetAllStudentLogsForTeacher(int teacherId)
	{
		return await client.GetFromJsonAsync<List<PracticeLog>>($"api/PianoLessons/logs/{teacherId}");
	}
}

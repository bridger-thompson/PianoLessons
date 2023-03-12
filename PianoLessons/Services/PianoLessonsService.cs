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

	public async Task<List<Appointment>> GetAppointmentsForUser(string userId)
	{
		return await client.GetFromJsonAsync<List<Appointment>>($"api/PianoLessons/appointments/{userId}");
	}

	public async Task<List<Student>> GetStudentsScoresForTeacher(string teacherId, string time)
	{
		return await client.GetFromJsonAsync<List<Student>>($"api/PianoLessons/students/{teacherId}/{time}");
	}

	public async Task<List<Student>> GetStudentsForTeacher(string teacherId)
	{
		return await client.GetFromJsonAsync<List<Student>>($"api/PianoLessons/students/{teacherId}");
	}

	public async Task<List<PracticeLog>> GetStudentLogs(string teacherId, string student)
	{
		return await client.GetFromJsonAsync<List<PracticeLog>>($"api/PianoLessons/logs/{teacherId}/{student}");
	}
}

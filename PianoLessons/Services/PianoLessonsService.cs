using PianoLessons.Shared.Data;
using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

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
		return await client.GetFromJsonAsync<List<PracticeLog>>($"api/PianoLessons/logs/all/{teacherId}");
	}

	public async Task<List<PracticeLog>> GetLogsForStudent(int studentId)
	{
		return await client.GetFromJsonAsync<List<PracticeLog>>($"api/PianoLessons/logs/{studentId}");
	}

	public async Task DeleteLog(int logId)
	{
		await client.DeleteAsync($"api/PianoLessons/logs/{logId}");
	}

	public async Task UpdateLog(PracticeLog newLog)
	{
		var log = JsonSerializer.Serialize(newLog);
		var requestContent = new StringContent(log, Encoding.UTF8, "application/json");
		await client.PutAsync($"api/PianoLessons/logs", requestContent);
	}
}

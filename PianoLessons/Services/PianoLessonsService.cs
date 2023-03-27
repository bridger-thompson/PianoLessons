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
		var appointments = await client.GetFromJsonAsync<List<Appointment>>($"api/PianoLessons/appointments/teacher/{teacherId}");
		return appointments;
	}

	public async Task<bool> AddAppointment(Appointment appointment)
	{
		var result = await client.PostAsJsonAsync($"api/PianoLessons/appointments", appointment);
		if (result.IsSuccessStatusCode) return true;
		return false;
	}

	public async Task<List<Appointment>> GetAppointmentsForStudent(int studentId)
	{
		var appointments = await client.GetFromJsonAsync<List<Appointment>>($"api/PianoLessons/appointments/student/{studentId}");
		return appointments;
	}

	public async Task<List<StudentScore>> GetScoresForCourseAndTime(int courseId, string time)
	{
		return await client.GetFromJsonAsync<List<StudentScore>>($"api/PianoLessons/scores/{courseId}/{time}");
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

	public async Task<List<Course>> GetTeacherCourses(int teacherId)
	{
		return await client.GetFromJsonAsync<List<Course>>($"api/PianoLessons/courses/teacher/{teacherId}");
	}

	public async Task<List<Course>> GetStudentCourses(int studentId)
	{
		return await client.GetFromJsonAsync<List<Course>>($"api/PianoLessons/courses/student/{studentId}");
	}

	public async Task AddLog(PracticeLog log)
	{
		await client.PostAsJsonAsync("api/PianoLessons/logs", log);
	}

	public async Task<PracticeLog> GetLog(int logId)
	{
		return await client.GetFromJsonAsync<PracticeLog>($"api/PianoLessons/logs/log/{logId}");
	}

	public async Task<List<PracticeAssignment>> GetStudentAssignments(int studentId)
	{
		return await client.GetFromJsonAsync<List<PracticeAssignment>>($"api/PianoLessons/assignments/{studentId}");
	}

	public async Task<bool> IsTeacher(int teacherId)
	{
		return await client.GetFromJsonAsync<bool>($"api/PianoLessons/isTeacher/{teacherId}");
	}

	public async Task AddCourse(Course course)
	{
		await client.PostAsJsonAsync("api/PianoLessons/course", course);
	}

	public async Task DeleteCourse(int courseId)
	{
		await client.DeleteAsync($"api/PianoLessons/course/{courseId}");
	}

	public async Task UpdateCourse(int courseId, string newName)
	{
		await client.PutAsync($"api/PianoLessons/course/{courseId}/{newName}", null);
	}

    public async Task<Course> GetCourse(int id)
	{
		return await client.GetFromJsonAsync<Course>($"api/PianoLessons/course/{id}");
	}

	public async Task<List<Student>> GetCourseStudents(int id)
	{
		return await client.GetFromJsonAsync<List<Student>>($"api/PianoLessons/course/{id}/students");
	}

	public async Task<string> GenerateCourseInvite(int courseId)
	{
		return await client.GetStringAsync($"api/PianoLessons/invite/generate/{courseId}");
	}

	public async Task RemoveStudent(int courseId, int studentId)
	{
		await client.DeleteAsync($"api/PianoLessons/course/{courseId}/student/{studentId}");
	}

    public async Task<bool> JoinCourse(int studentId, string code)
	{
		return await client.GetFromJsonAsync<bool>($"api/PianoLessons/invite/{studentId}/{code}");
	}

    public async Task<List<Recording>> GetStudentCourseRecordings(int studentId, int courseId)
	{
		return await client.GetFromJsonAsync<List<Recording>>($"api/PianoLessons/recording/student/{studentId}/course/{courseId}");
	}
}


using PianoLessons.Shared.Data;
using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace PianoLessons.Services;

public class PianoLessonsService
{
	private readonly HttpClient client;
	private readonly string version = "1.0";

	public PianoLessonsService(HttpClient client)
	{
		this.client = client;
	}

	public async Task<List<Appointment>> GetAppointmentsForTeacher(string teacherId)
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

	public async Task<List<Appointment>> GetAppointmentsForStudent(string studentId)
	{
		var appointments = await client.GetFromJsonAsync<List<Appointment>>($"api/PianoLessons/appointments/student/{studentId}");
		return appointments;
	}

	public async Task<List<StudentScore>> GetScoresForCourseAndTime(int courseId, string time)
	{
		var request = new HttpRequestMessage(HttpMethod.Get, $"api/PianoLessons/scores/{courseId}/{time}");
		request.Headers.Add("version", version);

		var response = await client.SendAsync(request);

		if (response.IsSuccessStatusCode)
		{
			var json = await response.Content.ReadAsStringAsync();
			var objects = JsonSerializer.Deserialize<List<StudentScore>>(json);
			return objects;
		}
		var errorMessage = $"Failed to get scores for course {courseId}. Status code: {response.StatusCode}";
		throw new HttpRequestException(errorMessage);
	}

	public async Task<List<Student>> GetStudentsForTeacher(string teacherId)
	{
		var request = new HttpRequestMessage(HttpMethod.Get, $"api/PianoLessons/students/{teacherId}");
		request.Headers.Add("version", version);

		var response = await client.SendAsync(request);

		if (response.IsSuccessStatusCode)
		{
			var json = await response.Content.ReadAsStringAsync();
			var objects = JsonSerializer.Deserialize<List<Student>>(json);
			return objects;
		}
		var errorMessage = $"Failed to get students for teacher {teacherId}. Status code: {response.StatusCode}";
		throw new HttpRequestException(errorMessage);
	}

	public async Task<List<PracticeLog>> GetAllStudentLogsForTeacher(string teacherId)
	{
		return await client.GetFromJsonAsync<List<PracticeLog>>($"api/PianoLessons/logs/all/{teacherId}");
	}

	public async Task<List<PracticeLog>> GetLogsForStudent(string studentId)
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

	public async Task<List<Course>> GetTeacherCourses(string teacherId)
	{
		return await client.GetFromJsonAsync<List<Course>>($"api/PianoLessons/courses/teacher/{teacherId}");
	}

	public async Task<List<Course>> GetStudentCourses(string studentId)
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

	public async Task<bool> IsTeacher(string teacherId)
	{
		return await client.GetFromJsonAsync<bool>($"api/PianoLessons/isTeacher/{teacherId}");
	}

    public async Task<bool> IsStudent(string studentId)
    {
        return await client.GetFromJsonAsync<bool>($"api/PianoLessons/isStudent/{studentId}");
    }

    public async Task<bool> IsUser(string userId)
    {
        return await client.GetFromJsonAsync<bool>($"api/PianoLessons/isUser/{userId}");
    }

	public async Task<PianoLessonsUser> GetUser(string userId)
	{
		try
		{
			return await client.GetFromJsonAsync<PianoLessonsUser>($"api/PianoLessons/user/{userId}");

		}
		catch (Exception ex)
		{
			return null;
		}
    }

	public async Task RegisterUser(PianoLessonsUser user)
	{
		var result = await client.PostAsJsonAsync($"api/PianoLessons/user", user);
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

	public async Task<List<Student>> GetCourseStudents(int courseId)
	{
		return await client.GetFromJsonAsync<List<Student>>($"api/PianoLessons/course/{courseId}/students");
	}

	public async Task<string> GenerateCourseInvite(int courseId)
	{
		return await client.GetStringAsync($"api/PianoLessons/invite/generate/{courseId}");
	}

	public async Task RemoveStudent(int courseId, string studentId)
	{
		await client.DeleteAsync($"api/PianoLessons/course/{courseId}/student/{studentId}");
	}

    public async Task<bool> JoinCourse(string studentId, string code)
	{
		return await client.GetFromJsonAsync<bool>($"api/PianoLessons/invite/{studentId}/{code}");
	}

    public async Task<List<Recording>> GetStudentCourseRecordings(string studentId, int courseId)
	{
		return await client.GetFromJsonAsync<List<Recording>>($"api/PianoLessons/recording/student/{studentId}/course/{courseId}");
	}
}


using PianoLessons.Shared.Data;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using PianoLessons.Models;

namespace PianoLessons.Services;

public class PianoLessonsService
{
	private readonly HttpClient clientV1;
	private readonly HttpClient clientV2;

    public PianoLessonsService(HttpClient clientV1, HttpClient clientV2)
    {
        this.clientV1 = clientV1;
		this.clientV2 = clientV2;
	}

    public async Task<List<Appointment>> GetAppointmentsForTeacher(string teacherId)
    {
        var appointments = await clientV1.GetFromJsonAsync<List<Appointment>>($"api/PianoLessons/appointments/teacher/{teacherId}");
        return appointments;
    }

    public async Task<bool> AddAppointment(Appointment appointment)
    {
        var result = await clientV1.PostAsJsonAsync($"api/PianoLessons/appointments", appointment);
        if (result.IsSuccessStatusCode) return true;
        return false;
    }

    public async Task<List<Appointment>> GetAppointmentsForStudent(string studentId)
    {
        var appointments = await clientV1.GetFromJsonAsync<List<Appointment>>($"api/PianoLessons/appointments/student/{studentId}");
        return appointments;
    }

	public async Task<List<StudentScore>> GetScoresForCourseAndTime(int courseId, string time)
	{
        //return await clientV1.GetFromJsonAsync<List<StudentScore>>($"api/PianoLessons/scores/{courseId}/{time}");
        return await clientV2.GetFromJsonAsync<List<StudentScore>>($"api/PianoLessons/scores/{courseId}/{time}");
	}

	public async Task<List<Student>> GetStudentsForTeacher(string teacherId)
	{
		return await clientV1.GetFromJsonAsync<List<Student>>($"api/PianoLessons/students/{teacherId}");
		//return await clientV2.GetFromJsonAsync<List<Student>>($"api/PianoLessons/students/{teacherId}");
	}

    public async Task<List<PracticeLog>> GetAllStudentLogsForTeacher(string teacherId)
    {
        return await clientV1.GetFromJsonAsync<List<PracticeLog>>($"api/PianoLessons/logs/all/{teacherId}");
    }

    public async Task<List<PracticeLog>> GetLogsForStudent(string studentId)
    {
        return await clientV1.GetFromJsonAsync<List<PracticeLog>>($"api/PianoLessons/logs/{studentId}");
    }

	public async Task<List<PracticeLog>> GetLogsForStudentAndTeacher(string studentId, string teacherId)
	{
		return await clientV1.GetFromJsonAsync<List<PracticeLog>>($"api/PianoLessons/logs/student/{studentId}/teacher/{teacherId}");
	}

	public async Task DeleteLog(int logId)
    {
        await clientV1.DeleteAsync($"api/PianoLessons/logs/{logId}");
    }

    public async Task UpdateLog(PracticeLog newLog)
    {
        var log = JsonSerializer.Serialize(newLog);
        var requestContent = new StringContent(log, Encoding.UTF8, "application/json");
        await clientV1.PutAsync($"api/PianoLessons/logs", requestContent);
    }

    public async Task<List<Course>> GetTeacherCourses(string teacherId)
    {
        return await clientV1.GetFromJsonAsync<List<Course>>($"api/PianoLessons/courses/teacher/{teacherId}");
    }

    public async Task<List<Course>> GetStudentCourses(string studentId)
    {
        return await clientV1.GetFromJsonAsync<List<Course>>($"api/PianoLessons/courses/student/{studentId}");
    }

    public async Task AddLog(PracticeLog log)
    {
        await clientV1.PostAsJsonAsync("api/PianoLessons/logs", log);
    }

    public async Task<PracticeLog> GetLog(int logId)
    {
        return await clientV1.GetFromJsonAsync<PracticeLog>($"api/PianoLessons/logs/log/{logId}");
    }

    public async Task<bool> IsTeacher(string teacherId)
    {
        return await clientV1.GetFromJsonAsync<bool>($"api/PianoLessons/isTeacher/{teacherId}");
    }

    public async Task<bool> IsStudent(string studentId)
    {
        return await clientV1.GetFromJsonAsync<bool>($"api/PianoLessons/isStudent/{studentId}");
    }

    public async Task<bool> IsUser(string userId)
    {
        return await clientV1.GetFromJsonAsync<bool>($"api/PianoLessons/isUser/{userId}");
    }

    public async Task<PianoLessonsUser> GetUser(string userId)
    {
        try
        {
            var user = await clientV1.GetFromJsonAsync<PianoLessonsUser>($"api/PianoLessons/user/{userId}");
            return user;
        }
        catch
        {
            return null;
        }
    }

    public async Task RegisterUser(PianoLessonsUser user)
    {
        var result = await clientV1.PostAsJsonAsync($"api/PianoLessons/user", user);
    }

    public async Task AddCourse(Course course)
    {
        await clientV1.PostAsJsonAsync("api/PianoLessons/course", course);
    }

    public async Task DeleteCourse(int courseId)
    {
        await clientV1.DeleteAsync($"api/PianoLessons/course/{courseId}");
    }

    public async Task UpdateCourse(int courseId, string newName)
    {
        await clientV1.PutAsync($"api/PianoLessons/course/{courseId}/{newName}", null);
    }

    public async Task<Course> GetCourse(int id)
    {
        return await clientV1.GetFromJsonAsync<Course>($"api/PianoLessons/course/{id}");
    }

    public async Task<List<Student>> GetCourseStudents(int courseId)
    {
        return await clientV1.GetFromJsonAsync<List<Student>>($"api/PianoLessons/course/{courseId}/students");
    }

    public async Task<string> GenerateCourseInvite(int courseId)
    {
        return await clientV1.GetStringAsync($"api/PianoLessons/invite/generate/{courseId}");
    }

    public async Task RemoveStudent(int courseId, string studentId)
    {
        await clientV1.DeleteAsync($"api/PianoLessons/course/{courseId}/student/{studentId}");
    }

    public async Task<bool> JoinCourse(string studentId, string code)
    {
        return await clientV1.GetFromJsonAsync<bool>($"api/PianoLessons/invite/{studentId}/{code}");
    }

    public async Task<List<Recording>> GetStudentCourseRecordings(string studentId, int courseId)
    {
        return await clientV1.GetFromJsonAsync<List<Recording>>($"api/PianoLessons/recording/student/{studentId}/course/{courseId}");
        //return await clientV2.GetFromJsonAsync<List<Recording>>($"api/PianoLessons/recording/student/{studentId}/course/{courseId}");
	}

    public async Task AddRecording(FileData data, string studentId, int courseId)
    {
        var response = await clientV1.PostAsJsonAsync($"api/PianoLessons/recording/student/{studentId}/course/{courseId}", data);
    }

    public async Task DeleteRecording(string studentId, int recordingId, string fileName)
    {
        var response = await clientV1.DeleteAsync($"api/PianoLessons/recording/student/{studentId}/{recordingId}/{fileName}");
    }
}


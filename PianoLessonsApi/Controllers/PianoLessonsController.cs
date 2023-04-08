using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PianoLessons.Shared.Data;
using PianoLessonsApi.Data;
using System.Collections.ObjectModel;

namespace PianoLessonsApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class PianoLessonsController : ControllerBase
	{
		private readonly IPianoLessonsApplication app;

		public PianoLessonsController(IPianoLessonsApplication app)
		{
			this.app = app;
		}

		[HttpGet("appointments/teacher/{teacherId}")]
		public async Task<List<Appointment>> GetAppointmentsForTeacher(string teacherId)
		{
			return await app.GetAppointmentsForTeacher(teacherId);
		}

		[HttpPost("appointments")]
		public async Task AddAppointment(Appointment appointment)
		{
			await app.AddAppointment(appointment);
		}

		[HttpGet("appointments/student/{studentId}")]
		public async Task<List<Appointment>> GetAppointmentsForStudent(string studentId)
		{
			return await app.GetAppointmentsForStudent(studentId);
		}

		[HttpGet("students/{teacherId}")]
		public async Task<List<Student>> GetStudentsForTeacher(string teacherId)
		{
			var versionHeader = Request.Headers["version"].FirstOrDefault();
			return await app.GetStudentsForTeacher(teacherId, versionHeader);
		}

		[HttpGet("logs/all/{teacherId}")]
		public async Task<List<PracticeLog>> GetAllStudentLogsForTeacher(string teacherId)
		{
			return await app.GetAllStudentLogsForTeacher(teacherId);
		}

		[HttpGet("logs/{studentId}")]
		public async Task<List<PracticeLog>> GetLogsForStudent(string studentId)
		{
			return await app.GetLogsForStudent(studentId);
		}

		[HttpGet("logs/student/{studentId}/teacher/{teacherId}")]
		public async Task<List<PracticeLog>> GetLogsForStudentAndTeacher(string studentId, string teacherId)
		{
			return await app.GetLogsForStudentAndTeacher(studentId, teacherId);
		}

		[HttpGet("logs/log/{logId}")]
		public async Task<PracticeLog> GetLog(int logId)
		{
			return await app.GetLog(logId);
		}

		[HttpDelete("logs/{logId}")]
		public async Task DeleteLog(int logId)
		{
			await app.DeleteLog(logId);
		}

		[HttpPut("logs")]
		public async Task UpdateLog([FromBody] PracticeLog newLog)
		{
			await app.UpdateLog(newLog);
		}

		[HttpPost("logs")]
		public async Task AddLog(PracticeLog log)
		{
			await app.AddLog(log);
		}

		[HttpGet("courses/teacher/{teacherId}")]
		public async Task<List<Course>> GetTeacherCourses(string teacherId)
		{
			return await app.GetTeacherCourses(teacherId);
		}

		[HttpGet("courses/student/{studentId}")]
		public async Task<List<Course>> GetStudentCourses(string studentId)
		{
			return await app.GetStudentCourses(studentId);
		}

		[HttpGet, Route("scores/{courseId}/{time}"), HttpHeader("version", "1.0")]
		public async Task<IActionResult> GetPracticeScoresForCourseV1(int courseId, string time)
		{
			var scores = await app.GetPracticeScores(courseId, time, 10);
			return Ok(scores);
		}

		[HttpGet, Route("scores/{courseId}/{time}"), HttpHeader("version", "2.0")]
		public async Task<IActionResult> GetPracticeScoresForCourseV2(int courseId, string time)
		{
			var scores = await app.GetPracticeScores(courseId, time, 1000);
			return Ok(scores);
		}

		[HttpGet("isTeacher/{teacherId}")]
		public async Task<bool> IsTeacher(string teacherId)
		{
			return await app.IsTeacher(teacherId);
		}

		[HttpGet("isStudent/{studentId}")]
		public async Task<bool> IsStudent(string studentId)
		{
			return await app.IsStudent(studentId);
		}

        [HttpGet("isUser/{userId}")]
        public async Task<bool> IsUser(string userId)
        {
            return await app.IsStudent(userId);
        }

		[HttpGet("user/{userId}")]
		public async Task<PianoLessonsUser> GetUser(string userId)
		{
			return await app.GetUser(userId);
		}

        [HttpPost("user")]
        public async Task RegisterUser(PianoLessonsUser user)
		{
			await app.RegisterUser(user);
		}

        [HttpPost("course")]
		public async Task AddCourse([FromBody] Course course)
		{
			await app.AddCourse(course);
		}

		[HttpDelete("course/{courseId}")]
		public async Task DeleteCourse(int courseId)
		{
			await app.DeleteCourse(courseId);
		}

		[HttpPut("course/{courseId}/{newName}")]
		public async Task UpdateCourseName(int courseId, string newName)
		{
			await app.UpdateCourseName(courseId, newName);
		}

		[HttpGet("course/{id}")]
        public async Task<Course> GetCourse(int id)
		{
			return await app.GetCourse(id);
		}

		[HttpGet("course/{id}/students")]
		public async Task<List<Student>> GetCourseStudents(int id)
		{
			return await app.GetCourseStudents(id);
		}

		[HttpGet("invite/generate/{courseId}")]
		public async Task<string> GenerateCourseInvite(int courseId)
		{
			return await app.GenerateCourseInvite(courseId);
		}

		[HttpDelete("course/{courseId}/student/{studentId}")]
        public async Task RemoveStudent(int courseId, string studentId)
		{
			await app.RemoveStudent(courseId, studentId);
		}

		[HttpGet("invite/{studentId}/{code}")]
		public async Task<bool> JoinCourse(string studentId, string code)
		{
			return await app.JoinCourse(studentId, code);
		}

		[HttpGet, Route("recording/student/{studentId}/course/{courseId}"), HttpHeader("version", "1.0")]
		public async Task<List<Recording>> GetStudentCourseRecordingsV1(string studentId, int courseId)
		{
			return await app.GetStudentCourseRecordings(courseId, studentId);
		}

		[HttpGet, Route("recording/student/{studentId}/course/{courseId}"), HttpHeader("version", "2.0")]
		public async Task<List<Recording>> GetStudentCourseRecordingsV2(string studentId, int courseId)
		{
			return await app.GetStudentCourseRecordings(courseId, studentId);
		}

		[HttpPost("recording/student/{studentId}/course/{courseId}")]
		public async Task AddRecording(string studentId, int courseId, FileData data)
		{
			await app.AddRecordingToAzure(data, studentId, courseId);
		}


    }
}

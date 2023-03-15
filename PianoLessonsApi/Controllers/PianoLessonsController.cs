using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PianoLessons.Shared.Data;
using PianoLessonsApi.Data;
using System.Collections.ObjectModel;

namespace PianoLessonsApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PianoLessonsController : ControllerBase
	{
		private readonly IPianoLessonsApplication app;

		public PianoLessonsController(IPianoLessonsApplication app)
		{
			this.app = app;
		}

		[HttpGet("appointments/{teacherId}")]
		public async Task<List<Appointment>> GetAppointmentsForTeacher(int teacherId)
		{
			return await app.GetAppointmentsForTeacher(teacherId);
		}

		[HttpGet("students/{teacherId}/{time}")]
		public async Task<List<Student>> GetStudentsScoresForTeacher(int teacherId, string time)
		{
			//need logic to calculate scores based on time
			return await app.GetStudentsScoresForTeacher(teacherId, time);
		}

		[HttpGet("students/{teacherId}")]
		public async Task<List<Student>> GetStudentsForTeacher(int teacherId)
		{
			return await app.GetStudentsForTeacher(teacherId);
		}

		[HttpGet("logs/all/{teacherId}")]
		public async Task<List<PracticeLog>> GetAllStudentLogsForTeacher(int teacherId)
		{
			return await app.GetAllStudentLogsForTeacher(teacherId);
		}

		[HttpGet("logs/{studentId}")]
		public async Task<List<PracticeLog>> GetLogsForStudent(int studentId)
		{
			return await app.GetLogsForStudent(studentId);
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

		[HttpGet("courses/{teacherId}")]
		public async Task<List<Course>> GetTeacherCourses(int teacherId)
		{
			return await app.GetTeacherCourses(teacherId);
		}

		[HttpGet("scores/{courseId}/{time}")]
		public async Task<List<StudentScore>> GetPracticeScoresForCourse(int courseId, string time)
		{
			return await app.GetPracticeScores(courseId, time);
		}
	}
}

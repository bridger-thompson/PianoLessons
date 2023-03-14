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
	}
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PianoLessons.Shared.Data;
using System.Collections.ObjectModel;

namespace PianoLessonsApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PianoLessonsController : ControllerBase
	{
		private readonly List<Student> students = new()
		{
			new()
			{
				Name = "Bob",
				Score = 500,
				Id = 1
			},
			new()
			{
				Name = "Bridger",
				Score = 1500,
				Id = 2
			},
			new()
			{
				Name = "Steve",
				Score = 0,
				Id = 3
			},
		};

		public PianoLessonsController()
		{

		}

		[HttpGet("appointments/{userId}")]
		public async Task<List<Appointment>> GetAppointmentsForUser(string userId)
		{
			//get from db
			DateTime today = DateTime.Today;
			DateTime yesterday = new(today.Year, today.Month, today.Day - 1);
			var appointments = new List<Appointment>
			{
				new Appointment()
				{
					StartAt = today.AddHours(9),
					EndAt = today.AddHours(11),
					Subject = "Client Meeting",
				},
				new Appointment()
				{
					StartAt = yesterday.AddHours(9),
					EndAt = today.AddHours(10),
					Subject = "Long Meeting",
				}
			};
			return appointments;
		}

		[HttpGet("students/{teacherId}/{time}")]
		public async Task<List<Student>> GetStudentsScoresForTeacher(string teacherId, string time)
		{
			//need logic to calculate scores based on time
			return students;
		}

		[HttpGet("students/{teacherId}")]
		public async Task<List<Student>> GetStudentsForTeacher(string teacherId)
		{
			return students;
		}

		[HttpGet("logs/{teacherId}/{student}")]
		public async Task<List<PracticeLog>> GetStudentLogs(string teacherId, string student)
		{
			List<PracticeLog> logs = new();
			List<PracticeLog> tempLogs = new()
			{
				new PracticeLog()
				{
					StudentId = 1,
					LogDate = DateTime.Now,
					Duration = TimeSpan.FromHours(1),
				},
				new PracticeLog()
				{
					StudentId = 1,
					LogDate = DateTime.Now,
					Duration = TimeSpan.FromMinutes(90),
				},
				new PracticeLog()
				{
					StudentId = 2,
					LogDate = DateTime.Now,
					Duration = TimeSpan.FromMinutes(90),
				},
			};
			foreach (var log in tempLogs)
			{
				if (student == "All")
				{
					logs.Add(log);
				}
			}
			return logs;
		}
	}
}

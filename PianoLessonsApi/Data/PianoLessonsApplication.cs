using PianoLessons.Shared.Data;
using PianoLessonsApi.Repositories;
using System.Security.Cryptography.X509Certificates;

namespace PianoLessonsApi.Data;

public class PianoLessonsApplication : IPianoLessonsApplication
{
	private readonly IPianoLessonsRepo repo;

	public PianoLessonsApplication(IPianoLessonsRepo repo)
	{
		this.repo = repo;
	}

    public async Task AddLog(PracticeLog log)
    {
        await repo.AddLog(log);	
    }

    public async Task DeleteLog(int logId)
	{
		await repo.DeleteLog(logId);
	}

	public async Task<List<PracticeLog>> GetAllStudentLogsForTeacher(int teacherId)
	{
		var logs = await repo.GetAllStudentLogsForTeacher(teacherId);
		return logs;
	}

	public async Task<List<Appointment>> GetAppointmentsForTeacher(int teacherId)
	{
		return await repo.GetAppointmentsForTeacher(teacherId);
	}

	public async Task<List<Appointment>> GetAppointmentsForStudent(int studentId)
	{
		return await repo.GetAppointmentsForStudent(studentId);
	}

    public async Task<PracticeLog> GetLog(int logId)
    {
        return await repo.GetLog(logId);	
    }

    public async Task<List<PracticeLog>> GetLogsForStudent(int studentId)
	{
		return await repo.GetLogsForStudent(studentId);
	}

	public async Task<List<StudentScore>> GetPracticeScores(int courseId, string time)
	{
		DateTime today = DateTime.Today;
		var startDate = time switch
		{
			"Today" => new DateTime(today.Year, today.Month, today.Day, 0, 0, 0),
			"Week" => DateTime.Now.AddDays(-7),
			"Month" => DateTime.Now.AddMonths(-1),
			"Year" => DateTime.Now.AddYears(-1),
			"Ever" => new DateTime(),
			_ => new DateTime(),
		};
		var logs = await repo.GetPracticeLogsForCourseAndStartDate(courseId, startDate);
		return CalculateScores(logs);
	}

	private static List<StudentScore> CalculateScores(List<PracticeLog> logs)
	{
		List<StudentScore> scores = new();
		foreach (var log in logs)
		{
			if (scores.Exists(s => s.Name == log.Student.Name))
			{
				var studentScore = scores.Find(s => s.Name == log.Student.Name);
				var index = scores.IndexOf(studentScore);
				scores[index].Score += (int)(log.EndTime - log.StartTime).TotalMinutes;
			}
			else
			{
				scores.Add(new()
				{
					Id = log.Student.Id,
					Name = log.Student.Name,
					Score = (int)(log.EndTime - log.StartTime).TotalMinutes * 10
				});
			}
		}

		scores = scores.OrderByDescending(s => s.Score).ToList();

		var rank = 1;
		foreach (var score in scores)
		{
			score.Rank = rank++;
		}

		return scores.OrderByDescending(s => s.Score).ToList();
	}

	public async Task<List<Student>> GetStudentsForTeacher(int teacherId)
	{
		return await repo.GetStudentsForTeacher(teacherId);
	}

	public async Task<List<Student>> GetStudentsScoresForTeacher(int teacherId, string time)
	{
		return await repo.GetStudentsForTeacher(teacherId);
	}

	public async Task<List<Course>> GetTeacherCourses(int teacherId)
	{
		return await repo.GetTeacherCourses(teacherId);
	}

	public async Task<List<Course>> GetStudentCourses(int studentId)
	{
		return await repo.GetStudentCourses(studentId);
	}

	public async Task UpdateLog(PracticeLog newLog)
	{
		await repo.UpdateLog(newLog);
	}

    public async Task<List<PracticeAssignment>> GetStudentAssignments(int studentId)
	{
		return await repo.GetStudentAssignments(studentId);
	}

	public async Task<bool> IsTeacher(int teacherId)
	{
		return await repo.IsTeacher(teacherId);
	}

	public async Task AddCourse(Course course)
	{
		await repo.AddCourse(course);
	}

	public async Task DeleteCourse(int courseId)
	{
		await repo.DeleteCourse(courseId);
	}

	public async Task UpdateCourseName(int id, string newName)
	{
		await repo.UpdateCourseName(id, newName);
	}
}

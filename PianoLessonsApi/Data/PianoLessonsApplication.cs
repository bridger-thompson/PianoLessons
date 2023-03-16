using PianoLessons.Shared.Data;
using PianoLessonsApi.Repositories;

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
			"Week" => new DateTime(today.Year, today.Month, today.Day - 7),
			"Month" => new DateTime(today.Year, today.Month - 1, today.Day),
			"Year" => new DateTime(today.Year - 1, today.Month, today.Day),
			"Ever" => new DateTime(),
			_ => new DateTime(),
		};
		var logs = await repo.GetPracticeScores(courseId, startDate);

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
					Id = log.StudentId,
					Name = log.Student.Name,
					Score = (int)(log.EndTime - log.StartTime).TotalMinutes
				});
			}
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

	public async Task UpdateLog(PracticeLog newLog)
	{
		await repo.UpdateLog(newLog);
	}

    public async Task<List<PracticeAssignment>> GetStudentAssignments(int studentId)
	{
		return await repo.GetStudentAssignments(studentId);
	}
}

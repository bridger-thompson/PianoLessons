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
        DateTime startDate = GetStartDate(time, DateTime.Today);
        var students = await repo.GetCourseStudents(courseId);
        var studentScores = new List<StudentScore>();
        foreach (var student in students)
        {
            StudentScore score = await GetStudentScoreForCourseAndStartDates(student, courseId, startDate);
            studentScores.Add(score);
        }

        return GetRankedStudentScores(studentScores);
    }

    private static List<StudentScore> GetRankedStudentScores(List<StudentScore> studentScores)
    {
        var rank = 1;
        var orderedStudentScores = studentScores.OrderByDescending(s => s.Score);
        foreach (var score in orderedStudentScores)
        {
            score.Rank = rank++;
        }

        return orderedStudentScores.ToList();
    }

    private static DateTime GetStartDate(string time, DateTime today)
    {
        return time switch
        {
            "Today" => new DateTime(today.Year, today.Month, today.Day, 0, 0, 0),
            "Week" => DateTime.Now.AddDays(-7),
            "Month" => DateTime.Now.AddMonths(-1),
            "Year" => DateTime.Now.AddYears(-1),
            "Ever" => new DateTime(),
            _ => new DateTime(),
        };
    }

    private async Task<StudentScore> GetStudentScoreForCourseAndStartDates(Student student, int courseId, DateTime startDate)
    {
		var practiceLogs = await repo.GetStudentsPracticeLogsForCourseAndDate(student.Id, courseId, startDate);

		var score = 0;

		foreach(var log in practiceLogs)
		{
			score += (int)log.Duration.TotalMinutes * 10;
		}

		return new StudentScore { Id = student.Id, Name = student.Name, Score = score};
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

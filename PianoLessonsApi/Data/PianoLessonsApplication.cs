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

	public async Task<List<PracticeLog>> GetAllStudentLogsForTeacher(string teacherId)
	{
		var logs = await repo.GetAllStudentLogsForTeacher(teacherId);
		return logs;
	}

	public async Task<List<Appointment>> GetAppointmentsForTeacher(string teacherId)
	{
		return await repo.GetAppointmentsForTeacher(teacherId);
	}

	public async Task AddAppointment(Appointment appointment)
	{
		await repo.AddAppointment(appointment);
	}

	public async Task<List<Appointment>> GetAppointmentsForStudent(string studentId)
	{
		return await repo.GetAppointmentsForStudent(studentId);
	}

    public async Task<PracticeLog> GetLog(int logId)
    {
        return await repo.GetLog(logId);	
    }

    public async Task<List<PracticeLog>> GetLogsForStudent(string studentId)
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

		int score = CalculateScore(practiceLogs);

		return new StudentScore { Id = student.Id, Name = student.Name, Score = score };
    }

	public int CalculateScore(List<PracticeLog> logs)
	{
		int score = 0;
		foreach (var log in logs)
		{
			score += (int)log.Duration.TotalMinutes * 10;
		}
		return score;
	}

	public async Task<List<Student>> GetStudentsForTeacher(string teacherId)
	{
		return await repo.GetStudentsForTeacher(teacherId);
	}

	public async Task<List<Student>> GetStudentsScoresForTeacher(string teacherId, string time)
	{
		return await repo.GetStudentsForTeacher(teacherId);
	}

	public async Task<List<Course>> GetTeacherCourses(string teacherId)
	{
		return await repo.GetTeacherCourses(teacherId);
	}

	public async Task<List<Course>> GetStudentCourses(string studentId)
	{
		return await repo.GetStudentCourses(studentId);
	}

	public async Task UpdateLog(PracticeLog newLog)
	{
		await repo.UpdateLog(newLog);
	}

    public async Task<List<PracticeAssignment>> GetStudentAssignments(string studentId)
	{
		return await repo.GetStudentAssignments(studentId);
	}

	public async Task<bool> IsTeacher(string teacherId)
	{
		return await repo.IsTeacher(teacherId);
	}

	public async Task<bool> IsStudent(string studentId)
	{
		return await repo.IsStudent(studentId);
	}

	public async Task<bool> IsUser(string userId)
	{
		return await repo.IsUser(userId);	
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

    public async Task<Course> GetCourse(int id)
	{
		return await repo.GetCourse(id);
	}

	public async Task<List<Student>> GetCourseStudents(int id)
	{
        return await repo.GetCourseStudents(id);
    }

	public async Task<string> GenerateCourseInvite(int courseId)
    {
        var code = GenerateRandomCode();
        CourseInvite invite = new()
        {
            Id = 0,
            CourseId = courseId,
			Code = code,
			ExpireDate = DateTime.Now.AddDays(1)
        };
		await repo.GenerateCourseInvite(invite);
		return code;
    }

    private static string GenerateRandomCode()
    {
        Random rand = new();
        string str = "";
        char letter;
        for (int i = 0; i < 4; i++)
        {
            int randValue = rand.Next(0, 26);
            letter = Convert.ToChar(randValue + 65);
            str += letter;
        }
		return str;
    }

    public async Task<bool> JoinCourse(string studentId, string code)
	{
		return await repo.JoinCourse(studentId, code);
	}

    public async Task RemoveStudent(int courseId, string studentId)
	{
		await repo.RemoveStudent(courseId, studentId);
	}

    public async Task<List<Recording>> GetStudentCourseRecordings(int courseId, string studentId)
	{
		return await repo.GetStudentCourseRecordings(courseId, studentId);
	}

	public async Task<PianoLessonsUser> GetUser(string userId)
	{
		if (await IsTeacher(userId))
		{
			var teacher = await repo.GetTeacher(userId);
			return new PianoLessonsUser(teacher);
		}
		else if (await IsStudent(userId))
		{
			var student = await repo.GetStudent(userId);
			return new PianoLessonsUser(student);
		}

		return null;
	}

	public async Task RegisterUser(PianoLessonsUser user)
	{
		await repo.RegisterUser(user);
	}
}

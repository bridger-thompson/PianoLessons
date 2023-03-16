using Microsoft.EntityFrameworkCore;
using PianoLessons.Shared.Data;
using PianoLessonsApi.Data;
namespace PianoLessonsApi.Repositories;

public class PianoLessonRepo : IPianoLessonsRepo
{
	private readonly PianoLessonDbContext context;

	public PianoLessonRepo(PianoLessonDbContext context)
	{
		this.context = context;
	}

	public async Task<List<Appointment>> GetAppointmentsForTeacher(int teacherid)
	{
		return await context.Appointments.Where(a => a.TeacherId == teacherid).ToListAsync();
	}

	public async Task<List<PracticeLog>> GetAllStudentLogsForTeacher(int teacherId)
	{
		return await context.PracticeLogs.Include(l => l.Student)
			.ThenInclude(s => s.StudentCourses)
			.ThenInclude(sc => sc.Course)
			.ThenInclude(c => c.Teacher)
			.Where(l => l.Student.StudentCourses.Any(c => c.Course.TeacherId == teacherId))
			.ToListAsync();
	}

	public async Task<List<Student>> GetStudentsForTeacher(int teacherId)
	{
		return await context.Students.Include(s => s.StudentCourses)
			.ThenInclude(sc => sc.Course)
			.ThenInclude(c => c.Teacher)
			.Where(s => s.StudentCourses.Any(c => c.Course.TeacherId == teacherId))
			.ToListAsync();
	}

	public async Task<List<PracticeLog>> GetLogsForStudent(int studentId)
	{
		return await context.PracticeLogs
			.Include(l => l.Student)
			.Where(l => l.StudentId == studentId)
			.ToListAsync();
	}

	public async Task DeleteLog(int logId)
	{
		var log = await context.PracticeLogs.FirstOrDefaultAsync(l => l.Id == logId);
		if (log != null)
		{
			context.PracticeLogs.Remove(log);
			await context.SaveChangesAsync();
		}
	}

	public async Task UpdateLog(PracticeLog newLog)
	{
		var log = await context.PracticeLogs.FirstOrDefaultAsync(l => l.Id == newLog.Id);
		if (log != null)
		{
			log.Duration = newLog.Duration;
			log.StartTime = newLog.StartTime;
			log.EndTime = newLog.EndTime;
			log.Notes = newLog.Notes;
			log.AssignmentId = newLog.AssignmentId;
			log.StudentId = newLog.StudentId;
			await context.SaveChangesAsync();
		}
	}

    public async Task AddLog(PracticeLog log)
    {
		await context.PracticeLogs.AddAsync(log);
		await context.SaveChangesAsync();
    }

    public async Task<PracticeLog> GetLog(int logId)
    {
		return await context.PracticeLogs.FirstOrDefaultAsync(l => l.Id == logId);
    }

	public async Task<List<PracticeLog>> GetPracticeScores(int courseId, DateTime startDate)
	{
		return await context.PracticeLogs
			.Include(l => l.Student)
			.ThenInclude(s => s.StudentCourses)
			.ThenInclude(sc => sc.Course)
			.Where(s => s.Student.StudentCourses.Any(sc => sc.CourseId == courseId))
			.Where(l => l.StartTime <= DateTime.Now && l.StartTime >= startDate)
			.ToListAsync();
	}

	public async Task<List<Course>> GetTeacherCourses(int teacherId)
	{
		return await context.Courses
			.Include(s => s.StudentCourses)
			.ThenInclude(sc => sc.Student)
			.Where(c => c.TeacherId == teacherId)
			.ToListAsync();
	}

    public async Task<List<PracticeAssignment>> GetStudentAssignments(int studentId)
	{
		return await context.PracticeAssignments
			.Include(sa => sa.StudentAssignments)
			.Where(sa => sa.StudentAssignments.Any(a => a.StudentId == studentId))
			.ToListAsync();
	}
}

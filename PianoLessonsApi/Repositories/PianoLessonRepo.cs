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
}

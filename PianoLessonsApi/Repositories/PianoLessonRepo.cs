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

	public async Task<List<Appointment>> GetAppointmentsForTeacher(string teacherid)
	{
		return await context.Appointments.Where(a => a.TeacherId == teacherid).ToListAsync();
	}

	public async Task AddAppointment(Appointment appointment)
	{
		await context.AddAsync(appointment);
		await context.SaveChangesAsync();
	}

	public async Task<List<Appointment>> GetAppointmentsForStudent(string studentId)
	{
		return await context.Appointments.Where(a => a.StudentId == studentId).ToListAsync();
	}

	public async Task<List<PracticeLog>> GetAllStudentLogsForTeacher(string teacherId)
	{
		return await context.PracticeLogs.Include(l => l.Student)
			.ThenInclude(s => s.StudentCourses)
			.ThenInclude(sc => sc.Course)
			.ThenInclude(c => c.Teacher)
			.Where(l => l.Student.StudentCourses.Any(c => c.Course.TeacherId == teacherId))
			.ToListAsync();
	}

	public async Task<List<Student>> GetStudentsForTeacher(string teacherId)
	{
		return await context.Students.Include(s => s.StudentCourses)
			.ThenInclude(sc => sc.Course)
			.ThenInclude(c => c.Teacher)
			.Where(s => s.StudentCourses.Any(c => c.Course.TeacherId == teacherId))
			.ToListAsync();
	}

	public async Task<List<PracticeLog>> GetLogsForStudent(string studentId)
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

	public async Task<List<PracticeLog>> GetPracticeLogsForCourseAndStartDate(int courseId, DateTime startDate)
	{
		return await context.PracticeLogs
			.Include(l => l.Student)
			.ThenInclude(s => s.StudentCourses)
			.ThenInclude(sc => sc.Course)
			.Where(s => s.Student.StudentCourses.Any(sc => sc.CourseId == courseId))
			.Where(l => l.StartTime <= DateTime.Now && l.StartTime >= startDate)
			.ToListAsync();
	}

	public async Task<List<PracticeLog>> GetStudentsPracticeLogsForCourseAndDate(string studentId, int courseId, DateTime startDate)
	{
		var logs = await context.PracticeLogs
			.Where(p => p.StudentId == studentId)
			.Include(p => p.Assignment)
			.Where(p => p.Assignment.CourseId == courseId)
            .Where(p => p.StartTime < DateTime.Now && p.StartTime > startDate)
            .ToListAsync();

		return logs;
	}

	public async Task<List<Course>> GetTeacherCourses(string teacherId)
	{
		return await context.Courses
			.Include(s => s.StudentCourses)
			.ThenInclude(sc => sc.Student)
			.Where(c => c.TeacherId == teacherId)
			.ToListAsync();
	}

	public async Task<List<Course>> GetStudentCourses(string studentId)
	{
		return await context.Courses
			.Include(c => c.StudentCourses)
			.Where(c => c.StudentCourses.Any(sc => sc.StudentId == studentId))
			.ToListAsync();
	}


	public async Task<List<PracticeAssignment>> GetStudentAssignments(string studentId)
	{
		return await context.PracticeAssignments
			.Include(sa => sa.StudentAssignments)
			.Where(sa => sa.StudentAssignments.Any(a => a.StudentId == studentId))
			.ToListAsync();
	}

	public async Task<bool> IsTeacher(string teacherId)
	{
		var teacher = await context.Teachers.Where(t => t.Id == teacherId).FirstOrDefaultAsync();
		return teacher != null;
	}

	public async Task<bool> IsStudent(string studentId)
	{
		var student = await context.Students.Where(t => t.Id == studentId).FirstOrDefaultAsync();
		return student != null;
	}

	public async Task<bool> IsUser(string userId)
	{
		return await IsStudent(userId) && await IsTeacher(userId);
	}

	public async Task AddCourse(Course course)
	{
		var teacher = await context.Teachers
			.Where(t => t.Id == course.TeacherId)
			.FirstOrDefaultAsync();
		course.Teacher = teacher;
		var existingCourse = await context.Courses
			.Where(c => c.TeacherId == course.TeacherId && c.Name == course.Name)
			.FirstOrDefaultAsync();
		if (existingCourse == null)
		{
			await context.Courses.AddAsync(course);
			await context.SaveChangesAsync();
		}
	}

	public async Task DeleteCourse(int courseId)
	{
		var existingCourse = await context.Courses
			.Where(c => c.Id == courseId)
			.FirstOrDefaultAsync();
		if (existingCourse != null)
		{
			context.Remove(existingCourse);
			await context.SaveChangesAsync();
		}
	}

	public async Task UpdateCourseName(int id, string newName)
	{
		var existingCourse = await context.Courses
			.Where(c => c.Id == id)
			.FirstOrDefaultAsync();
		if (existingCourse != null)
		{
			existingCourse.Name = newName;
			await context.SaveChangesAsync();
		}
	}

	public async Task<Course> GetCourse(int id)
	{
		return await context.Courses.Where(c => c.Id == id).FirstOrDefaultAsync();
	}

	public async Task<List<Student>> GetCourseStudents(int courseId)
	{
		var students = await context.Students
			.Include(s => s.StudentCourses)
			.Where(s => s.StudentCourses.Any(c => c.CourseId == courseId))
			.ToListAsync();

		return students;
	}

	public async Task GenerateCourseInvite(CourseInvite invite)
	{
		await context.CourseInvites.AddAsync(invite);
		await context.SaveChangesAsync();
	}

	public async Task<bool> JoinCourse(string studentId, string code)
	{
		var validInvite = await context.CourseInvites
			.Where(i => i.Code == code && i.ExpireDate > DateTime.Now && i.Used == false)
			.FirstOrDefaultAsync();
		if (validInvite != null)
		{
			StudentCourse sc = new()
			{
				Id = 0,
				StudentId = studentId,
				CourseId = validInvite.CourseId,
			};
			await context.AddAsync(sc);
			validInvite.Used = true;
			await context.SaveChangesAsync();
			return true;
		}
		return false;
	}

	public async Task RemoveStudent(int courseId, string studentId)
	{
		var sc = await context.StudentCourses
			.Where(sc => sc.CourseId == courseId && sc.StudentId == studentId)
			.FirstOrDefaultAsync();
		if (sc != null)
		{
			context.StudentCourses.Remove(sc);
			await context.SaveChangesAsync();
		}
	}

	public async Task<List<Recording>> GetStudentCourseRecordings(int courseId, string studentId)
	{
		var recordings = await context.Recordings
			.Include(r => r.Student)
			.Where(r => r.StudentId == studentId && r.CourseId == courseId)
			.ToListAsync();
		return recordings;
	}

	public async Task<Student> GetStudent(string studentId)
	{
		return await context.Students.FirstOrDefaultAsync(s => s.Id == studentId);
	}

    public async Task<Teacher> GetTeacher(string teacherId)
    {
        return await context.Teachers.FirstOrDefaultAsync(t => t.Id == teacherId);
    }

    public async Task RegisterUser(PianoLessonsUser user)
    {
        if (user.IsTeacher)
		{
			await context.Teachers.AddAsync(new Teacher(user));
		}
		else
		{
			await context.Students.AddAsync(new Student(user));
		}

		await context.SaveChangesAsync();
    }
}

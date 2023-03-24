using PianoLessons.Shared.Data;

namespace PianoLessonsApi.Repositories;

public interface IPianoLessonsRepo
{
	public Task<List<Appointment>> GetAppointmentsForTeacher(int teacherId);

	public Task<List<Student>> GetStudentsForTeacher(int teacherId);

	public Task<List<PracticeLog>> GetAllStudentLogsForTeacher(int teacherId);

	public Task<List<PracticeLog>> GetLogsForStudent(int studentId);

	public Task<PracticeLog> GetLog(int logId);

	public Task AddLog(PracticeLog log);

	public Task DeleteLog(int logId);

	public Task UpdateLog(PracticeLog newLog);

	public Task<List<PracticeLog>> GetPracticeLogsForCourseAndStartDate(int courseId, DateTime startDate);

	public Task<List<Course>> GetTeacherCourses(int teacherId);

	public Task<List<PracticeAssignment>> GetStudentAssignments(int studentId);
    Task<List<Student>> GetCourseStudents(int courseId);
    Task<bool> IsTeacher(int teacherId);
    Task AddCourse(Course course);
    Task DeleteCourse(int courseId);
    Task UpdateCourseName(int id, string newName);
    Task<List<Appointment>> GetAppointmentsForStudent(int studentId);
    Task<List<Course>> GetStudentCourses(int studentId);
    Task<List<PracticeLog>> GetStudentsPracticeLogsForCourseAndDate(int studentId, int courseId, DateTime startDate);
    Task<Course> GetCourse(int id);
    Task GenerateCourseInvite(CourseInvite invite);
    Task RemoveStudent(int courseId, int studentId);
    Task<bool> JoinCourse(int studentId, string code);
}

using PianoLessons.Shared.Data;

namespace PianoLessonsApi.Data;

public interface IPianoLessonsApplication
{
	public Task<List<Appointment>> GetAppointmentsForTeacher(string teacherId);

	public Task<List<Student>> GetStudentsForTeacher(string teacherId, string? version = "1.0");

	public Task<List<PracticeLog>> GetAllStudentLogsForTeacher(string teacherId);

	public Task<List<PracticeLog>> GetLogsForStudent(string studentId);

	public Task<List<PracticeLog>> GetLogsForStudentAndTeacher(string studentId, string teacherId);

	public Task AddLog(PracticeLog log);

	public Task DeleteLog(int logId);

	public Task UpdateLog(PracticeLog newLog);

	public Task<PracticeLog> GetLog(int logId);

	public Task<List<StudentScore>> GetPracticeScores(int courseId, string time, string? version = "1.0");

	public Task<List<Course>> GetTeacherCourses(string teacherId);

    Task<Course> GetCourse(int id);
    Task<List<Appointment>> GetAppointmentsForStudent(string studentId);
    Task UpdateCourseName(int id, string newName);
    Task DeleteCourse(int courseId);
    Task AddCourse(Course course);
    Task<bool> IsTeacher(string teacherId);
    Task<List<Course>> GetStudentCourses(string studentId);
    Task<List<Student>> GetCourseStudents(int id);
	int CalculateScore(List<PracticeLog> logs, int modifier);
    Task<string> GenerateCourseInvite(int courseId);
    Task RemoveStudent(int courseId, string studentId);
    Task<bool> JoinCourse(string studentId, string code);
    Task<List<Recording>> GetStudentCourseRecordings(int courseId, string studentId);
    Task AddAppointment(Appointment appointment);
    Task<bool> IsStudent(string studentId);
    Task<bool> IsUser(string userId);
    Task<PianoLessonsUser> GetUser(string userId);
    Task RegisterUser(PianoLessonsUser user);
}

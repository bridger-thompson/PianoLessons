using PianoLessons.Shared.Data;

namespace PianoLessonsApi.Repositories;

public interface IPianoLessonsRepo
{
	public Task<List<Appointment>> GetAppointmentsForTeacher(string teacherId);

	public Task<List<Student>> GetStudentsForTeacher(string teacherId);

	public Task<List<PracticeLog>> GetAllStudentLogsForTeacher(string teacherId);

	public Task<List<PracticeLog>> GetLogsForStudent(string studentId);

	public Task<PracticeLog> GetLog(int logId);

	public Task AddLog(PracticeLog log);

	public Task DeleteLog(int logId);

	public Task UpdateLog(PracticeLog newLog);

	public Task<List<PracticeLog>> GetPracticeLogsForCourseAndStartDate(int courseId, DateTime startDate);

	public Task<List<Course>> GetTeacherCourses(string teacherId);

    Task<List<Student>> GetCourseStudents(int courseId);
    Task<bool> IsTeacher(string teacherId);
    Task AddCourse(Course course);
    Task DeleteCourse(int courseId);
    Task UpdateCourseName(int courseId, string newName);
    Task<List<Appointment>> GetAppointmentsForStudent(string studentId);
    Task<List<Course>> GetStudentCourses(string studentId);
    Task<List<PracticeLog>> GetStudentsPracticeLogsForCourseAndDate(string studentId, int courseId, DateTime startDate);
    Task<Course> GetCourse(int courseId);
    Task GenerateCourseInvite(CourseInvite invite);
    Task RemoveStudent(int courseId, string studentId);
    Task<bool> JoinCourse(string studentId, string code);
	Task AddAppointment(Appointment appointment);
    Task<List<Recording>> GetStudentCourseRecordings(int courseId, string studentId);
    Task<bool> IsStudent(string studentId);
    Task<bool> IsUser(string userId);
    Task<Student> GetStudent(string studentId);
    Task<Teacher> GetTeacher(string teacherId);
    Task RegisterUser(PianoLessonsUser user);
	Task<List<PracticeLog>> GetLogsForStudentAndTeacher(string studentId, string teacherId);
	Task AddRecording(Recording recording);
	Task<Recording> GetFunRecording();
}

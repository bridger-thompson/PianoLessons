using PianoLessons.Shared.Data;

namespace PianoLessonsApi.Data;

public interface IPianoLessonsApplication
{
	public Task<List<Appointment>> GetAppointmentsForTeacher(int teacherId);

	public Task<List<Student>> GetStudentsScoresForTeacher(int teacherId, string time);

	public Task<List<Student>> GetStudentsForTeacher(int teacherId);

	public Task<List<PracticeLog>> GetAllStudentLogsForTeacher(int teacherId);

	public Task<List<PracticeLog>> GetLogsForStudent(int studentId);

	public Task AddLog(PracticeLog log);

	public Task DeleteLog(int logId);

	public Task UpdateLog(PracticeLog newLog);

	public Task<PracticeLog> GetLog(int logId);

	public Task<List<StudentScore>> GetPracticeScores(int courseId, string time);

	public Task<List<Course>> GetTeacherCourses(int teacherId);

    public Task<List<PracticeAssignment>> GetStudentAssignments(int studentId);
    Task<Course> GetCourse(int id);
    Task<List<Appointment>> GetAppointmentsForStudent(int studentId);
    Task UpdateCourseName(int id, string newName);
    Task DeleteCourse(int courseId);
    Task AddCourse(Course course);
    Task<bool> IsTeacher(int teacherId);
    Task<List<Course>> GetStudentCourses(int studentId);
    Task<List<Student>> GetCourseStudents(int id);
	int CalculateScore(List<PracticeLog> logs);
    Task<string> GenerateCourseInvite(int courseId);
    Task RemoveStudent(int courseId, int studentId);
    Task<bool> JoinCourse(int studentId, string code);
}

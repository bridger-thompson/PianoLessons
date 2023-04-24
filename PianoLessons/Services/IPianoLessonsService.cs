using PianoLessons.Shared.Data;

namespace PianoLessons.Services
{
	public interface IPianoLessonsService
	{
		Task<bool> AddAppointment(Appointment appointment);
		Task AddCourse(Course course);
		Task AddLog(PracticeLog log);
		Task AddRecording(FileData data, string studentId, int courseId);
		Task DeleteCourse(int courseId);
		Task DeleteLog(int logId);
		Task DeleteRecording(string studentId, int recordingId, string fileName);
		Task<string> GenerateCourseInvite(int courseId);
		Task<List<PracticeLog>> GetAllStudentLogsForTeacher(string teacherId);
		Task<List<Appointment>> GetAppointmentsForStudent(string studentId);
		Task<List<Appointment>> GetAppointmentsForTeacher(string teacherId);
		Task<Course> GetCourse(int id);
		Task<List<Student>> GetCourseStudents(int courseId);
		Task<PracticeLog> GetLog(int logId);
		Task<List<PracticeLog>> GetLogsForStudent(string studentId);
		Task<List<PracticeLog>> GetLogsForStudentAndTeacher(string studentId, string teacherId);
		Task<List<StudentScore>> GetScoresForCourseAndTime(int courseId, string time);
		Task<List<Recording>> GetStudentCourseRecordings(string studentId, int courseId);
		Task<List<Course>> GetStudentCourses(string studentId);
		Task<List<Student>> GetStudentsForTeacher(string teacherId);
		Task<List<Course>> GetTeacherCourses(string teacherId);
		Task<PianoLessonsUser> GetUser(string userId);
		Task<bool> IsStudent(string studentId);
		Task<bool> IsTeacher(string teacherId);
		Task<bool> IsUser(string userId);
		Task<bool> JoinCourse(string studentId, string code);
		Task RegisterUser(PianoLessonsUser user);
		Task RemoveStudent(int courseId, string studentId);
		Task UpdateCourse(int courseId, string newName);
		Task UpdateLog(PracticeLog newLog);
	}
}
using PianoLessons.Shared.Data;

namespace PianoLessonsApi.Data;

public interface IPianoLessonsApplication
{
	public Task<List<Appointment>> GetAppointmentsForTeacher(int teacherId);

	public Task<List<Student>> GetStudentsScoresForTeacher(int teacherId, string time);

	public Task<List<Student>> GetStudentsForTeacher(int teacherId);

	public Task<List<PracticeLog>> GetAllStudentLogsForTeacher(int teacherId);

	public Task<List<PracticeLog>> GetLogsForStudent(int studentId);

	public Task DeleteLog(int logId);

	public Task UpdateLog(PracticeLog newLog);
}

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
}

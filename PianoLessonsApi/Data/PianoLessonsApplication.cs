using PianoLessons.Shared.Data;
using PianoLessonsApi.Repositories;

namespace PianoLessonsApi.Data;

public class PianoLessonsApplication : IPianoLessonsApplication
{
	private readonly IPianoLessonsRepo repo;

	public PianoLessonsApplication(IPianoLessonsRepo repo)
	{
		this.repo = repo;
	}

	public async Task<List<PracticeLog>> GetAllStudentLogsForTeacher(int teacherId)
	{
		var logs = await repo.GetAllStudentLogsForTeacher(teacherId);
		return logs;
	}

	public async Task<List<Appointment>> GetAppointmentsForTeacher(int teacherId)
	{
		return await repo.GetAppointmentsForTeacher(teacherId);
	}

	public async Task<List<PracticeLog>> GetLogsForStudent(int studentId)
	{
		return await repo.GetLogsForStudent(studentId);
	}

	public async Task<List<Student>> GetStudentsForTeacher(int teacherId)
	{
		return await repo.GetStudentsForTeacher(teacherId);
	}

	public async Task<List<Student>> GetStudentsScoresForTeacher(int teacherId, string time)
	{
		return await repo.GetStudentsForTeacher(teacherId);
	}
}

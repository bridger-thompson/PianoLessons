using PianoLessons.Shared.Data;

namespace PianoLessonsApi.Repositories;

public interface IPianoLessonsRepo
{
	public Task<List<Appointment>> GetAppointmentsForTeacher(int teacherId);

	public Task<List<Student>> GetStudentsForTeacher(int teacherId);

	public Task<List<PracticeLog>> GetAllStudentLogsForTeacher(int teacherId);
}

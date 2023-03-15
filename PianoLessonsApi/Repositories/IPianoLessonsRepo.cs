﻿using PianoLessons.Shared.Data;

namespace PianoLessonsApi.Repositories;

public interface IPianoLessonsRepo
{
	public Task<List<Appointment>> GetAppointmentsForTeacher(int teacherId);

	public Task<List<Student>> GetStudentsForTeacher(int teacherId);

	public Task<List<PracticeLog>> GetAllStudentLogsForTeacher(int teacherId);

	public Task<List<PracticeLog>> GetLogsForStudent(int studentId);

	public Task DeleteLog(int logId);

	public Task UpdateLog(PracticeLog newLog);

	public Task<List<PracticeLog>> GetPracticeScores(int courseId, DateTime startDate);

	public Task<List<Course>> GetTeacherCourses(int teacherId);
}

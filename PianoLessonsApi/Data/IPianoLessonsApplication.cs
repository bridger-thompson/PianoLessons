﻿using PianoLessons.Shared.Data;

namespace PianoLessonsApi.Data;

public interface IPianoLessonsApplication
{
	public Task<List<Appointment>> GetAppointmentsForTeacher(int teacherId);

	public Task<List<Appointment>> GetAppointmentsForStudent(int studentId);

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

	public Task<List<Course>> GetStudentCourses(int studentId);

	public Task<List<PracticeAssignment>> GetStudentAssignments(int studentId);

	public Task<bool> IsTeacher(int teacherId);
	Task AddCourse(Course course);
	Task DeleteCourse(int courseId);
	Task UpdateCourseName(int id, string newName);
}

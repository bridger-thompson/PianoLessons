using PianoLessons.Shared.Data;
using PianoLessonsApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoLessons.Tests;

public class TestPianoLessonRepo : IPianoLessonsRepo
{
    public List<PracticeLog> Logs { get; init; }

    public TestPianoLessonRepo()
    {
        Logs = new();
    }

    public async Task AddLog(PracticeLog log)
    {
        Logs.Add(log);
    }

    public async Task DeleteLog(int logId)
    {
        var log = Logs.FirstOrDefault(x => x.Id == logId);
        Logs.Remove(log);
    }

    public Task<List<PracticeLog>> GetAllStudentLogsForTeacher(string teacherId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Appointment>> GetAppointmentsForTeacher(string teacherId)
    {
        throw new NotImplementedException();
    }

    public async Task<PracticeLog> GetLog(int logId)
    {
        var log = Logs.FirstOrDefault(x => x.Id == logId);

        if (log is not null)
        {
            return log;
        }

        throw new Exception("Log Not Found!");
    }

    public Task<List<PracticeLog>> GetLogsForStudent(string studentId)
    {
        throw new NotImplementedException();
    }

    public Task<List<PracticeLog>> GetPracticeScores(int courseId, DateTime startDate)
    {
        throw new NotImplementedException();
    }

    public Task<List<PracticeAssignment>> GetStudentAssignments(string studentId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Student>> GetStudentsForTeacher(string teacherId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Course>> GetTeacherCourses(string teacherId)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateLog(PracticeLog updatedLog)
    {
        var log = Logs.FirstOrDefault(x => x.Id == updatedLog.Id);

        if (log is not null) 
        {
            log.Id = updatedLog.Id;
            log.StudentId = updatedLog.StudentId;
            log.StartTime = updatedLog.StartTime;
            log.EndTime = updatedLog.EndTime;
            log.Notes = updatedLog.Notes;
            log.AssignmentId = updatedLog.AssignmentId;
        }
    }

    public Task<List<PracticeLog>> GetPracticeLogsForCourseAndStartDate(int courseId, DateTime startDate)
    {
        throw new NotImplementedException();
    }

	public Task<List<Student>> GetCourseStudents(int courseId)
	{
		throw new NotImplementedException();
	}

	public Task<bool> IsTeacher(string teacherId)
	{
		throw new NotImplementedException();
	}

	public Task AddCourse(Course course)
	{
		throw new NotImplementedException();
	}

	public Task DeleteCourse(int courseId)
	{
		throw new NotImplementedException();
	}

	public Task UpdateCourseName(int id, string newName)
	{
		throw new NotImplementedException();
	}

	public Task<List<Appointment>> GetAppointmentsForStudent(string studentId)
	{
		throw new NotImplementedException();
	}

	public Task<List<Course>> GetStudentCourses(string studentId)
	{
		throw new NotImplementedException();
	}

	public Task<List<PracticeLog>> GetStudentsPracticeLogsForCourseAndDate(string studentId, int courseId, DateTime startDate)
	{
		throw new NotImplementedException();
	}

	public Task<Course> GetCourse(int id)
	{
		throw new NotImplementedException();
	}

    public Task GenerateCourseInvite(CourseInvite invite)
    {
        throw new NotImplementedException();
    }

    public Task RemoveStudent(int courseId, string studentId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> JoinCourse(string studentId, string code)
    {
        throw new NotImplementedException();
    }

    public Task AddAppointment(Appointment appointment)
    {
        throw new NotImplementedException();
    }

    public Task<List<Recording>> GetStudentCourseRecordings(int courseId, string studentId)
    {
        throw new NotImplementedException();
    }
}

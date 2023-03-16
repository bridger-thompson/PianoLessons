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

    public Task<List<PracticeLog>> GetAllStudentLogsForTeacher(int teacherId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Appointment>> GetAppointmentsForTeacher(int teacherId)
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

    public Task<List<PracticeLog>> GetLogsForStudent(int studentId)
    {
        throw new NotImplementedException();
    }

    public Task<List<PracticeLog>> GetPracticeScores(int courseId, DateTime startDate)
    {
        throw new NotImplementedException();
    }

    public Task<List<PracticeAssignment>> GetStudentAssignments(int studentId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Student>> GetStudentsForTeacher(int teacherId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Course>> GetTeacherCourses(int teacherId)
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
}

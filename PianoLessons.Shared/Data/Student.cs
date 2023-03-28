using System;
using System.Collections.Generic;

namespace PianoLessons.Shared.Data;

public partial class Student
{
    public Student()
    {
        Name = "";
    }
    public string Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; } = new List<Appointment>();

    public virtual ICollection<PaymentHistory> PaymentHistories { get; } = new List<PaymentHistory>();

    public virtual ICollection<PracticeLog> PracticeLogs { get; } = new List<PracticeLog>();

    public virtual ICollection<StudentAssignment> StudentAssignments { get; } = new List<StudentAssignment>();

    public virtual ICollection<StudentCourse> StudentCourses { get; } = new List<StudentCourse>();
    public virtual ICollection<Recording> Recordings { get; } = new List<Recording>();
}

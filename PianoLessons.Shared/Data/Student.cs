using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PianoLessons.Shared.Data;

public partial class Student
{
    public Student()
    {
        Name = "";
    }

    public Student(PianoLessonsUser user)
    {
        Id = user.Id;
        Name = user.Name;
    }

	[JsonPropertyName("id")]
	public string Id { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; } = null!;

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    public virtual ICollection<Appointment> Appointments { get; } = new List<Appointment>();

    public virtual ICollection<PaymentHistory> PaymentHistories { get; } = new List<PaymentHistory>();

    public virtual ICollection<PracticeLog> PracticeLogs { get; } = new List<PracticeLog>();

    public virtual ICollection<StudentCourse> StudentCourses { get; } = new List<StudentCourse>();
    public virtual ICollection<Recording> Recordings { get; } = new List<Recording>();
}

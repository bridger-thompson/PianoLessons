using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PianoLessons.Shared.Data;

public partial class Teacher
{
    public Teacher()
    {
        Name = "";
    }

    public Teacher(PianoLessonsUser user)
    {
        Id= user.Id;
        Name = user.Name;   
    }

	[JsonPropertyName("id")]
	public string Id { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; } = new List<Appointment>();

    public virtual ICollection<Course> Courses { get; } = new List<Course>();

    public virtual ICollection<PaymentHistory> PaymentHistories { get; } = new List<PaymentHistory>();
}

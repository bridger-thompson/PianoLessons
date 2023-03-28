using System;
using System.Collections.Generic;

namespace PianoLessons.Shared.Data;

public partial class Teacher
{
    public Teacher()
    {
        Name = "";
    }
    public string Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; } = new List<Appointment>();

    public virtual ICollection<Course> Courses { get; } = new List<Course>();

    public virtual ICollection<PaymentHistory> PaymentHistories { get; } = new List<PaymentHistory>();
}

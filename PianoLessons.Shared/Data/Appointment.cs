using System;
using System.Collections.Generic;

namespace PianoLessons.Shared.Data;

public partial class Appointment
{
    public int Id { get; set; }

    public string Subject { get; set; } = null!;

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    public string TeacherId { get; set; }

    public string StudentId { get; set; }

    public virtual Student? Student { get; set; } = null!;

    public virtual Teacher? Teacher { get; set; } = null!;
}

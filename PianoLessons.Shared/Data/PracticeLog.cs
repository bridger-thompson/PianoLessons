using System;
using System.Collections.Generic;

namespace PianoLessons.Shared.Data;

public partial class PracticeLog
{
    public PracticeLog()
    {
        //Assignment = new();
        //Student = new();
    }
    public int Id { get; set; }

    public DateTime LogDate { get; set; }

    public int StudentId { get; set; }

    public TimeSpan Duration { get; set; }

    public string? Notes { get; set; }

    public int AssignmentId { get; set; }

    public virtual PracticeAssignment? Assignment { get; set; } = null!;

    public virtual Student? Student { get; set; } = null!;
}

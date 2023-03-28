using System;
using System.Collections.Generic;

namespace PianoLessons.Shared.Data;

public partial class StudentAssignment
{
    public int Id { get; set; }

    public string StudentId { get; set; }

    public int AssignmentId { get; set; }

    public virtual PracticeAssignment Assignment { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}

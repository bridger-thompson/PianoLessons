using System;
using System.Collections.Generic;

namespace PianoLessons.Shared.Data;

public partial class PracticeAssignment
{
    public PracticeAssignment()
    {
        Name = "";
        Course = new();
    }
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CourseId { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<PracticeLog> PracticeLogs { get; } = new List<PracticeLog>();

    public virtual ICollection<StudentAssignment> StudentAssignments { get; } = new List<StudentAssignment>();
}

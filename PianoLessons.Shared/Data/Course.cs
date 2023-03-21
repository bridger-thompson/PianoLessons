using System;
using System.Collections.Generic;

namespace PianoLessons.Shared.Data;

public partial class Course
{
    public Course()
    {
        Name = "";
        Teacher = new();
    }
    
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int TeacherId { get; set; }

    public virtual ICollection<PracticeAssignment>? PracticeAssignments { get; } = null!;

    public virtual ICollection<StudentCourse>? StudentCourses { get; } = null!;

    public virtual Teacher? Teacher { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PianoLessons.Shared.Data;

public partial class Course
{
    public Course()
    {
        Name = "";
        Teacher = new();
    }
    
	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; } = null!;

	[JsonPropertyName("teacherId")]
	public string TeacherId { get; set; }

    public virtual ICollection<StudentCourse>? StudentCourses { get; } = null!;
    public virtual ICollection<PracticeLog>? PracticeLogs { get; } = null!;
	public virtual ICollection<Recording>? Recordings { get; } = null!;

	[JsonPropertyName("teacher")]
	public virtual Teacher? Teacher { get; set; } = null!;
    public virtual ICollection<CourseInvite>? CourseInvites { get; } = null!;
}

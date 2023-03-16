namespace PianoLessons.Shared.Data;

public partial class PracticeLog
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string? Notes { get; set; }

    public int AssignmentId { get; set; }

    public virtual PracticeAssignment? Assignment { get; set; } = null!;

    public virtual Student? Student { get; set; } = null!;

    public TimeSpan Duration => EndTime - StartTime;
}

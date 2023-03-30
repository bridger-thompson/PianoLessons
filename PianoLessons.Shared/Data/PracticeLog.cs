namespace PianoLessons.Shared.Data;

public partial class PracticeLog
{
    public int Id { get; set; }

    public string StudentId { get; set; }

    public int CourseId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string? Notes { get; set; }

    public virtual Student? Student { get; set; } = null!;

    public virtual Course? Course { get; set; } = null!;

	public TimeSpan Duration => EndTime - StartTime;
}

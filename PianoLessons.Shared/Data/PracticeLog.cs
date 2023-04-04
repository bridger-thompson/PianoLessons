using System.Text.Json.Serialization;

namespace PianoLessons.Shared.Data;

public partial class PracticeLog
{
 
	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("studentId")]
	public string StudentId { get; set; }

    [JsonPropertyName("courseId")]
	public int CourseId { get; set; }

	[JsonPropertyName("startTime")]
	public DateTime StartTime { get; set; }

    [JsonPropertyName("endTime")]
	public DateTime EndTime { get; set; }

    [JsonPropertyName("notes")]
	public string? Notes { get; set; }

	[JsonPropertyName("student")]
	public virtual Student? Student { get; set; } = null!;

	[JsonPropertyName("course")]
	public virtual Course? Course { get; set; } = null!;

	public TimeSpan Duration => EndTime - StartTime;
}

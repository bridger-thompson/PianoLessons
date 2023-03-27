using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoLessons.Shared.Data;

public partial class Recording
{
    public int Id { get; set; }

    public string? FilePath { get; set; }

    public DateTime? Created { get; set; }

    public int CourseId { get; set; }

    public virtual Course? Course { get; set; } = null!;

    public int StudentId { get; set; }

    public virtual Student? Student { get; set; } = null!;
}

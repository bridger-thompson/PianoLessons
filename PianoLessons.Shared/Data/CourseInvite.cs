using System;
using System.Collections.Generic;

namespace PianoLessons.Shared.Data;

public partial class CourseInvite
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public string Code { get; set; } = null!;

    public DateTime ExpireDate { get; set; }

    public bool? Used { get; set; }

    public virtual Course Course { get; set; } = null!;
}

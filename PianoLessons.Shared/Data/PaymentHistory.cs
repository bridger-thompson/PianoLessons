using System;
using System.Collections.Generic;

namespace PianoLessons.Shared.Data;

public partial class PaymentHistory
{
    public int Id { get; set; }

    public int TeacherId { get; set; }

    public int StudentId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PayDate { get; set; }

    public virtual Student Student { get; set; } = null!;

    public virtual Teacher Teacher { get; set; } = null!;
}

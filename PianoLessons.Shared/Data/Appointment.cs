using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoLessons.Shared.Data;

public class Appointment
{
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
	public string Subject { get; set; }
}

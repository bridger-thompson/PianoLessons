using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoLessons.Shared.Data;

public class Student
{
	public int Id { get; set; }
	public string Name { get; set; }
	public int? Score { get; set; }
	public int TeacherId { get; set; }
}

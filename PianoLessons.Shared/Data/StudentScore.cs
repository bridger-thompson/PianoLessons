using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PianoLessons.Shared.Data;

public class StudentScore
{
	[JsonPropertyName("id")]
	public string Id { get; set; }

	[JsonPropertyName("rank")]
	public int Rank { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("score")]
	public int Score { get; set; }
}

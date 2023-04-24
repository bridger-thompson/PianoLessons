using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PianoLessons.Shared.Data;

public class PianoLessonsUser
{
    public string Id { get; private set; }
    public string Name { get; private set; }

    public string? Email { get; private set; }
    public bool IsTeacher { get; private set; }
    public bool IsStudent => !IsTeacher;

    public PianoLessonsUser()
    {

    }

    [JsonConstructor]
    public PianoLessonsUser(string id, string name, bool isTeacher, string email)
    {
        Id = id;
        Name = name;    
        IsTeacher = isTeacher;
        Email = email;
    }

    public PianoLessonsUser(Teacher teacher)
    {
        Id = teacher.Id;
        Name = teacher.Name;
        IsTeacher = true;
    }

    public PianoLessonsUser(Student student)
    {
        Id = student.Id;
        Name = student.Name;
        IsTeacher = false;
        Email = student.Email;
    }
}

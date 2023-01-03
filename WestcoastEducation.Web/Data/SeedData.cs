using System.Text.Json;
using WestcoastEducation.Web.Models;

namespace WestcoastEducation.Web.Data;

public static class SeedData
{
    public static async Task LoadClassroomData(WestcoastEducationContext context)
    {
        // "neutraliserar" versaler och gemener för att inte problem ska uppstå vid inläsning
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Vill endast ladda data om databasens tabell är tom
        if (context.Classrooms.Any()) return;

        // Läs in json data 
        var json = System.IO.File.ReadAllText("Data/json/classroom.json");

        // Konvertera json objekten till en lista av Classroom objekt 
        var classrooms = JsonSerializer.Deserialize<List<Classroom>>(json, options);

        // Kontrollerar att classroom inte är null och innehåller data 
        if (classrooms is not null && classrooms.Count > 0)
        {
            await context.Classrooms.AddRangeAsync(classrooms);
            //flytta ifrån minnet till databasen 
            await context.SaveChangesAsync();
        }
    }
    public static async Task LoadStudentData(WestcoastEducationContext context)
    {
        // "neutraliserar" versaler och gemener för att inte problem ska uppstå vid inläsning
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Vill endast ladda data om databasens tabell är tom
        if (context.Students.Any()) return;

        // Läs in json data 
        var json = System.IO.File.ReadAllText("Data/json/student.json");

        // Konvertera json objekten till en lista av Classroom objekt 
        var students = JsonSerializer.Deserialize<List<Student>>(json, options);

        // Kontrollerar att classroom inte är null och innehåller data 
        if (students is not null && students.Count > 0)
        {
            await context.Students.AddRangeAsync(students);
            //flytta ifrån minnet till databasen 
            await context.SaveChangesAsync();
        }
    }
    public static async Task LoadTeacherData(WestcoastEducationContext context)
    {
        // "neutraliserar" versaler och gemener för att inte problem ska uppstå vid inläsning
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Vill endast ladda data om databasens tabell är tom
        if (context.Teachers.Any()) return;

        // Läs in json data 
        var json = System.IO.File.ReadAllText("Data/json/teacher.json");

        // Konvertera json objekten till en lista av Classroom objekt 
        var teachers = JsonSerializer.Deserialize<List<Teacher>>(json, options);

        // Kontrollerar att classroom inte är null och innehåller data 
        if (teachers is not null && teachers.Count > 0)
        {
            await context.Teachers.AddRangeAsync(teachers);
            //flytta ifrån minnet till databasen 
            await context.SaveChangesAsync();
        }
    }
}

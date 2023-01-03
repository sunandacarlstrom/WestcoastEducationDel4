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
}

using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Web.Models;

public class ClassroomModel
{
    [Key]
    public int ClassroomId { get; set; }
    public string Number { get; set; } = "";
    public string Name { get; set; } = "";
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public double AvgGrade { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public TimeSpan Length { get => End - Start; }
    public string Schedule { get; set; } = "";
    public bool IsOnDistance { get; init; }
    public int TeacherId { get; set; }
}
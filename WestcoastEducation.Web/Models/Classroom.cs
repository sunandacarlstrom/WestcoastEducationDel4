namespace WestcoastEducation.Web.Models;

public class Classroom
{
    public Guid CourseId { get; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string Content { get; set; } = "";
    public double AvgGrade { get; set; }
    public DateTime Start { get; set; } = DateTime.Now;
    public DateTime End { get; set; } = DateTime.Now.AddDays(40);
    public string Schedule { get; set; } = "";
    public bool IsOnDistance { get; init; }
    public Guid TeacherId { get; set; }

    public Classroom(Guid teacherId, bool isOnDistance)
    {
        TeacherId = teacherId;
        IsOnDistance = isOnDistance;
    }
}
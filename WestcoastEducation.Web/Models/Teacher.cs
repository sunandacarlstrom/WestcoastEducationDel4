namespace WestcoastEducation.Web.Models;

public class Teacher : User
{
    public int TeacherId { get => UserId; set => UserId = value; }
    public List<Classroom> _ongoingCourses = new List<Classroom>();
}
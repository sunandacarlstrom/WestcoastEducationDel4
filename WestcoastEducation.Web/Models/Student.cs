namespace WestcoastEducation.Web.Models;

public class Student : User
{
    public int StudentId { get => UserId; set => UserId = value; }
    public List<Classroom> _acceptedCourses = new List<Classroom>();
    public List<Classroom> _ongoingCourses = new List<Classroom>();
    public List<Classroom> _finishedCourses = new List<Classroom>();
}
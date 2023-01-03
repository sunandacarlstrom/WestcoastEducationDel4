namespace WestcoastEducation.Web.Models;

public class Student : User
{
    public List<Classroom> _acceptedCourses = new List<Classroom>();
    public List<Classroom> _ongoingCourses = new List<Classroom>();
    public List<Classroom> _finishedCourses = new List<Classroom>();

    public Student(string username, string password) : base(username, password)
    {
    }
}
namespace WestcoastEducation.Web.Models;

public class Teacher : User
{
    public List<Classroom> _ongoingCourses = new List<Classroom>();

    public Teacher(string username, string password) : base(username, password)
    {
    }
}
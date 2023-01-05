using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Web.Models;

public class Teacher : User
{
    public List<Classroom> _ongoingCourses = new List<Classroom>();
}
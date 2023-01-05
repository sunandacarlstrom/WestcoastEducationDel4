using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Web.Models;

public class Teacher : User
{
    public List<User> _ongoingCourses = new List<User>();
}
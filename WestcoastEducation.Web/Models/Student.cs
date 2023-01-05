using System.ComponentModel.DataAnnotations;

namespace WestcoastEducation.Web.Models;

public class Student : User
{
    public List<User> _acceptedCourses = new List<User>();
    public List<User> _ongoingCourses = new List<User>();
    public List<User> _finishedCourses = new List<User>();
}
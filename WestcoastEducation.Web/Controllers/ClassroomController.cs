using Microsoft.AspNetCore.Mvc;
using WestcoastEducation.Web.Models;

namespace WestcoastEducation.Web.Controllers;

[Route("classroom")]
public class ClassroomController : Controller
{
    public IActionResult Index()
    {
        var classroom = new List<Classroom>{
            new Classroom() { Name = "Webbapplikation MVP", Content = "Grunderna i HTML, CSS och JavaScript"},
            new Classroom() { Name = "Objektorienterad programmering med C#", Content = "Grunderna i OOP och C#"},
            new Classroom() { Name = "Dynamiska Webbsystem 1", Content = "Grunderna i MVC med TDD"}
            };

        return View("Index", classroom);
    }

    [Route("details/{courseId}")]
    public IActionResult Details(int courseId)
    {
        var foundClassroom = new Classroom()
        {
            Name = "Webbapplikation MVP",
            Content = "Grunderna i HTML, CSS och JavaScript"
        };

        return View("Details", foundClassroom);
    }
}

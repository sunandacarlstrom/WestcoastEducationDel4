using Microsoft.AspNetCore.Mvc;
using WestcoastEducation.Domain;

namespace WestcoastEducation.Web.Controllers;

public class ClassroomController : Controller
{
    [Route("classroom")]
    public IActionResult Index()
    {
        // skapar en ny lista av typen Classroom
        var classroomCourse = new List<Classroom>{
            new Classroom(Guid.NewGuid(), false) { Name = "Webbapplikation MVP"},
            new Classroom(Guid.NewGuid(), false) { Name = "Objektorienterad programmering med C#"},
            new Classroom(Guid.NewGuid(), false) { Name = "Dynamiska Webbsystem 1"}
            };

        return View("Index", classroomCourse);
    }
}

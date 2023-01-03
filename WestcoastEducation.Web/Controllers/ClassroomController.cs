using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Web.Data;

namespace WestcoastEducation.Web.Controllers;

[Route("classroom")]
public class ClassroomController : Controller
{
    private readonly WestcoastEducationContext _context;
    public ClassroomController(WestcoastEducationContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var classrooms = await _context.Classrooms.ToListAsync();
        return View("Index", classrooms);
    }

    [Route("details/{courseId}")]
    public IActionResult Details(int courseId)
    {
        return View("Details");
    }
}

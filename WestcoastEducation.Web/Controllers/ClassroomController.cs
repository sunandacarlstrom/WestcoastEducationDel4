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

    [Route("details/{classroomId}")]
    public async Task<IActionResult> Details(int classroomId)
    {
        // hämtar kurserna(klassrum) från databasen och lägger endast den kursen som har det ID som användaren har klickat på i variabeln classroom
        var classroom = await _context.Classrooms.SingleOrDefaultAsync(c => c.ClassroomId == classroomId);

        //kontrollerar att kursen(klassrummet) existerar
        if (classroom is null) return View(nameof(Index));

        // skicka kursen(klassrummet) till vyn
        return View("Details", classroom);
    }
}

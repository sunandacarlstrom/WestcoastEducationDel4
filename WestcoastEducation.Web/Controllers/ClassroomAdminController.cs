using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Web.Data;
using WestcoastEducation.Web.Models;

namespace WestcoastEducation.Web.Controllers
{
    [Route("classroomadmin")]
    public class ClassroomAdminController : Controller
    {
        private readonly WestcoastEducationContext _context;
        public ClassroomAdminController(WestcoastEducationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Använder context för att koppla vyn till databasen
            var classrooms = await _context.Classrooms.ToListAsync();
            return View("Index", classrooms);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            // skapa ett nytt objekt för att skicka över till vyn
            var classroom = new Classroom();
            return View("Create", classroom);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(Classroom classroom)
        {
            // lägg upp en kurs i minnet
            await _context.Classrooms.AddAsync(classroom);
            // spara ner i databas
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
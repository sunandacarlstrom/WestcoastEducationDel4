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

        [HttpGet("edit/{classroomId}")]
        public async Task<IActionResult> Edit(int classroomId)
        {
            // får tillbaka en kurs och skicka till en vy
            // här vill jag alltså få tag i en kurs med Id som är lika med det som kommer in mitt metodanrop
            var classroom = await _context.Classrooms.SingleOrDefaultAsync(c => c.ClassroomId == classroomId);

            if (classroom is not null) return View("Edit", classroom);
            return Content("Det gick fel...");
        }

        [HttpPost("edit/{classroomId}")]
        public async Task<IActionResult> Edit(int classroomId, Classroom classroom)
        {
            // vara säker på att kursen jag redigerar/uppdaterar verkligen finns i Changetracking listan
            var classroomToUpdate = _context.Classrooms.SingleOrDefault(c => c.ClassroomId == classroomId);

            if (classroomToUpdate is null) return RedirectToAction(nameof(Index));

            classroomToUpdate.Name = classroom.Name;
            classroomToUpdate.Title = classroom.Title;
            classroomToUpdate.Start = classroom.Start;
            classroomToUpdate.End = classroom.End;

            //uppdatera en kurs via ef 
            _context.Classrooms.Update(classroomToUpdate);

            // spara ner i databas (alla ändringar på en o samma gång med _context)
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Route("delete/{classroomId}")]
        public async Task<IActionResult> Delete(int classroomId)
        {
            //hämta in kursen som jag vill radera 
            var classroomToDelete = await _context.Classrooms.SingleOrDefaultAsync(c => c.ClassroomId == classroomId);

            if (classroomToDelete is null) return RedirectToAction(nameof(Index));

            //radera en kurs direkt 
            _context.Classrooms.Remove(classroomToDelete);

            // spara ner i databas (alla ändringar på en o samma gång med _context)
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
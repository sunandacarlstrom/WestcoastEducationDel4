using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Web.Data;

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
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Web.Data;
using WestcoastEducation.Web.Models;

namespace WestcoastEducation.Web.Controllers;

[Route("useradmin")]
public class UserAdminController : Controller
{
    private readonly WestcoastEducationContext _context;
    public UserAdminController(WestcoastEducationContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var students = await _context.Students.ToListAsync();
        var teachers = await _context.Teachers.ToListAsync();

        List<User> users = new List<User>();
        users.AddRange(teachers);
        users.AddRange(students);

        return View("Index", users);
    }

    [Route("details/{userId}")]
    public IActionResult Details(int userId)
    {
        return View("Details");
    }
}

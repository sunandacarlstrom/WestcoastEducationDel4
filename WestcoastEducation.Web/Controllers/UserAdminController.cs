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

    private async Task<List<User>> GetUsers()
    {
        // hämtar användarna(lärare och elever) från databasen och lägger endast den kursen som har det ID som användaren har klickat på i variabeln classroom
        var students = await _context.Students.ToListAsync();
        var teachers = await _context.Teachers.ToListAsync();

        List<User> users = new List<User>();
        users.AddRange(teachers);
        users.AddRange(students);
        return users;
    }

    public async Task<IActionResult> Index()
    {
        List<User> users = await GetUsers();
        return View("Index", users);
    }

    [Route("details/{userId}")]
    public async Task<IActionResult> Details(int userId)
    {
        List<User> users = await GetUsers();
        User? user = users.FirstOrDefault(u => u.UserId == userId);

        //kontrollerar att användaren existerar
        if (user is null) return View(nameof(Index));

        // skicka användaren till vyn
        return View("Details", user);
    }

    [HttpGet("create-teacher")]
    public IActionResult CreateTeacher()
    {
        // skapa ett nytt objekt för att skicka över till vyn
        var teacher = new Teacher();
        return View("CreateTeacher", teacher);
    }

    [HttpPost("create-teacher")]
    public async Task<IActionResult> CreateTeacher(Teacher teacher)
    {
        try
        {
            // söker efter personnummer lika med det som kommer in i anropet
            var exists = await _context.Teachers.SingleOrDefaultAsync(
                t => t.SocialSecurityNumber.Trim().ToLower() == teacher.SocialSecurityNumber.Trim().ToLower());

            // kontrollerar om det inmatade personnumret redan existerar
            if (exists is not null)
            {
                var error = new ErrorModel
                {
                    ErrorTitle = "Ett fel har inträffat när användaren (läraren) skulle sparas",
                    ErrorMessage = $"En lärare med samma personnummer som {teacher.SocialSecurityNumber} finns redan i systemet"
                };

                //skicka tillbaka en vy som visar information gällande felet 
                return View("_Error", error);
            }
        }
        // Ett annat fel har inträffat som vi inte har räknat med...
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när användaren (läraren) skulle sparas",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }

        // Om allt går bra, inga fel inträffar...

        // lägg upp kursen i minnet
        await _context.Teachers.AddAsync(teacher);
        // spara ner i databas
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("create-student")]
    public IActionResult CreateStudent()
    {
        // skapa ett nytt objekt för att skicka över till vyn
        var student = new Student();
        return View("CreateStudent", student);
    }

    [HttpPost("create-student")]
    public async Task<IActionResult> CreateStudent(Student student)
    {
        try
        {
            // söker efter en kurstitel lika med det som kommer in i anropet 
            var exists = await _context.Students.SingleOrDefaultAsync(
                s => s.SocialSecurityNumber.Trim().ToLower() == student.SocialSecurityNumber.Trim().ToLower());

            // kontrollerar om det inmatade personnumret redan existerar
            if (exists is not null)
            {
                var error = new ErrorModel
                {
                    ErrorTitle = "Ett fel har inträffat när användaren (studenten) skulle sparas",
                    ErrorMessage = $"En student med samma personnummer som {student.SocialSecurityNumber} finns redan i systemet"
                };

                //skicka tillbaka en vy som visar information gällande felet 
                return View("_Error", error);
            }
        }
        // Ett annat fel har inträffat som vi inte har räknat med...
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när användaren (studenten) skulle sparas",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }

        // Om allt går bra, inga fel inträffar...

        // lägg upp kursen i minnet
        await _context.Students.AddAsync(student);
        // spara ner i databas
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{userId}")]
    public async Task<IActionResult> Edit(int userId)
    {
        try
        {
            List<User> users = await GetUsers();
            User? user = users.FirstOrDefault(u => u.UserId == userId);

            if (user is not null) return View("Edit", user);

            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när en användare skulle hämtas för redigering",
                ErrorMessage = $"Hittar ingen användare med id {userId}"
            };

            return View("_Error", error);
        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när en användare skulle hämtas för redigering",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }

    [HttpPost("edit/{userId}")]
    public async Task<IActionResult> Edit(int userId, User user)
    {
        try
        {
            List<User> users = await GetUsers();
            // vara säker på att användaren jag redigerar/uppdaterar verkligen finns i Changetracking listan
            User? userToUpdate = users.FirstOrDefault(u => u.UserId == userId);

            if (userToUpdate is null) return RedirectToAction(nameof(Index));

            userToUpdate.Email = user.Email;
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.SocialSecurityNumber = user.SocialSecurityNumber;
            userToUpdate.StreetAddress = user.StreetAddress;
            userToUpdate.PostalCode = user.PostalCode;
            userToUpdate.Phone = user.Phone;

            //uppdatera en kurs via ef 
            if (userToUpdate.GetType() == typeof(Teacher))
            {
                _context.Teachers.Update(userToUpdate as Teacher);
            }
            else
            {
                _context.Students.Update(userToUpdate as Student);
            }

            // spara ner i databas (alla ändringar på en o samma gång med _context)
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när redigering av kursen skulle sparas",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }

    [Route("delete/{userId}")]
    public async Task<IActionResult> Delete(int userId)
    {
        try
        {
            List<User> users = await GetUsers();
            User? userToDelete = users.FirstOrDefault(c => c.UserId == userId);

            if (userToDelete is null) return RedirectToAction(nameof(Index));

            //radera en användare direkt 
            if (userToDelete.GetType() == typeof(Teacher))
            {
                _context.Teachers.Remove(userToDelete as Teacher);
            }
            else
            {
                _context.Students.Remove(userToDelete as Student);
            }

            // spara ner i databas (alla ändringar på en o samma gång med _context)
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när användaren skulle raderas",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }
}

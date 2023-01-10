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
        List<UserModel> users = await _context.Users.ToListAsync();
        return View("Index", users);
    }

    [Route("details/{userId}")]
    public async Task<IActionResult> Details(int userId)
    {
        List<UserModel> users = await _context.Users.ToListAsync();
        UserModel? user = users.SingleOrDefault(u => u.UserId == userId);

        //kontrollerar att användaren existerar
        if (user is null) return View(nameof(Index));

        // skicka användaren till vyn
        return View("Details", user);
    }

    [HttpGet("create/{isATeacher}")]
    public IActionResult Create(bool isATeacher)
    {
        // skapa ett nytt objekt för att skicka över till vyn
        var user = new UserModel();
        // stoppar in boolean värdet in i modellen User för att använda i vyn 
        user.IsATeacher = isATeacher;
        return View("Create", user);
    }

    [HttpPost("create/{isATeacher}")]
    public async Task<IActionResult> Create(UserModel user)
    {
        try
        {
            // söker efter personnummer lika med det som kommer in i anropet
            var exists = await _context.Users.SingleOrDefaultAsync(
                u => u.SocialSecurityNumber.Trim().ToLower() == user.SocialSecurityNumber.Trim().ToLower());

            // kontrollerar om det inmatade personnumret redan existerar
            if (exists is not null)
            {
                var error = new ErrorModel
                {
                    ErrorTitle = "Ett fel har inträffat när användaren skulle sparas",
                    ErrorMessage = $"En användare med samma personnummer som {user.SocialSecurityNumber} finns redan i systemet"
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
                ErrorTitle = "Ett fel har inträffat när användaren skulle sparas",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }

        // Om allt går bra, inga fel inträffar...

        // lägg upp kursen i minnet
        await _context.Users.AddAsync(user);
        // spara ner i databas
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{userId}")]
    public async Task<IActionResult> Edit(int userId)
    {
        try
        {
            List<UserModel> users = await _context.Users.ToListAsync();
            UserModel? user = users.SingleOrDefault(u => u.UserId == userId);

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
    public async Task<IActionResult> Edit(int userId, UserModel user)
    {
        // samma funktion som i Create 
        try
        {
            // söker efter personnummer lika med det som kommer in i anropet
            var exists = await _context.Users.SingleOrDefaultAsync(
                u => u.SocialSecurityNumber.Trim().ToLower() == user.SocialSecurityNumber.Trim().ToLower());

            // kontrollerar om det inmatade personnumret redan existerar
            if (exists is not null)
            {
                var error = new ErrorModel
                {
                    ErrorTitle = "Ett fel har inträffat när användaren skulle sparas",
                    ErrorMessage = $"En användare med samma personnummer som {user.SocialSecurityNumber} finns redan i systemet"
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
                ErrorTitle = "Ett fel har inträffat när användaren skulle sparas",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }

        try
        {
            List<UserModel> users = await _context.Users.ToListAsync();
            // vara säker på att användaren jag redigerar/uppdaterar verkligen finns i Changetracking listan
            UserModel? userToUpdate = users.SingleOrDefault(u => u.UserId == userId);

            if (userToUpdate is null) return RedirectToAction(nameof(Index));

            userToUpdate.Email = user.Email;
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.SocialSecurityNumber = user.SocialSecurityNumber;
            userToUpdate.StreetAddress = user.StreetAddress;
            userToUpdate.PostalCode = user.PostalCode;
            userToUpdate.Phone = user.Phone;
            userToUpdate.IsATeacher = user.IsATeacher;

            //uppdatera en användare via ef 
            _context.Users.Update(userToUpdate);


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
            List<UserModel> users = await _context.Users.ToListAsync();
            UserModel? userToDelete = users.SingleOrDefault(u => u.UserId == userId);

            if (userToDelete is null) return RedirectToAction(nameof(Index));

            //radera en användare direkt 
            _context.Users.Remove(userToDelete);



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

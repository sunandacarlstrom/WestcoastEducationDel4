using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Web.Interfaces;
using WestcoastEducation.Web.Models;
using WestcoastEducation.Web.ViewModels.Users;

namespace WestcoastEducation.Web.Controllers;

[Route("useradmin")]
public class UserAdminController : Controller
{
    private readonly IUserRepository _repo;
    public UserAdminController(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _repo.ListAllAsync();
        var users = result.Select(u => new UserListViewModel
        {
            UserId = u.UserId,
            //TODO: Lägg till UserName = user.UserName 
            Email = u.Email,
            FirstName = u.FirstName,
            LastName = u.LastName,
            SocialSecurityNumber = u.SocialSecurityNumber,
            StreetAddress = u.StreetAddress,
            PostalCode = u.PostalCode,
            Phone = u.Phone,
            IsATeacher = u.IsATeacher
            //TODO: Lägg till Password = user.Password 
        }).ToList();

        return View("Index", users);
    }

    [Route("details/{userId}")]
    public async Task<IActionResult> Details(int userId)
    {
        // List<UserModel> users = await _context.Users.ToListAsync();
        // UserModel? user = users.SingleOrDefault(u => u.UserId == userId);

        //TODO: Kontrollera att det funkar
        var user = await _repo.FindByIdAsync(userId);

        //kontrollerar att användaren existerar
        if (user is null) return View(nameof(Index));

        // skicka användaren till vyn
        return View("Details", user);
    }

    [HttpGet("create/{isATeacher}")]
    public IActionResult Create(bool isATeacher)
    {
        // skapa ett nytt objekt för att skicka över till vyn
        var user = new UserPostViewModel();
        // stoppar in boolean värdet in i modellen User för att använda i vyn 
        user.IsATeacher = isATeacher;
        return View("Create", user);
    }

    [HttpPost("create/{isATeacher}")]
    public async Task<IActionResult> Create(UserPostViewModel user)
    {
        {
            // tittar på om det som kommer in stämmer överrens med kraven i UserPostViewModel
            if (!ModelState.IsValid) return View("Create", user);

            // söker efter personnummer lika med det som kommer in i anropet
            // var exists = await _context.Users.SingleOrDefaultAsync(
            //     u => u.SocialSecurityNumber.Trim().ToLower() == user.SocialSecurityNumber.Trim().ToLower());

            // skapar ett felmeddelande ifall E-postadressen redan finns
            if (await _repo.FindByEmailAsync(user.Email) is not null)
            {
                var error = new ErrorModel
                {
                    ErrorTitle = "Användaren finns redan",
                    ErrorMessage = $"Användare med e-postadressen {user.Email} finns redan i systemet"
                };

                //skicka tillbaka en vy som visar information gällande felet 
                return View("_Error", error);
            }

            //UserModel (datamodell) är det jag kan skicka till databasen
            var userToAdd = new UserModel
            {
                //TODO: Lägg till UserName = user.UserName 
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                SocialSecurityNumber = user.SocialSecurityNumber,
                StreetAddress = user.StreetAddress,
                PostalCode = user.PostalCode,
                Phone = user.Phone,
                IsATeacher = user.IsATeacher
                //TODO: Lägg till Password = user.Password 
            };

            // Om allt går bra, inga fel inträffar...

            // lägg upp användaren i minnet
            if (await _repo.AddAsync(userToAdd))
            {
                //spara ner det i databasen
                if (await _repo.SaveAsync())
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            // annars inträffar ett fel som vi inte har räknat med
            return View("Error", new ErrorModel
            {
                ErrorTitle = "Gick inte att spara användare",
                ErrorMessage = $"Ett fel har inträffat när användare {user.CompleteName} skulle sparas"
            });
        }
    }

    // [HttpGet("edit/{userId}")]
    // public async Task<IActionResult> Edit(int userId)
    // {
    //     try
    //     {
    //         List<UserModel> users = await _context.Users.ToListAsync();
    //         UserModel? user = users.SingleOrDefault(u => u.UserId == userId);

    //         if (user is not null) return View("Edit", user);

    //         var error = new ErrorModel
    //         {
    //             ErrorTitle = "Ett fel har inträffat när en användare skulle hämtas för redigering",
    //             ErrorMessage = $"Hittar ingen användare med id {userId}"
    //         };

    //         return View("_Error", error);
    //     }
    //     catch (Exception ex)
    //     {
    //         var error = new ErrorModel
    //         {
    //             ErrorTitle = "Ett fel har inträffat när en användare skulle hämtas för redigering",
    //             ErrorMessage = ex.Message
    //         };

    //         return View("_Error", error);
    //     }
    // }

    // [HttpPost("edit/{userId}")]
    // public async Task<IActionResult> Edit(int userId, UserModel user)
    // {
    //     try
    //     {
    //         List<UserModel> users = await _context.Users.ToListAsync();
    //         // vara säker på att användaren jag redigerar/uppdaterar verkligen finns i Changetracking listan
    //         UserModel? userToUpdate = users.SingleOrDefault(u => u.UserId == userId);

    //         if (userToUpdate is null) return RedirectToAction(nameof(Index));

    //         userToUpdate.Email = user.Email;
    //         userToUpdate.FirstName = user.FirstName;
    //         userToUpdate.LastName = user.LastName;
    //         userToUpdate.SocialSecurityNumber = user.SocialSecurityNumber;
    //         userToUpdate.StreetAddress = user.StreetAddress;
    //         userToUpdate.PostalCode = user.PostalCode;
    //         userToUpdate.Phone = user.Phone;
    //         userToUpdate.IsATeacher = user.IsATeacher;

    //         //uppdatera en användare via ef 
    //         _context.Users.Update(userToUpdate);


    //         // spara ner i databas (alla ändringar på en o samma gång med _context)
    //         await _context.SaveChangesAsync();

    //         return RedirectToAction(nameof(Index));

    //     }
    //     catch (Exception ex)
    //     {
    //         var error = new ErrorModel
    //         {
    //             ErrorTitle = "Ett fel har inträffat när redigering av kursen skulle sparas",
    //             ErrorMessage = ex.Message
    //         };

    //         return View("_Error", error);
    //     }
    // }

    // [Route("delete/{userId}")]
    // public async Task<IActionResult> Delete(int userId)
    // {
    //     try
    //     {
    //         List<UserModel> users = await _context.Users.ToListAsync();
    //         UserModel? userToDelete = users.SingleOrDefault(u => u.UserId == userId);

    //         if (userToDelete is null) return RedirectToAction(nameof(Index));

    //         //radera en användare direkt 
    //         _context.Users.Remove(userToDelete);



    //         // spara ner i databas (alla ändringar på en o samma gång med _context)
    //         await _context.SaveChangesAsync();

    //         return RedirectToAction(nameof(Index));
    //     }
    //     catch (Exception ex)
    //     {
    //         var error = new ErrorModel
    //         {
    //             ErrorTitle = "Ett fel har inträffat när användaren skulle raderas",
    //             ErrorMessage = ex.Message
    //         };

    //         return View("_Error", error);
    //     }
    // }
}

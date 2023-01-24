using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WestcoastEducation.Web.Models;
using WestcoastEducation.Web.ViewModels.Account;

namespace WestcoastEducation.Web.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    // skapar en constructor för att göra det möjligt att hantera användare genom att lägga till två klasser från Identity biblioteket 
    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        var model = new LoginViewModel();
        return View("Login", model);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = new IdentityUser
        {
            // UserName ska vara det användarnamn som kommer in från formuläret 
            UserName = model.UserName
        };

        // kapsla allting i try-catch för att fånga fel som kan uppstå 
        try
        {
            // Vill ej att Cookies ska övereleva när man stänger ner wenbbläsaren - svar nej (false)
            // Vill ej låsa ute användaren vid x antal om hen misslyckas med inloggningen - svar nej (false)
            var result = await _signInManager.PasswordSignInAsync(model.UserName!, model.Password!, false, false);


            // Om det går bra... 
            // retunerar till rätt vy 
            if (result.Succeeded)
            {
                return RedirectToRoute(new { controller = "classroom", Action = "Index" });
            }
            // Om det ej går bra.. 
            // kan beror på att det inte rä tillåtet, något fel 

            if (result.IsNotAllowed)
            {
                ModelState.AddModelError("Login", "Gick inte att logga in");
            }
            // eller kan beror på att kontot är fel 
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("Login", "Kontot är låst");
            }

            // P.S. detta testar sådant som är fördefinerat och därför måste vi skapa en ny ModelState
            // får ej en lista med alla fel
            ModelState.AddModelError("Login", "Något gick fel! Kontrollera användarnamn och lösenord");
            // all data retuneras till samma utgångspunkt 
            return View("Login", model);
        }
        catch (Exception ex)
        {
            // TODO: ändra till ErrorViewModel samt skapa en ErrorViewModel i ViewModels
            var errorModel = new ErrorModel
            {
                ErrorTitle = "Inloggningen gick fel",
                ErrorMessage = ex.Message
            };

            return View("_Error", errorModel);
        }
    }

    // Anrop för att kunna skapa en användare 
    [HttpGet("register")]
    // P.S. denna metod är synkront eftersom den ej pratar med databasen, utan levererar endast ett formulär
    public IActionResult Register()
    {
        // skapar en ny modell för att kunna registrera användaren
        var registerModel = new RegisterUserViewModel();
        // anropar vyn Register och skickar över modellen 
        return View("Register", registerModel);
    }

    // Anrop för att kunna spara ner en ny användare
    [HttpPost("register")]
    // P.S. denna metod kommunicerar asynkront med Identity biblioteket för att kunna spara ner användaren asynkront till databasen 
    public async Task<IActionResult> Register(RegisterUserViewModel model)
    {
        // titta så att modelstate inte innehåller några fel, annars retuneras vyn igen och jag skickar med modellen där felmeddelanden finns
        if (!ModelState.IsValid) return View("Register", model);

        // Om det inte finns några fel...
        // skapar jag en ny instans av klassen IdentityUser från Identity biblioteket
        var user = new IdentityUser
        {
            // får in UserName genom att sätta den som Email då jag kommer använda Email som användarnamn 
            // även Email placeras i databasen 
            UserName = model.Email,
            Email = model.Email
        };

        // anropar UserManager som är asynkront 
        // P.S. sätter utropstecken efter Password eftersom det inte finns någon risk att det är null eftersom jag redan har satt validering med attribut i RegisterViewModel
        var result = await _userManager.CreateAsync(user, model.Password!);

        // Om allt gick bra... 
        // då vill jag skicka användaren till vyn listningen av kurser 
        if (result.Succeeded)
        {
            // RedirectToRoute används för att skicka till en actionmetod som finns i en annan Controller än denna 
            return RedirectToRoute(new { controller = "Classroom", action = "Index" });
        }
        else
        {
            // Om det går fel...
            // skapar en lista med alla felmeddelanden som har uppstått 
            foreach (var error in result.Errors)
            {
                // Använder AddModelError för att lägga till egna felmeddelanden i ModelState
                ModelState.AddModelError("Register", error.Description);
            }
        }

        // när jag har loopat igenom alla fel...
        // då kan jag retunerar Register som är fylld med felmeddelanden
        return View("Register", model);
    }
    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        // vad ska hända när jag trycker på knappen logga ut
        return RedirectToRoute(new { controller = "Home", action = "Index" });
    }
}
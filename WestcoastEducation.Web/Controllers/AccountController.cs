using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WestcoastEducation.Web.Models;
using WestcoastEducation.Web.ViewModels.Account;
using WestcoastEducation.Web.ViewModels.Account.Admin;

namespace WestcoastEducation.Web.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    // skapar en constructor för att göra det möjligt att hantera användare genom att lägga till två klasser från Identity biblioteket 
    public AccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _signInManager = signInManager;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpGet("{returnUrl}")]
    // Denna metod förväntar sig att vi tar in login? = frågesträng = [FromQuery] men också string returnUrl
    public IActionResult Login([FromQuery] string returnUrl)
    {
        // om returnUrl är null då dirigeras användaren till classroom-sidan (standard URL:en)
        if (returnUrl is null) returnUrl = "/classroom";
        // som ta med returnUrl i min ViewBag
        ViewBag.returnUrl = returnUrl;

        var model = new LoginViewModel();
        return View("Login", model);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
    {
        if (!ModelState.IsValid) return View(model);

        var user = new IdentityUser
        {
            // UserName ska vara det användarnamn som kommer in från formuläret 
            UserName = model.UserName
        };

        // Viktigt att kapsla in med try-catch för att fånga fel som kan uppstå 
        try
        {
            // Vill ej att Cookies ska övereleva när man stänger ner wenbbläsaren - svar nej (false)
            // Vill ej låsa ute användaren vid x antal om hen misslyckas med inloggningen - svar nej (false)
            var result = await _signInManager.PasswordSignInAsync(model.UserName!, model.Password!, false, false);


            // Om det går bra... 
            // retunerar till rätt vy 
            if (result.Succeeded)
            {
                // return RedirectToRoute(new { controller = "classroom", Action = "Index" });
                // Istället för att skicka användaren till en fördefinerad sida så vill jag dynamiskt kunna skicka användaren till den sida som hen vill komma åt
                return Redirect(returnUrl);
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
    // Denna metod tar emot det inmatade formuläret och sparar ner detta (användaren) asynkront till databasen 
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

    [HttpGet("admin/roles")]
    public IActionResult CreateRole()
    {
        // skapa en ny model som är min nya RoleViewModel
        var model = new RoleViewModel();

        // retunerar en ny vy
        return View("CreateRole", model);
    }

    [HttpPost("admin/roles")]
    public async Task<IActionResult> CreateRole(RoleViewModel model)
    {
        if (!ModelState.IsValid) return View("CreateRole", model);

        // Här kan man skicka in namnet på rollen direkt
        var role = new IdentityRole(model.RoleName!);

        // anrop för att lägga till min nya roll
        var result = await _roleManager.CreateAsync(role);

        // Om allt går bra...
        // ska man kunna fortsätta mata in rollerna en efter en 
        if (result.Succeeded) RedirectToAction(nameof(CreateRole));

        // Om det inte går bra... 
        // listas alla felmeddelanden 
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("CreateRole", error.Description);
        }

        // retunerar en ny vy 
        return View("CreateRole", model);
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout()
    {
        // tala om för SignInManager att göra en SignOutAsync (logga ut)
        await _signInManager.SignOutAsync();

        // vad ska hända när jag trycker på knappen logga ut...
        // användaren ska då dirigeras om till Home-controller till metoden Index
        return RedirectToRoute(new { controller = "Home", action = "Index" });
    }
}
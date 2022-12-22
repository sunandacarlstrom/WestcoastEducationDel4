using Microsoft.AspNetCore.Mvc;

namespace WestcoastEducation.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewBag.Message = "Vi har kurserna för din framtid 😄";
        return View("Index");
    }
}

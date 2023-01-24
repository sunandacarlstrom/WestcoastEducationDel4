using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WestcoastEducation.Web.Controllers;

[Route("apply")]
public class ApplyController : Controller
{
    [Route("apply")]
    [Authorize()]
    public IActionResult Index()
    {
        return View("Index");
    }
}

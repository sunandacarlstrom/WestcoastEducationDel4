using Microsoft.AspNetCore.Mvc;

namespace WestcoastEducation.Web.Controllers
{
    [Route("[controller]")]
    public class ClassroomAdminController : Controller
    {
        private readonly ILogger<ClassroomAdminController> _logger;

        public ClassroomAdminController(ILogger<ClassroomAdminController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
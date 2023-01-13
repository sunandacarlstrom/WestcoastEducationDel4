using Microsoft.AspNetCore.Mvc;
using WestcoastEducation.Web.Interfaces;
using WestcoastEducation.Web.Models;
using WestcoastEducation.Web.ViewModels.Classrooms;

namespace WestcoastEducation.Web.Controllers;

[Route("classroom")]
public class ClassroomController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public ClassroomController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            // går direkt till UnitOfWork och hittar rätt metod 
            var classrooms = await _unitOfWork.ClassroomRepository.ListAllAsync();

            // här görs en projicering med hjälp av LINQ, dvs. jag vill ta all data som finns i ClassroomModel och gör ett nytt objekt
            // för varje kurs i den listan kommer det ske en intern loop och skapar ett nytt ClassroomListViewModel
            var model = classrooms.Select(c => new ClassroomListViewModel
            {
                ClassroomId = c.ClassroomId,
                Number = c.Number,
                Name = c.Name,
                Title = c.Title,
                Content = c.Content,
                Start = c.Start,
                End = c.End,
                IsOnDistance = c.IsOnDistance
                // typar om min projicering till en IList eftersom min model per automatik vill ta emot en IEnumerable
            }).ToList();

            return View("Index", model);
        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat vid inhämtning av kurser",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }

    [Route("details/{classroomId}")]
    public async Task<IActionResult> Details(int classroomId)
    {
        try
        {
            // går direkt till UnitOfWork och hittar rätt metod 
            var result = await _unitOfWork.ClassroomRepository.FindByIdAsync(classroomId);

            // här görs en projicering med hjälp av LINQ, dvs. jag vill ta all data som finns i ClassroomModel och gör ett nytt objekt
            // för varje kurs i den listan kommer det ske en intern loop och skapar ett nytt ClassroomListViewModel
            var model = new ClassroomDetailsViewModel
            {
                ClassroomId = result.ClassroomId,
                Number = result.Number,
                Name = result.Name,
                Title = result.Title,
                Content = result.Content,
                Start = result.Start,
                End = result.End,
                IsOnDistance = result.IsOnDistance
            };

            return View("Details", model);
        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }
}

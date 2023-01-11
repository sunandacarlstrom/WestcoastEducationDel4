using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Web.Data;
using WestcoastEducation.Web.Models;
using WestcoastEducation.Web.ViewModels;

namespace WestcoastEducation.Web.Controllers;

[Route("classroomadmin")]
public class ClassroomAdminController : Controller
{
    private readonly WestcoastEducationContext _context;
    public ClassroomAdminController(WestcoastEducationContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            // använder context för att koppla vyn till databasen
            var classrooms = await _context.Classrooms.ToListAsync();

            // här görs en projicering med hjälp av LINQ, dvs. jag vill ta all data som finns i ClassroomModel och gör ett nytt objekt
            // för varje kurs i den listan kommer det ske en intern loop och skapar ett nytt ClassroomListViewModel
            var model = classrooms.Select(c => new ClassroomListViewModel
            {
                ClassroomId = c.ClassroomId,
                Number = c.Number,
                Name = c.Name,
                Title = c.Title,
                Content = c.Content,
                AvgGrade = c.AvgGrade,
                Start = c.Start,
                End = c.End,
                Schedule = c.Schedule,
                IsOnDistance = c.IsOnDistance,
                TeacherId = c.TeacherId
                // typar om min projicering till en IList eftersom min model per automatik vill ta emot en IEnumerable
            }).ToList();

            return View("Index", classrooms);
        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat vid inhämtning av alla kurser",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        // skapa ett nytt objekt för att skicka över till vyn
        var classroom = new ClassroomPostViewModel();
        return View("Create", classroom);
    }

    [HttpPost("create")]
    // Förändrat vad som förväntas att få in som argument 
    public async Task<IActionResult> Create(ClassroomPostViewModel classroom)
    {
        try
        {
            //skriver ut felmeddelandet direkt i vyn med hjälp av dekorations attributen i ClassroomPostViewModel
            if (!ModelState.IsValid) return View("Create", classroom);

            // söker efter ett kursnummer lika med det som kommer in i anropet 
            var exists = await _context.Classrooms.SingleOrDefaultAsync(
                c => c.Number.Trim().ToUpper() == classroom.Number.Trim().ToUpper());

            // kontrollerar om detta nummer redan existerar
            if (exists is not null)
            {
                var error = new ErrorModel
                {
                    ErrorTitle = "Ett fel har inträffat när kursen skulle sparas",
                    ErrorMessage = $"En kurs med numret {classroom.Number} finns redan i systemet"
                };

                //skicka tillbaka en vy som visar information gällande felet 
                return View("_Error", error);
            }

            // här görs en manuell konvertering till typen ClassroomModel som förväntas i vår datacontext 
            var classrooomToAdd = new ClassroomModel
            {
                Number = classroom.Number,
                Name = classroom.Name,
                Title = classroom.Title,
                Content = classroom.Content,
                AvgGrade = classroom.AvgGrade,
                Start = classroom.Start,
                End = classroom.End,
                Schedule = classroom.Schedule,
                IsOnDistance = classroom.IsOnDistance,
                TeacherId = classroom.TeacherId
            };

            // Om allt går bra, inga fel inträffar...

            // lägg upp kursen i minnet
            await _context.Classrooms.AddAsync(classrooomToAdd);
            // spara ner i databas
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
        // Ett annat fel har inträffat som vi inte har räknat med...
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när kursen skulle sparas",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }

    [HttpGet("edit/{classroomId}")]
    public async Task<IActionResult> Edit(int classroomId)
    {
        try
        {
            // får tillbaka en kurs och skicka till en vy
            // här vill jag alltså få tag i en kurs med Id som är lika med det som kommer in i metodanropet
            var result = await _context.Classrooms.SingleOrDefaultAsync(c => c.ClassroomId == classroomId);

            // kontrollerar om jag inte hittar kursen så skickas ett felmeddelande ut 
            if (result is null)
            {
                var error = new ErrorModel
                {
                    ErrorTitle = "Ett fel har inträffat när en kurs skulle hämtas för redigering",
                    ErrorMessage = $"Hittar ingen kurs med id {classroomId}"
                };

                return View("_Error", error);
            }

            // Om jag hittar kursen då retuneras vyn ClassroomUpdateViewModel
            var model = new ClassroomUpdateViewModel
            {
                ClassroomId = result.ClassroomId,
                Number = result.Number,
                Name = result.Name,
                Title = result.Title,
                Content = result.Content,
                AvgGrade = result.AvgGrade,
                Start = result.Start,
                End = result.End,
                Schedule = result.Schedule,
                IsOnDistance = result.IsOnDistance,
                TeacherId = result.TeacherId
            };

            return View("Edit", model);
        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när en kurs skulle hämtas för redigering",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }

    [HttpPost("edit/{classroomId}")]
    public async Task<IActionResult> Edit(int classroomId, ClassroomUpdateViewModel classroom)
    {
        try
        {
            //skriver ut felmeddelandet direkt i vyn med hjälp av dekorations attributen i ClassroomUpdateViewModel
            if (!ModelState.IsValid) return View("Edit", classroom);

            // vara säker på att kursen jag vill redigera/uppdatera verkligen finns i Changetracking listan
            var classroomToUpdate = _context.Classrooms.SingleOrDefault(c => c.ClassroomId == classroomId);

            if (classroomToUpdate is null) return RedirectToAction(nameof(Index));

            classroomToUpdate.Number = classroom.Number;
            classroomToUpdate.Name = classroom.Name;
            classroomToUpdate.Title = classroom.Title;
            classroomToUpdate.Start = classroom.Start;
            classroomToUpdate.End = classroom.End;

            //uppdatera en kurs via ef 
            _context.Classrooms.Update(classroomToUpdate);

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

    [Route("delete/{classroomId}")]
    public async Task<IActionResult> Delete(int classroomId)
    {
        try
        {
            //hämta in kursen som jag vill radera 
            var classroomToDelete = await _context.Classrooms.SingleOrDefaultAsync(c => c.ClassroomId == classroomId);

            if (classroomToDelete is null) return RedirectToAction(nameof(Index));

            //radera en kurs direkt 
            _context.Classrooms.Remove(classroomToDelete);

            // spara ner i databas (alla ändringar på en o samma gång med _context)
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            var error = new ErrorModel
            {
                ErrorTitle = "Ett fel har inträffat när kursen skulle raderas",
                ErrorMessage = ex.Message
            };

            return View("_Error", error);
        }
    }
}
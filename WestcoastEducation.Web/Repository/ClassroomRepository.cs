using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Web.Data;
using WestcoastEducation.Web.Interfaces;
using WestcoastEducation.Web.Models;

namespace WestcoastEducation.Web.Repository;

// här talar jag om att jag vill realisera (använda) ett Interface som heter IClassroomRepository
public class ClassroomRepository : IClassroomRepository
{
    private readonly WestcoastEducationContext _context;
    // definerar att det är ClassroomRepository som kommunicerar med databasen
    public ClassroomRepository(WestcoastEducationContext context)
    {
        _context = context;
    }

    public async Task<bool> AddAsync(ClassroomModel classroom)
    {
        try
        {
            await _context.Classrooms.AddAsync(classroom);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Task<bool> DeleteAsync(ClassroomModel classroom)
    {
        try
        {
            _context.Classrooms.Remove(classroom);
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public async Task<ClassroomModel?> FindByIdAsync(int id)
    {
        return await _context.Classrooms.FindAsync(id);
    }

    public async Task<ClassroomModel?> FindByNumberAsync(string numb)
    {
        return await _context.Classrooms.SingleOrDefaultAsync(c => c.Number.Trim().ToLower() == numb.Trim().ToLower());
    }

    public async Task<IList<ClassroomModel>> ListAllAsync()
    {
        // hämtar alla kurser från databasen
        return await _context.Classrooms.ToListAsync();
    }

    public async Task<bool> SaveAsync()
    {
        try
        {
            if (await _context.SaveChangesAsync() > 0) return true;
            return false;
        }
        catch
        {
            return false;
        }
    }

    public Task<bool> UpdateAsync(ClassroomModel classroom)
    {
        try
        {
            _context.Classrooms.Update(classroom); 
            return Task.FromResult(true); 
        }
        catch
        {
        return Task.FromResult(false);
        }
    }
}

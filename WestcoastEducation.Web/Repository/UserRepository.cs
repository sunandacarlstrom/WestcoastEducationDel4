using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Web.Data;
using WestcoastEducation.Web.Interfaces;
using WestcoastEducation.Web.Models;

namespace WestcoastEducation.Web.Repository;

public class UserRepository : IUserRepository
{
    private readonly WestcoastEducationContext _context;
    // definerar att det är ClassroomRepository som kommunicerar med databasen
    public UserRepository(WestcoastEducationContext context)
    {
        _context = context;
    }

    public async Task<bool> AddAsync(UserModel user)
    {
        try
        {
            await _context.Users.AddAsync(user);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Task<bool> DeleteAsync(UserModel user)
    {
        try
        {
            _context.Users.Remove(user);
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public async Task<UserModel?> FindByEmailAsync(string mail)
    {
        // tillåter INTE dubletter av e-postadresser med metoden SingleOrDefaultAsync
        return await _context.Users.SingleOrDefaultAsync(u => u.Email.Trim().ToLower() ==
        mail.Trim().ToLower());
    }

    public async Task<UserModel?> FindByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<IList<UserModel>> ListAllAsync()
    {
        return await _context.Users.ToListAsync();
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

    public Task<bool> UpdateAsync(UserModel user)
    {
        try
        {
            _context.Users.Update(user);
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }
}

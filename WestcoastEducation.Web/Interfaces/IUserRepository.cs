using WestcoastEducation.Web.Models;

namespace WestcoastEducation.Web.Interfaces;

public interface IUserRepository
{
    Task<IList<UserModel>> ListAllAsync();
    Task<UserModel?> FindByIdAsync(int id);
    Task<UserModel?> FindByEmailAsync(string mail);
    Task<bool> AddAsync(UserModel user);
    Task<bool> UpdateAsync(UserModel user);
    Task<bool> DeleteAsync(UserModel user);
    Task<bool> SaveAsync();
}

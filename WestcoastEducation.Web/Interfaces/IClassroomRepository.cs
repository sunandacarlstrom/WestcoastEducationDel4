using WestcoastEducation.Web.Models;

namespace WestcoastEducation.Web.Interfaces;
public interface IClassroomRepository
{
    //här sätter jag upp metoder som ska jobba mot databasen 
    Task<IList<ClassroomModel>> ListAllAsync();
    // denna är nullable ifall jag inte hittar någoting 
    Task<ClassroomModel?> FindByIdAsync(int id);
    Task<ClassroomModel?> FindByNumberAsync(string numb);
    Task<bool> AddAsync(ClassroomModel classroom);
    Task<bool> UpdateAsync(ClassroomModel classroom);
    Task<bool> DeleteAsync(ClassroomModel classroom);
    Task<bool> SaveAsync();
}

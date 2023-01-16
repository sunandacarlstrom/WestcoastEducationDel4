using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Web.Models;

namespace WestcoastEducation.Web.Data
{
    public class WestcoastEducationContext : IdentityDbContext
    {
        // Skapar kopplingen mellan min databas och mina klasser
        public DbSet<ClassroomModel> Classrooms => Set<ClassroomModel>();

        public WestcoastEducationContext(DbContextOptions options) : base(options) { }
    }
}
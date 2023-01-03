using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Web.Models;

namespace WestcoastEducation.Web.Data
{
    public class WestcoastEducationContext : DbContext
    {
        // Skapar kopplingen mellan min databas och mina klasser
        public DbSet<Classroom> Classrooms => Set<Classroom>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Teacher> Teachers => Set<Teacher>();

        public WestcoastEducationContext(DbContextOptions options) : base(options) { }
    }
}
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Web.Models;

namespace WestcoastEducation.Web.Data
{
    public class WestcoastEducationContext : DbContext
    {
        //TODO: Lägg till flera klasser här för att mappa till min databas (skapa kopplingen mellan min databas och mina klasser)
        public DbSet<Classroom> Classrooms => Set<Classroom>();

        public WestcoastEducationContext(DbContextOptions options) : base(options) { }
    }
}
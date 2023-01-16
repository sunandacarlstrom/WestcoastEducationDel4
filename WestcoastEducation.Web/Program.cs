using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WestcoastEducation.Web.Data;
using WestcoastEducation.Web.Interfaces;
using WestcoastEducation.Web.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add database support 
builder.Services.AddDbContext<WestcoastEducationContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"))
);

// Set up Identity to use cookies and our westcoastcars context... 
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    // options.Password.RequireLowercase = true; 
    // options.Password.RequireUppercase = true;

    options.Lockout.MaxFailedAccessAttempts = 5;

    options.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<WestcoastEducationContext>();

// Configure cookies...
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/account/login";
});

// Add Dependency Injection
builder.Services.AddScoped<IClassroomRepository, ClassroomRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed the database 
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<WestcoastEducationContext>();
    await context.Database.MigrateAsync();
    await SeedData.LoadClassroomData(context);
}

catch (Exception ex)
{
    Console.WriteLine("{0} - {1}", ex.Message, ex.InnerException!.Message);
    throw;
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

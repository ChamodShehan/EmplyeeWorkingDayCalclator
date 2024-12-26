using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebApplication2.Entry;
using WebApplication2.Helpers;
using WebApplication2.Services;

var builder = WebApplication.CreateBuilder(args);

// Retrieve connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DBCS");

// Register EmployeeDB as a Scoped dependency (recommended for database contexts)
builder.Services.AddScoped<EmployeeDB>(_ => new EmployeeDB(connectionString));

// Register the EmployeeService and other dependencies
builder.Services.AddScoped<EmployeeService>();

// Register IMemoryCache to enable caching capabilities
builder.Services.AddMemoryCache();

// Register CacheHelper if it’s used by EmployeeService
builder.Services.AddScoped<CacheHelper>();

// Add MVC controllers and views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Configure default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


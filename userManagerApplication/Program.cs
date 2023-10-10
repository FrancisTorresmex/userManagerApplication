using Microsoft.EntityFrameworkCore;
using userManagerAplication.Models.Data;
using userManagerApplication.Repository.Entities;
using userManagerApplication.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//inyección conexión bd y contexto
builder.Services.AddDbContext<UserManagerAplicationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionSqlServer"));
});

//inyección de repository
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); //forma generica
builder.Services.AddScoped<IUsersRepository<User>, UsersRepository>(); //forma especifica

var app = builder.Build();

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

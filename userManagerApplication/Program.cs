using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using userManagerAplication.Models.Data;
using userManagerApplication.Repository.Entities;
using userManagerApplication.Repository.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



//Autorización y JWT
string keyJWT = builder.Configuration["keyJWT"];
builder.Services.AddAuthorization(options =>
{
    //Policy custom
    //options.AddPolicy(IdentityData.AdminUserPolicyName, p =>
    //p.RequireClaim(IdentityData.AdminUserClaimName, "true"));

    //options.AddPolicy(IdentityData.UserPolicyName, p =>
    //p.RequireClaim(IdentityData.UserClaimName, "true"));

    options.AddPolicy("AdminPolicy", policy => policy.RequireAssertion(context =>
    {
        return context.User.HasClaim(c => c.Type == "Admin" && c.Value == "true");
    }));

    options.AddPolicy("UserPolicy", policy => policy.RequireAssertion(context =>
    {
        return context.User.HasClaim(c => c.Type == "Admin" && c.Value == "false");
    }));

});

builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    //Decir a jwt que en lugar de enviar token por header, sera por cookie
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["Token"]; //nombre de cookie
            return Task.CompletedTask;
        }
    };

    //JWT
    var signingkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyJWT));
    var signingCredentials = new SigningCredentials(signingkey, SecurityAlgorithms.HmacSha256Signature);

    options.RequireHttpsMetadata = false;

    //Parametros del token
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = signingkey
    };
});

//Sesión de usuario (el tiempo que se mantiene viva la sesión con inactividad)
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(20); //20 min de inactividad permitidos
//});

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

//app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");/*.RequireAuthorization();*/ //.RequireAuthorization() para complementar el jwt

app.Run();

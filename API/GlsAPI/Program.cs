using GlsAPI.Data;
using GlsAPI.Interfaces;
using GlsAPI.Models;
using GlsAPI.Repository;
using GlsAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.IgnoreNullValues = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")), ServiceLifetime.Transient);
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var dbContext = services.GetRequiredService<DBContext>();
        dbContext.Database.EnsureCreated();
        if (!dbContext.Users.Any())
        {
            var newUser = new User { UserName = "User1", Password = "User123#" };
            newUser.PasswordHash();
            dbContext.Users.Add(newUser);
            dbContext.SaveChanges();
        }
        if (!dbContext.Errors.Any())
        {
            var errors = new List<Error>
            {   //Login, Logout
                new Error{Name ="err_user_incorrect_username_password", Message="Niepoprawna nazwa u�ytkownika i/lub has�o."},
                new Error{Name ="err_user_permissions_problem", Message="Problem z uprawnieniami u�ytkownika. Taki przypadek wymaga kontaktu w firm� GLS."},
                new Error{Name ="err_user_blocked", Message="Konto u�ytkownika w systemie jest zablokowane. Nie ma mo�liwo�ci zalogowania si� do systemu."},
                new Error{Name ="err_user_webapi_blocked", Message="U�ytkownik nie posiada dost�pu do us�ug WebAPI."},
                new Error{Name ="err_user_login_by_localization_code_required", Message="U�ytkownik musi si� zalogowa� za pomoc� adeLoginByLocalizationCode."},
                new Error{Name ="err_sess_create_problem", Message="Problem z procesem autoryzacji. Taki przypadek wymaga kontaktu w firm� GLS."},
                new Error{Name ="err_sess_not_found", Message="Identyfikator sesji nie zosta� znaleziony. Uzyskanie nowego identyfikatora mo�liwe jest tylko za pomoc� funkcji adeLogin, adeLoginIntegrator lub adeLoginByLocalizationCode"},
                new Error{Name ="err_sess_expired", Message="Wa�no�� identyfikatora sesji wygas�a. Aplikacja kliencka pr�bowa�a komunikowa� si� z systemem po up�ywie czasu okre�lonym w Wytycznych dla aplikacji."},
                new Error{Name ="err_user_insufficient_permissions", Message="U�ytkownik nie posiada odpowiednich uprawnie�, aby wykona� podan� metod�."},
            };
            dbContext.Errors.AddRange(errors);
            dbContext.SaveChanges();
        }
    }
    catch (Exception ex)
    {

    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("MyCorsPolicy");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

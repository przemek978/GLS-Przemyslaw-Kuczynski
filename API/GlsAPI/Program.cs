using GlsAPI.Data;
using GlsAPI.Interfaces;
using GlsAPI.Models;
using GlsAPI.Repository;
using GlsAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Reflection;

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
builder.Services.AddScoped<IPackageService, PackageService>();
builder.Services.AddScoped<IPackageRepository, PackageRepository>();

builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var dbContext = services.GetRequiredService<DBContext>();
        dbContext.Database.EnsureCreated();
        if (!dbContext.Customers.Any() && !dbContext.Users.Any())
        {
            var newUser = new User { UserName = "User1", Password = "User123#" };
            var customers = new List<Customer>{
                new Customer
                {
                    Name1 = "Jan",
                    Country = "PL",
                    ZipCode = "00-001",
                    City = "Warszawa",
                    Street = "ul. Prosta",
                    Phone = "123-456-789",
                    Contact = "Jan Kowalski"
                },
                new Customer
                {
                    Name1 = "Anna",
                    Country = "PL",
                    ZipCode = "50-001",
                    City = "Wroc³aw",
                    Street = "ul. Krótka",
                    Phone = "987-654-321",
                    Contact = "Anna Nowak"
                }
            };
            newUser.PasswordHash();
            dbContext.Users.Add(newUser);
            dbContext.Customers.AddRange(customers);
            dbContext.SaveChanges();

        }
        if (!dbContext.Errors.Any())
        {
            var errors = new List<Error>
            {   //Login, Logout
                new Error{Name ="err_user_incorrect_username_password", Message="Niepoprawna nazwa u¿ytkownika i/lub has³o."},
                new Error{Name ="err_user_permissions_problem", Message="Problem z uprawnieniami u¿ytkownika. Taki przypadek wymaga kontaktu w firm¹ GLS."},
                new Error{Name ="err_user_blocked", Message="Konto u¿ytkownika w systemie jest zablokowane. Nie ma mo¿liwoœci zalogowania siê do systemu."},
                new Error{Name ="err_user_webapi_blocked", Message="U¿ytkownik nie posiada dostêpu do us³ug WebAPI."},
                new Error{Name ="err_user_login_by_localization_code_required", Message="U¿ytkownik musi siê zalogowaæ za pomoc¹ adeLoginByLocalizationCode."},
                new Error{Name ="err_sess_create_problem", Message="Problem z procesem autoryzacji. Taki przypadek wymaga kontaktu w firm¹ GLS."},
                new Error{Name ="err_sess_not_found", Message="Identyfikator sesji nie zosta³ znaleziony. Uzyskanie nowego identyfikatora mo¿liwe jest tylko za pomoc¹ funkcji adeLogin, adeLoginIntegrator lub adeLoginByLocalizationCode"},
                new Error{Name ="err_sess_expired", Message="Wa¿noœæ identyfikatora sesji wygas³a. Aplikacja kliencka próbowa³a komunikowaæ siê z systemem po up³ywie czasu okreœlonym w Wytycznych dla aplikacji."},
                new Error{Name ="err_user_insufficient_permissions", Message="U¿ytkownik nie posiada odpowiednich uprawnieñ, aby wykonaæ podan¹ metodê."},
                //ConsignsIds
                new Error{Name ="err_id_start_invalid", Message="Identyfikator startowy jest niepoprawny."},
                //Consign
                new Error{Name ="err_cons_not_found", Message="Nie znaleziono przesy³ki."},

            };
            dbContext.Errors.AddRange(errors);
            dbContext.SaveChanges();
        }
        if (!dbContext.PackageStatuses.Any())
        {
            var statuses = new List<PackageStatus>
            {
                new PackageStatus{ Name="Zarejestrowana"},
                new PackageStatus{ Name="Przechowywana"},
                new PackageStatus{ Name="Wydana do dorêczenia"},
                new PackageStatus{ Name="Dostarczona"},
            };
            dbContext.PackageStatuses.AddRange(statuses);
            dbContext.SaveChanges();
        }
        if (dbContext.Customers.Any() && !dbContext.Packages.Any() && dbContext.PackageStatuses.Any())
        {  
            for (int i = 0; i < 5; i++)
            {
                var package = new Package
                {
                    Id = 123450 + i,
                    References = $"Ref{i + 1}",
                    Notes = $"Note{i + 1}",
                    Quantity = 1,
                    Weight = 10.5f + i,
                    Date = DateTime.Now.AddDays(-2),
                    SenderId = (i % 2)+1,
                    RecipientId = ((i +1) % 2) + 1,
                    StatusId = 2
                };
                dbContext.Packages.Add(package);
            }
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

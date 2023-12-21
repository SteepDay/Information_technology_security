using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using lab_2_server;
using System.Data;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder();

// Регистрация контекста базы данных
builder.Services.AddDbContext<AppServerContext>(options => options.UseMySql("server=localhost;database=testBD;user=root;password=root;", new MySqlServerVersion(new Version(8, 0, 32))));

// Регистрация сервисов
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
                ("BasicAuthentication", null);
builder.Services.AddAuthorization();

var app = builder.Build();

// Использование аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

// Обработчики запросов

// Регистрация нового пользователя
app.MapPost("/registration", async (HttpRequest request, AppServerContext db) =>
{
    try
    {
        var objJson = await request.ReadFromJsonAsync<LoginPassword>();
        if (objJson != null)
        {
            var login = objJson.Login;
            var password = objJson.Password;
            var userFind = db.Users.FirstOrDefault(u => u.Login == login);
            if (userFind == null)
            {
                User newUser = new User { Login = login, Password = Encoding.UTF8.GetString(SHA1.HashData(Encoding.UTF8.GetBytes(password))) };
                db.Users.Add(newUser);
                newUser.UserRights.Add(new UserRight { UserId = newUser.Id, RightId = 1 });
                db.SaveChanges();
                return Results.Ok();
            }
        }
        return Results.BadRequest();
    }
    catch
    {
        return Results.BadRequest();
    }
});

// Получение прав пользователя
app.MapGet("/login", [Authorize] (HttpContext context) =>
{
    try
    {
        var roles = context.User.FindAll(ClaimsIdentity.DefaultRoleClaimType);
        List<string> rights = new List<string>();
        foreach (var claim in roles)
        {
            rights.Add(claim.Value);
        }
        return Results.Json(rights);
    }
    catch
    {
        return Results.BadRequest();
    }
});

// Получение данных (список)
app.MapGet("/data/read", [Authorize(Roles = "READ")] async (AppServerContext db) =>
{
    try
    {
        List<Data>? data = await db.Datas.ToListAsync();
        return Results.Json(data);
    }
    catch
    {
        return Results.BadRequest();
    }
});

// Запись новых данных
app.MapPost("/data/write", [Authorize(Roles = "WRITE")] async (HttpRequest request, AppServerContext db) =>
{
    try
    {
        var objJson = await request.ReadFromJsonAsync<ValueData>();
        if (objJson != null)
        {
            await db.Datas.AddAsync(new Data { Value = objJson.Value });
            await db.SaveChangesAsync();
            return Results.Ok();
        }
        return Results.BadRequest();
    }
    catch
    {
        return Results.BadRequest();
    }
});

app.Run();

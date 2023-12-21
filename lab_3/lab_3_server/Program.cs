using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using lab_3_server;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1.Ocsp;

var builder = WebApplication.CreateBuilder();

// Настроим подключение к базе данных MySQL через Entity Framework
builder.Services.AddDbContext<AppServerContext>(options => options.UseMySql("server=localhost;database=testBD;user=root;password=root;", new MySqlServerVersion(new Version(8, 0, 32))));

// Добавляем сервисы в контейнер зависимостей
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
                ("BasicAuthentication", null);
builder.Services.AddAuthorization();

var app = builder.Build();

// Используем наше промежуточное ПО для шифрования/дешифрования данных
app.UseMiddleware<EncryptionMiddleware>();

// Включаем аутентификацию и авторизацию
app.UseAuthentication();
app.UseAuthorization();

// Определяем обработчики для различных HTTP-запросов
app.MapPost("/registration", async (HttpRequest request, AppServerContext db) =>
{
    try
    {
        // Обрабатываем запрос на регистрацию пользователя
        var objJson = await request.ReadFromJsonAsync<LoginPassword>();
        if (objJson != null)
        {
            var login = objJson.Login;
            var password = objJson.Password;
            var userFind = db.Users.FirstOrDefault(u => u.Login == login);
            if (userFind == null)
            {
                // Создаем нового пользователя
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

app.MapGet("/some", (HttpContext context) =>
{
    try
    {
        // Обрабатываем запрос "some"
        ValueData data = new ValueData() { Value = "5" };
        return Results.Json(data);
    }
    catch
    {
        return Results.BadRequest();
    }
});

app.MapGet("/login", [Authorize] (HttpContext context) =>
{
    try
    {
        // Обрабатываем запрос на получение прав пользователя (аутентифицированный запрос)
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

app.MapGet("/data/read", [Authorize(Roles = "READ")] async (AppServerContext db) =>
{
    try
    {
        // Обрабатываем запрос на чтение данных (аутентифицированный запрос с ролью READ)
        List<Data>? data = await db.Datas.ToListAsync();
        return Results.Json(data);
    }
    catch
    {
        return Results.BadRequest();
    }
});

app.MapPost("/data/write", [Authorize(Roles = "WRITE")] async (HttpContext context, HttpRequest request, AppServerContext db) =>
{
    try
    {
        // Обрабатываем запрос на запись данных (аутентифицированный запрос с ролью WRITE)
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

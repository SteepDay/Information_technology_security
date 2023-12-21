using Microsoft.EntityFrameworkCore;
using lab_1_server;

var builder = WebApplication.CreateBuilder();

// Регистрация сервиса контекста базы данных
builder.Services.AddDbContext<AppServerContext>(options => options.UseMySql("server=localhost;database=testBD;user=root;password=root;", new MySqlServerVersion(new Version(8, 0, 32))));

var app = builder.Build();

// Обработчик запроса на чтение данных
app.MapGet("/data/read", async (AppServerContext db) =>
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

// Обработчик запроса на запись данных
app.MapPost("/data/write", async (HttpRequest request, AppServerContext db) =>
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
        else
        {
            return Results.BadRequest();
        }
    }
    catch
    {
        return Results.BadRequest();
    }
});

app.Run();

using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MySqlDemo;
using NoEntityFramework;
using NoEntityFramework.MySql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMySqlDbContext<ApplicationDbContext>(
    options =>
    {
        options.ConnectionString = "server=127.0.0.1;uid=root;pwd=password;database=noentityframeworktest";
        options.NumberOfTries = 6;
        options.MaxTimeInterval = 5;
        options.DbCommandTimeout = 20;
        options.EnableStatistics = true;
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/users", async ([FromServices] ApplicationDbContext dbContext) =>
    {
        await dbContext.UseCommand(
                @"Insert into users (id, name)
                    VALUES (@id, @name);")
            .WithParameter(new User() { Id = 1, Name = "Ben" })
            .ExecuteAsync();

        var user = (await dbContext
                .UseCommand("Select * from users;")
                .AsListAsync<User>())
            .First();

        for (var i = 0; i < 20; i++)
        {
            await using var query = dbContext.UseCommand("select * from users;");
            await using var reader = await query.AsDataReaderAsync();
            await reader.ReadAsync();
            Console.WriteLine(reader[1]);
        }

        await dbContext.UseCommand(
                @"DELETE FROM users
                    Where id=@id;")
            .WithInputParameter("@id", MySqlDbType.Int32, 1)
            .ExecuteAsync();
        return user;
    })
    .WithName("GetUsers");

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
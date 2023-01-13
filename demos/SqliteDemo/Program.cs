using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using NoEntityFramework.Sqlite;
using SqliteDemo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSqliteDbContext<ApplicationDbContext>(
    options =>
    {
        options.ConnectionString = "Data Source=demo.db;Cache=Shared";
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
                @"Insert into User (Id, Name)
                    VALUES ($Id, $Name);")
            .WithParameter(new User() { Id = 1, Name = "Ben" })
            .ExecuteAsync();

        var user = (await dbContext
                .UseCommand("Select * from User;")
                .AsListAsync<User>())
            .First();

        for (var i = 0; i < 20; i++)
        {
            await using var query = dbContext.UseCommand("select * from User;");
            await using var reader = await query.AsDataReaderAsync();
            await reader.ReadAsync();
            Console.WriteLine(reader[1]);
        }

        await dbContext.UseCommand(
                @"DELETE FROM User
                    Where Id=$Id;")
            .WithInputParameter("$Id", SqliteType.Integer, 1)
            .ExecuteAsync();
        return user;
    })
    .WithName("GetUsers");

app.Run();
using System.Data;
using Microsoft.AspNetCore.Mvc;
using NoEntityFramework;
using NoEntityFramework.Npgsql;
using NpgsqlTypes;
using PostgresDemo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPostgresDbContext<ApplicationDbContext>(
    options =>
    {
        options.ConnectionString = "Server=127.0.0.1;Port=5433;Database=postgres;User Id=demo;Password=password;;\r\n";
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

app.MapGet("/users", async ([FromServices] ApplicationDbContext dbContext) =>
    {
        await dbContext.UseCommand(
                @"select * from fn_test(@var_id, @var_name);")
            .WithParameter(new User() { Id = 1, Name = "Ben" })
            .ExecuteAsync();

        var user = (await dbContext
            .UseCommand("Select * from \"User\";")
            .AsListAsync<User>())
            .First();

        for (var i = 0; i < 20; i++)
        {
            using var query = dbContext.UseCommand("select * from \"User\";");
            await using var reader = await query.AsDataReaderAsync();
            await reader.ReadAsync();
            Console.WriteLine(reader[1]);
        }

        await dbContext.UseCommand(
                @"DELETE FROM ""User""
                    Where Id=:var_Id;")
            .WithInputParameter("var_Id", NpgsqlDbType.Integer, 1)
            .ExecuteAsync();
        return user;
    })
.WithName("GetUsers");

app.Run();
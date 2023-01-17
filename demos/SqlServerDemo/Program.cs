using System.Data;
using Microsoft.AspNetCore.Mvc;
using NoEntityFramework;
using NoEntityFramework.SqlServer;
using SqlServerDemo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSqlServerDbContext<ApplicationDbContext>(
    options =>
    {
        options.ConnectionString = "Server=(localdb)\\MSSQLLocalDB; Database=NoEntityFrameWorkDemo; MultipleActiveResultSets=true;TrustServerCertificate=true;Max Pool Size=5";
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
                @"Insert into [dbo].[User] (Id, Name)
                    VALUES (@Id, @Name);")
            .WithParameter(new User() { Id = 1, Name = "Ben" })
            .ExecuteAsync();

        var user = (await dbContext
            .UseCommand("Select * from [dbo].[User];")
            .AsListAsync<User>())
            .First();

        for (var i = 0; i < 20; i++)
        {
            using var query = dbContext.UseCommand("select * from [dbo].[User];");
            await using var reader = await query.AsDataReaderAsync();
            await reader.ReadAsync();
            Console.WriteLine(reader[1]);
        }

        await dbContext.UseCommand(
                @"DELETE FROM [dbo].[User]
                    Where Id=@Id;")
            .WithInputParameter("@Id", SqlDbType.Int, 1)
            .ExecuteAsync();
        return user;
    })
.WithName("GetUsers");

app.Run();
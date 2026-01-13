
using Microsoft.EntityFrameworkCore;
using TacomaTrivia.Application.Services;
using TacomaTrivia.Application.Contracts;
using TacomaTrivia.Infrastructure;
using TacomaTrivia.Infrastructure.Repositories;
using TacomaTrivia.Application.Services.User;
using TacomaTrivia.Infrastructure.Context;
using TacomaTrivia.Application.Contracts.TeamRecords;
using TacomaTrivia.Infrastructure.Repositories.Postgres;
using TacomaTrivia.Application.Services.TeamRecords;
using TacomaTrivia.Application.Contracts.Team;
using TacomaTrivia.Application.Services.Teams;
using TacomaTrivia.Application.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Connection string: env var ConnectionStrings__Postgres takes precedence
var conn = builder.Configuration.GetConnectionString("New-Postgres");
// Get the connection string from the json file
var teamRecordConn = builder.Configuration.GetConnectionString("TeamRecordsDB");

// Connection string for teams
var teamConn = builder.Configuration.GetConnectionString("TestTeamDb");

// FOr postgres this is the VenueDB Context
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(conn));

// Postgres: TeamRecord DB context
builder.Services.AddDbContext<TeamRecordDBContext>(option => option.UseNpgsql(teamRecordConn));

// Postgre: Teams DB
builder.Services.AddDbContext<TeamDbContext>(option => option.UseNpgsql(teamConn));

//For MS Sql
builder.Services.AddDbContext<UserDbContext>(opt => opt.UseSqlServer(
    builder.Configuration.GetConnectionString("UserMockMsSql"),
    sql => sql.EnableRetryOnFailure() // good for transient faults
));

// DI for the repo and service for Venues (Postgres)
builder.Services.AddScoped<IVenueRepository, EfVenueRepository>();
builder.Services.AddScoped<IVenueService, VenueService>();

builder.Services.AddScoped<IUserRepository, EFUserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Team DI
builder.Services.AddScoped<ITeamRepository, EFTeamRepo>();
builder.Services.AddScoped<ITeamService, TeamService>();

//register the HttpContextAccesor
builder.Services.AddHttpContextAccessor();
//Current user DI
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

// DI for TeamRecord
builder.Services.AddScoped<ITeamRecordRepository, EFTeamRecordRepo>();
builder.Services.AddScoped<ITeamRecordSvc,TeamRecordSvc>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers()
// THis snippet converts enum values such that when 
// Dtos for team use "owner" instead of 1 for team role
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()
        );
    }
    )
;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//var summaries = new[]
// {
//     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
// };

// app.MapGet("/weatherforecast", () =>
// {
//     var forecast = Enumerable.Range(1, 5).Select(index =>
//         new WeatherForecast
//         (
//             DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//             Random.Shared.Next(-20, 55),
//             summaries[Random.Shared.Next(summaries.Length)]
//         ))
//         .ToArray();
//     return forecast;
// })
// .WithName("GetWeatherForecast")
// .WithOpenApi();

app.MapControllers();

app.Run();

// record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// {
//     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
// }

//created for Api integration tests
public partial class Program { }
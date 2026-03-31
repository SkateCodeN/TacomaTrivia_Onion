
using Microsoft.EntityFrameworkCore;
using TacomaTrivia.Application.Services;
using TacomaTrivia.Application.Contracts;
using TacomaTrivia.Infrastructure;
using TacomaTrivia.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using TacomaTrivia.Api.Auth;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Connection string: env var ConnectionStrings__Postgres takes precedence
var conn = builder.Configuration.GetConnectionString("Postgres");

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(conn));

builder.Services.AddScoped<IVenueRepository, EfVenueRepository>();
builder.Services.AddScoped<IVenueService, VenueService>();



// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Auth Middleware

// 1. Get secret key from Config (User Secrets / Env vars -- This is for when it goes to server)
var jwtKey = builder.Configuration["Jwt:Key"] ?? "Temp";
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

// 2. Configure Auth (Id who the user is)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "tacomatrivia.com",
        ValidateAudience = true,
        ValidAudience = "tacomatrivia.com",
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateLifetime = true
    };
});

// 3. Configure Authorization (Decides what the user can do)
builder.Services.AddAuthorization(options =>
{
    // "ReadOnly" allows both Admin and TestUser to view data
    options.AddPolicy(AuthConfig.ReadOnlyPolicy, policy =>
        policy.RequireRole(AuthConfig.AdminRole, AuthConfig.TestUserRole));

    // "AdminOnly" strictly requires the Admin role 
    options.AddPolicy(AuthConfig.AdminPolicy, policy =>
        policy.RequireRole(AuthConfig.AdminRole));

});

// We confifure Swagger to handle JWT and to test with it

builder.Services.AddSwaggerGen( config =>
{
    config.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Paste JWT Token here"
    });

    config.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
        { new Microsoft.OpenApi.Models.OpenApiSecurityScheme { 
            Reference = new Microsoft.OpenApi.Models.OpenApiReference { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" }
        }, Array.Empty<string>() }
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// We register our auth and swagger middleware
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();
// So that we can use our published frontned 
// we will use the middleware to render static files from wwwroot
app.UseStaticFiles();
app.MapControllers();

// SPA fallback so client-side routing works
app.MapFallbackToFile("index.html");

app.Run();


//created for Api integration tests
public partial class Program { }
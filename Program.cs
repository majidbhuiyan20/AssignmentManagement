using AssignmentManagement.Data;
using AssignmentManagement.Interfaces;
using AssignmentManagement.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});


// Dependency Injection
    builder.Services.AddScoped<IPasswordHasher, PasswordHasherService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));
    builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();


// Add services to the container.
builder.Services.AddOpenApi();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
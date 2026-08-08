using AssignmentManagement.Data;
using AssignmentManagement.Interfaces;
using AssignmentManagement.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAuthorization();

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

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

if (jwtSettings is null)
{
    throw new InvalidOperationException("Jwt settings are missing.");
}

if (Encoding.UTF8.GetByteCount(jwtSettings.Key) < 32)
{
    throw new InvalidOperationException("Jwt key must be at least 32 bytes for HS256.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });


// Add services to the container.
builder.Services.AddOpenApi();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

using AssignmentManagement.Data;
using AssignmentManagement.Interfaces;
using AssignmentManagement.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

using System.Text;

var builder = WebApplication.CreateBuilder(args);


// ======================================================
// Controllers
// ======================================================

builder.Services.AddControllers();


// ======================================================
// Authorization
// ======================================================

builder.Services.AddAuthorization();


// ======================================================
// Database
// ======================================================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});


// ======================================================
// Dependency Injection
// ======================================================

builder.Services.AddScoped<IPasswordHasher, PasswordHasherService>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<ITeacherAssignmentService,TeacherAssignmentService>();

// ======================================================
// JWT Settings
// ======================================================

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt")
);

var jwtSettings = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtSettings>();

if (jwtSettings is null)
{
    throw new InvalidOperationException(
        "Jwt settings are missing."
    );
}

if (string.IsNullOrWhiteSpace(jwtSettings.Key))
{
    throw new InvalidOperationException(
        "Jwt key is missing."
    );
}

if (Encoding.UTF8.GetByteCount(jwtSettings.Key) < 32)
{
    throw new InvalidOperationException(
        "Jwt key must be at least 32 bytes for HS256."
    );
}


// ======================================================
// JWT Authentication
// ======================================================

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidateAudience = true,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings.Issuer,

                ValidAudience = jwtSettings.Audience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            jwtSettings.Key
                        )
                    )
            };
    });


// ======================================================
// Swagger
// ======================================================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    // JWT Security Definition
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",

            Type = SecuritySchemeType.Http,

            Scheme = "bearer",

            BearerFormat = "JWT",

            In = ParameterLocation.Header,

            Description =
                "Enter your JWT token."
        }
    );


    // Apply JWT security to Swagger endpoints
    options.AddSecurityRequirement(
        document => new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecuritySchemeReference("Bearer", document),
                new List<string>()
            }
        }
    );
});


var app = builder.Build();


// ======================================================
// HTTP Request Pipeline
// ======================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


app.UseHttpsRedirection();


// IMPORTANT:
// Authentication must come before Authorization.

app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();

app.Run();

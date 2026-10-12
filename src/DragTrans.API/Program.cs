using DragTrans.Application.Interfaces;
using DragTrans.Application.Interfeces;
using DragTrans.Application.Services.Files;
using DragTrans.Application.Services.Jwt;
using DragTrans.Application.Services.LogIn;
using DragTrans.Application.Services.Register;
using DragTrans.Application.Services.SettingProfile;
using DragTrans.Application.Services.SettingProfile.ChangePassword;
using DragTrans.Infrastructure.Data;
using DragTrans.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using DragTrans.Application.Service.Profile;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DragTransDbContext>(options =>
    options.UseNpgsql(connectionString));

var jwtSetting = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtSetting>()
    ?? throw new InvalidOperationException("JWT настройки не найдены.");

builder.Services.AddSingleton(jwtSetting);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();

builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<LogInService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<SettingService>();
builder.Services.AddScoped<ProfileService>();

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Введите JWT токен в формате: Bearer {token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    var key = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT не настроен.");

    var issuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT issuer не настроен.");

    var audience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT audience не настроен.");

    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,

        ValidateAudience = true,
        ValidAudience = audience,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key)
            ),

        ValidateLifetime = true,

        ClockSkew = TimeSpan.Zero
    };
});
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
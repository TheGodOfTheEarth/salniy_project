using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AutoService.API.Models;
using AutoService.API.Services;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. ПОДКЛЮЧЕНИЕ К БАЗЕ ДАННЫХ
// ==========================================
var connectionString = "Host=localhost;Port=5432;Database=autoservice_db;Username=autoservice;Password=autoservice_pass";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// ==========================================
// 2. РЕГИСТРАЦИЯ НАШИХ СЕРВИСОВ
// ==========================================
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

// ==========================================
// 3. НАСТРОЙКА JWT АУТЕНТИФИКАЦИИ
// ==========================================
var jwtKey = "SuperSecretKeyForAutoService1234567890!"; 
var jwtIssuer = "AutoServiceAPI";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddControllers();

// ==========================================
// 4. НАСТРОЙКА ВСТРОЕННОГО OPENAPI (.NET 10)
// ==========================================
builder.Services.AddOpenApi();

var app = builder.Build();

// ==========================================
// 5. CONFIGURE THE HTTP REQUEST PIPELINE
// ==========================================
if (app.Environment.IsDevelopment())
{
    // Генерирует OpenAPI JSON документ
    app.MapOpenApi(); 
    
    // Добавляет UI Scalar (современная замена Swagger UI)
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("AutoService API");
    });
}

app.UseHttpsRedirection();

// ВАЖНО: Порядок этих двух строк менять нельзя!
app.UseAuthentication(); // Сначала проверяем токен
app.UseAuthorization();  // Потом проверяем права доступа

app.MapControllers();

app.Run();
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoService.API.DTOs;
using AutoService.API.Models;
using AutoService.API.Services;
using Microsoft.AspNetCore.Identity; // Для хэширования паролей

namespace AutoService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")] // Базовый URL: api/v1/auth
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly TokenService _tokenService;
    private readonly IPasswordHasher<User> _passwordHasher; // Используем стандартный хэшер

    public AuthController(AppDbContext context, TokenService tokenService, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        // 1. Проверяем, есть ли уже такой email
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            return BadRequest("Пользователь с таким email уже существует");

        // 2. Создаем нового пользователя
        var user = new User
        {
            Email = request.Email,
            FullName = request.FullName,
            Phone = request.Phone,
            Role = "client", // По умолчанию — клиент
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // 3. Хэшируем пароль (никогда не храните пароли в открытом виде!)
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        // 4. Сохраняем в БД
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // 5. Возвращаем ответ (можно сразу выдать токен, чтобы не заставлять логиниться)
        return Ok(new AuthResponse
        {
            Token = _tokenService.CreateToken(user),
            Role = user.Role,
            Email = user.Email
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        // 1. Ищем пользователя
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        
        if (user == null || !user.IsActive)
            return Unauthorized("Неверный email или пароль, либо аккаунт заблокирован");

        // 2. Проверяем пароль
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
            return Unauthorized("Неверный email или пароль");

        // 3. Выдаем токен
        return Ok(new AuthResponse
        {
            Token = _tokenService.CreateToken(user),
            Role = user.Role,
            Email = user.Email
        });
    }
}
using AutoMapper;
using Microsoft.Extensions.Logging;
using SmartStoreReservation.Core.DTOs;
using SmartStoreReservation.Core.Entities;
using SmartStoreReservation.Core.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SmartStoreReservation.Services;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(RegisterDto dto);
    Task<UserDto> LoginAsync(LoginDto dto);
}

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AuthService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserDto> RegisterAsync(RegisterDto dto)
    {
        _logger.LogInformation("Se înregistrează utilizatorul cu email-ul {Email}", dto.Email);

        // Verifică dacă email-ul există deja
        var existingUsers = await _unitOfWork.Repository<User>()
            .FindAsync(u => u.Email.ToLower() == dto.Email.ToLower());

        if (existingUsers.Any())
        {
            throw new InvalidOperationException("Un utilizator cu acest email există deja.");
        }

        // Creează utilizatorul
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Email = dto.Email.ToLower(),
            PasswordHash = HashPassword(dto.Password),
            Measurements = dto.Measurements,
            StylePreferences = dto.StylePreferences,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Repository<User>().AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Utilizatorul {Email} a fost înregistrat cu succes", dto.Email);

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> LoginAsync(LoginDto dto)
    {
        _logger.LogInformation("Încercare de autentificare pentru {Email}", dto.Email);

        // Caută utilizatorul după email
        var users = await _unitOfWork.Repository<User>()
            .FindAsync(u => u.Email.ToLower() == dto.Email.ToLower());

        var user = users.FirstOrDefault();

        if (user == null)
        {
            _logger.LogWarning("Utilizatorul cu email-ul {Email} nu a fost găsit", dto.Email);
            throw new UnauthorizedAccessException("Email sau parolă incorectă.");
        }

        // Verifică parola
        if (!VerifyPassword(dto.Password, user.PasswordHash))
        {
            _logger.LogWarning("Parolă incorectă pentru utilizatorul {Email}", dto.Email);
            throw new UnauthorizedAccessException("Email sau parolă incorectă.");
        }

        _logger.LogInformation("Utilizatorul {Email} s-a autentificat cu succes", dto.Email);

        return _mapper.Map<UserDto>(user);
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private static bool VerifyPassword(string password, string hash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == hash;
    }
}

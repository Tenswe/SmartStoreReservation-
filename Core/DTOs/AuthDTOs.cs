using System.ComponentModel.DataAnnotations;

namespace SmartStoreReservation.Core.DTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "Numele este obligatoriu")]
    [StringLength(100, ErrorMessage = "Numele nu poate depăși 100 de caractere")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email-ul este obligatoriu")]
    [EmailAddress(ErrorMessage = "Format email invalid")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parola este obligatorie")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Parola trebuie să aibă între 6 și 100 de caractere")]
    public string Password { get; set; } = string.Empty;

    public string? Measurements { get; set; }
    public string? StylePreferences { get; set; }
}

public class LoginDto
{
    [Required(ErrorMessage = "Email-ul este obligatoriu")]
    [EmailAddress(ErrorMessage = "Format email invalid")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Parola este obligatorie")]
    public string Password { get; set; } = string.Empty;
}

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Measurements { get; set; }
    public string? StylePreferences { get; set; }
}

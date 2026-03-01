using Amaris.Application.DTOs.Auth;
using FluentValidation;

namespace Amaris.Application.Validators;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El usuario es requerido")
            .MinimumLength(3).WithMessage("Mínimo 3 caracteres")
            .MaximumLength(50).WithMessage("Máximo 50 caracteres")
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("Solo letras, números y guión bajo");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida")
            .MinimumLength(6).WithMessage("Mínimo 6 caracteres")
            .Matches("[A-Z]").WithMessage("Debe contener al menos una mayúscula")
            .Matches("[0-9]").WithMessage("Debe contener al menos un número");
    }
}
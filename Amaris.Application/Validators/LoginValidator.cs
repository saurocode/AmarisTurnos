using Amaris.Application.DTOs.Auth;
using FluentValidation;

namespace Amaris.Application.Validators;

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El usuario es requerido")
            .MinimumLength(3).WithMessage("Mínimo 3 caracteres");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida")
            .MinimumLength(6).WithMessage("Mínimo 6 caracteres");
    }
}
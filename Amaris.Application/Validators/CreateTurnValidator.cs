using Amaris.Application.DTOs.Turn;
using FluentValidation;

namespace Amaris.Application.Validators
{
    public class CreateTurnValidator : AbstractValidator<CreateTurnDto>
    {
        public CreateTurnValidator()
        {
            RuleFor(x => x.Identification)
                .NotEmpty().WithMessage("La cédula es requerida")
                .MinimumLength(6).WithMessage("Mínimo 6 dígitos")
                .MaximumLength(15).WithMessage("Máximo 15 dígitos")
                .Matches("^[0-9]+$").WithMessage("Solo se permiten números");

            RuleFor(x => x.IdLocation)
                .GreaterThan(0).WithMessage("Selecciona una sede válida");

            RuleFor(x => x.ServiceId)
                .GreaterThan(0).WithMessage("Selecciona un servicio válido");
        }
    }
}

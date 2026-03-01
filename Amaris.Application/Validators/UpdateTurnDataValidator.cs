using Amaris.Application.DTOs.Turn;
using FluentValidation;

namespace Amaris.Application.Validators
{
    public class UpdateTurnDataValidator : AbstractValidator<UpdateTurnDataDto>
    {
        public UpdateTurnDataValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El Id del turno es inválido");

            RuleFor(x => x.IdLocation)
                .GreaterThan(0).WithMessage("Selecciona una sede válida");

            RuleFor(x => x.ServiceId)
                .GreaterThan(0).WithMessage("Selecciona un servicio válido");
        }
    }
}

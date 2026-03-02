using Amaris.Application.DTOs.Turn;
using FluentValidation;

namespace Amaris.Application.Validators
{
    public class UpdateTurnStatusValidator : AbstractValidator<UpdateTurnDto>
    {
        private static readonly string[] ValidStatuses =
            { "Pendiente", "Activo", "Completado", "Expirado", "Cancelado" };

        public UpdateTurnStatusValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El Id del turno es inválido");

            RuleFor(x => x.NewStatus)
                .NotEmpty().WithMessage("El estado es requerido")
                .Must(s => ValidStatuses.Contains(s))
                .WithMessage($"Estado inválido. Valores permitidos: {string.Join(", ", ValidStatuses)}");
        }
    }
}

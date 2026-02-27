using Amaris.Application.DTOs.Turn;

namespace Amaris.Application.Interfaces
{
    public interface ITurnService
    {
        Task<TurnResponseDto> CrearTurnoAsync(CreateTurnDto dto);
        Task<IEnumerable<TurnResponseDto>> ObtenerTodosAsync();
        Task<TurnResponseDto?> ObtenerPorIdAsync(int id);
        Task<TurnResponseDto> ActivarTurnoAsync(int id);
        Task<TurnResponseDto> ActualizarEstadoAsync(UpdateTurnDto dto);
        Task ProcesarTurnosExpiradosAsync();
    }
}

using Amaris.Application.DTOs.Turn;

namespace Amaris.Application.Interfaces
{
    public interface ITurnService
    {
        Task<TurnResponseDto> CreateTurnAsync(CreateTurnDto dto);
        Task<IEnumerable<TurnResponseDto>> GetAllAsync();
        Task<TurnResponseDto?> GetByIdAsync(int id);
        Task<TurnResponseDto> ActivateTurnAsync(int id);
        Task<TurnResponseDto> UpdateStatusAsync(UpdateTurnDto dto);
        Task ProcessExpiredTurnAsync();
        Task<IEnumerable<TurnResponseDto>> GetByIdentificationAsync(string identification);
    }
}

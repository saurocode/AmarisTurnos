using Amaris.Application.DTOs.Turn;
using Amaris.Application.Interfaces;
using Amaris.Domain.Entities;
using Amaris.Domain.Enums;
using Amaris.Domain.Interfaces.Repositories;

namespace Amaris.Application.Services
{
    public class TurnService : ITurnService
    {
        private readonly ITurnRepository _turnRepository;
        private readonly ILocationRepository _sucursalRepository;
        private readonly IServiceRepository _serviceRepository;
        private const int DailyTurnLimit = 5;
        private const int MinutesLimit = 15;

        public TurnService(ITurnRepository turnoRepository, ILocationRepository sucursalRepository, IServiceRepository serviceRepository)
        {
            _turnRepository = turnoRepository;
            _sucursalRepository = sucursalRepository;
            _serviceRepository = serviceRepository;
        }


        public async Task<TurnResponseDto> CreateTurnAsync(CreateTurnDto dto)
        {
            var turnosHoy = await _turnRepository.CountTurnTodayByCedulaAsync(dto.Identification);
            if (turnosHoy >= DailyTurnLimit)
                throw new InvalidOperationException($"La cédula {dto.Identification} ya alcanzó el límite de {DailyTurnLimit} turnos por día.");

            var sucursal = await _sucursalRepository.GetByIdAsync(dto.IdLocation)
                ?? throw new KeyNotFoundException($"La sucursal con Id {dto.IdLocation} no existe.");

            var service = await _serviceRepository.GetByIdAsync(dto.ServiceId)
                 ?? throw new KeyNotFoundException($"El servicio con Id {dto.ServiceId} no existe.");

            var ahora = DateTime.UtcNow;
            var turno = new Turn
            {
                Identification = dto.Identification,
                IdLocation = dto.IdLocation,
                ServiceId = dto.ServiceId,
                DateCreation = ahora,
                DateExpiration = ahora.AddMinutes(MinutesLimit),
                Status = StatusTurn.Pendiente,
                TurnCode = GenerateCode()
            };

            var creado = await _turnRepository.CreateAsync(turno);
            return MapToDto(creado, sucursal.Name);
        }

        public async Task<IEnumerable<TurnResponseDto>> GetAllAsync()
        {
            var turnos = await _turnRepository.GetAllAsync();
            return turnos.Select(t => MapToDto(t, t.Location?.Name ?? ""));
        }

        public async Task<TurnResponseDto?> GetByIdAsync(int id)
        {
            var turno = await _turnRepository.GetByIdAsync(id);
            return turno is null ? null : MapToDto(turno, turno.Location?.Name ?? "");
        }

        public async Task<TurnResponseDto> ActivateTurnAsync(int id)
        {
            var turno = await _turnRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Turno {id} no encontrado.");

            if (turno.Status == StatusTurn.Expirado)
                throw new InvalidOperationException("El turno ya expiró. Por favor genere uno nuevo.");

            if (turno.Status != StatusTurn.Pendiente)
                throw new InvalidOperationException($"El turno no puede activarse. Status actual: {turno.Status}");

            if (DateTime.UtcNow > turno.DateExpiration)
            {
                turno.Status = StatusTurn.Expirado;
                await _turnRepository.UpdateAsync(turno);
                throw new InvalidOperationException("El turno expiró. Por favor genere uno nuevo.");
            }

            turno.Status = StatusTurn.Activo;
            turno.DateActivation = DateTime.UtcNow;
            var actualizado = await _turnRepository.UpdateAsync(turno);
            return MapToDto(actualizado, actualizado.Location?.Name ?? "");
        }

        public async Task<TurnResponseDto> UpdateStatusAsync(UpdateTurnDto dto)
        {
            var turno = await _turnRepository.GetByIdAsync(dto.Id)
                ?? throw new KeyNotFoundException($"Turno {dto.Id} no encontrado.");

            if (!Enum.TryParse<StatusTurn>(dto.NewStatus, true, out var nuevoStatus))
                throw new ArgumentException($"Status '{dto.NewStatus}' no válido.");

            turno.Status = nuevoStatus;
            var actualizado = await _turnRepository.UpdateAsync(turno);
            return MapToDto(actualizado, actualizado.Location?.Name ?? "");
        }

        public async Task ProcessExpiredTurnAsync()
        {
            var expirados = await _turnRepository.GetExpiredTurnAsync();
            foreach (var turno in expirados)
            {
                turno.Status = StatusTurn.Expirado;
                await _turnRepository.UpdateAsync(turno);
            }
        }

        private static string GenerateCode() =>
            $"T-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";

        private static TurnResponseDto MapToDto(Turn turno, string nombreSucursal, string serviceName = "")
        {
            var minutosRestantes = turno.Status == StatusTurn.Pendiente
                ? Math.Max(0, (int)(turno.DateExpiration - DateTime.UtcNow).TotalMinutes)
                : 0;

            return new TurnResponseDto
            {
                Id = turno.Id,
                TurnCode = turno.TurnCode,
                Identification = turno.Identification,
                IdLocation = turno.IdLocation,
                LocationName = nombreSucursal,
                DateCreation = turno.DateCreation,
                DateExpiration = turno.DateExpiration,
                DateActivation = turno.DateActivation,
                Status = turno.Status.ToString(),
                MinutesRemaining = minutosRestantes,
                ServiceId = turno.ServiceId,
                ServiceName = turno.Service?.Name ?? ""
            };
        }

        public async Task<IEnumerable<TurnResponseDto>> GetByIdentificationAsync(string identification)
        {
            var turns = await _turnRepository.GetByIdentificationAsync(identification);
            return turns.Select(t => MapToDto(t, t.Location?.Name ?? "", t.Service?.Name ?? ""));
        }

    }
}

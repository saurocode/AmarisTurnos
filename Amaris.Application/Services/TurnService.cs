using Amaris.Application.DTOs.Turn;
using Amaris.Application.Interfaces;
using Amaris.Domain.Entities;
using Amaris.Domain.Enums;
using Amaris.Domain.Interfaces.Repositories;

namespace Amaris.Application.Services
{
    public class TurnService : ITurnService
    {
        private readonly ITurnRepository _turnoRepository;
        private readonly ILocationRepository _sucursalRepository;
        private readonly IServiceRepository _serviceRepository;
        private const int LimiteTurnosDiarios = 5;
        private const int MinutosLimite = 15;

        public TurnService(ITurnRepository turnoRepository, ILocationRepository sucursalRepository, IServiceRepository serviceRepository)
        {
            _turnoRepository = turnoRepository;
            _sucursalRepository = sucursalRepository;
            _serviceRepository = serviceRepository;
        }


        public async Task<TurnResponseDto> CrearTurnoAsync(CreateTurnDto dto)
        {
            var turnosHoy = await _turnoRepository.CountTurnosHoyByCedulaAsync(dto.Identification);
            if (turnosHoy >= LimiteTurnosDiarios)
                throw new InvalidOperationException($"La cédula {dto.Identification} ya alcanzó el límite de {LimiteTurnosDiarios} turnos por día.");

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
                DateExpiration = ahora.AddMinutes(MinutosLimite),
                Status = StatusTurn.Pendiente,
                TurnCode = GenerarCodigo()
            };

            var creado = await _turnoRepository.CreateAsync(turno);
            return MapToDto(creado, sucursal.Name);
        }

        public async Task<IEnumerable<TurnResponseDto>> ObtenerTodosAsync()
        {
            var turnos = await _turnoRepository.GetAllAsync();
            return turnos.Select(t => MapToDto(t, t.Location?.Name ?? ""));
        }

        public async Task<TurnResponseDto?> ObtenerPorIdAsync(int id)
        {
            var turno = await _turnoRepository.GetByIdAsync(id);
            return turno is null ? null : MapToDto(turno, turno.Location?.Name ?? "");
        }

        public async Task<TurnResponseDto> ActivarTurnoAsync(int id)
        {
            var turno = await _turnoRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Turno {id} no encontrado.");

            if (turno.Status == StatusTurn.Expirado)
                throw new InvalidOperationException("El turno ya expiró. Por favor genere uno nuevo.");

            if (turno.Status != StatusTurn.Pendiente)
                throw new InvalidOperationException($"El turno no puede activarse. Status actual: {turno.Status}");

            if (DateTime.UtcNow > turno.DateExpiration)
            {
                turno.Status = StatusTurn.Expirado;
                await _turnoRepository.UpdateAsync(turno);
                throw new InvalidOperationException("El turno expiró. Por favor genere uno nuevo.");
            }

            turno.Status = StatusTurn.Activo;
            turno.DateActivation = DateTime.UtcNow;
            var actualizado = await _turnoRepository.UpdateAsync(turno);
            return MapToDto(actualizado, actualizado.Location?.Name ?? "");
        }

        public async Task<TurnResponseDto> ActualizarEstadoAsync(UpdateTurnDto dto)
        {
            var turno = await _turnoRepository.GetByIdAsync(dto.Id)
                ?? throw new KeyNotFoundException($"Turno {dto.Id} no encontrado.");

            if (!Enum.TryParse<StatusTurn>(dto.NewStatus, true, out var nuevoStatus))
                throw new ArgumentException($"Status '{dto.NewStatus}' no válido.");

            turno.Status = nuevoStatus;
            var actualizado = await _turnoRepository.UpdateAsync(turno);
            return MapToDto(actualizado, actualizado.Location?.Name ?? "");
        }

        public async Task ProcesarTurnosExpiradosAsync()
        {
            var expirados = await _turnoRepository.GetTurnosExpiradosAsync();
            foreach (var turno in expirados)
            {
                turno.Status = StatusTurn.Expirado;
                await _turnoRepository.UpdateAsync(turno);
            }
        }

        private static string GenerarCodigo() =>
            $"T-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";

        private static TurnResponseDto MapToDto(Turn turno, string nombreSucursal)
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

    }
}

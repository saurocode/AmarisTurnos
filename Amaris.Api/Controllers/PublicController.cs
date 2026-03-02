using Amaris.Application.Common;
using Amaris.Application.DTOs.Turn;
using Amaris.Application.Interfaces;
using Amaris.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Amaris.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("public")]
public class PublicController : ControllerBase
{
    private readonly ITurnService _turnService;
    private readonly ILocationRepository _locationRepository;
    private readonly IServiceRepository _serviceRepository;

    public PublicController(
        ITurnService turnService,
        ILocationRepository locationRepository,
        IServiceRepository serviceRepository)
    {
        _turnService = turnService;
        _locationRepository = locationRepository;
        _serviceRepository = serviceRepository;
    }

    // GET api/public/turns/1234567890
    [HttpGet("turns/{identification}")]
    public async Task<IActionResult> GetTurnsByIdentification(string identification)
    {
        if (string.IsNullOrWhiteSpace(identification) || !identification.All(char.IsDigit))
            return BadRequest(ApiResponse<object>.Fail("Cédula inválida"));

        var data = await _turnService.GetByIdentificationAsync(identification);
        return Ok(ApiResponse<IEnumerable<TurnResponseDto>>.Ok(data));
    }

    // POST api/public/turns
    [HttpPost("turns")]
    public async Task<IActionResult> Create([FromBody] CreateTurnDto dto)
    {
        var data = await _turnService.CreateTurnAsync(dto);
        return Ok(ApiResponse<TurnResponseDto>.Created(data, "Turno agendado exitosamente"));
    }

    // PUT api/public/turns/{id}/activate
    [HttpPut("turns/{id:int}/activate")]
    public async Task<IActionResult> Activate(int id)
    {
        var data = await _turnService.ActivateTurnAsync(id);
        return Ok(ApiResponse<TurnResponseDto>.Ok(data, "Turno activado correctamente"));
    }

    // PUT api/public/turns/status
    [HttpPut("turns/status")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateTurnDto dto)
    {
        var data = await _turnService.UpdateStatusAsync(dto);
        return Ok(ApiResponse<TurnResponseDto>.Ok(data, "Estado actualizado"));
    }

    // GET api/public/locations
    [HttpGet("locations")]
    public async Task<IActionResult> GetLocations()
    {
        var data = await _locationRepository.GetAllActiveAsync();
        return Ok(ApiResponse<object>.Ok(data));
    }

    // GET api/public/services
    [HttpGet("services")]
    public async Task<IActionResult> GetServices()
    {
        var data = await _serviceRepository.GetAllActiveAsync();
        return Ok(ApiResponse<object>.Ok(data));
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Amaris.Application.DTOs.Turn;
using Amaris.Application.Interfaces;

namespace Amaris.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TurnController : ControllerBase
    {
        private readonly ITurnService _turnService;

        public TurnController(ITurnService turnoService) => _turnService = turnoService;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _turnService.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var turno = await _turnService.GetByIdAsync(id);
            return turno is null ? NotFound() : Ok(turno);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTurnDto dto)
        {
            var turno = await _turnService.CreateTurnAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = turno.Id }, turno);
        }

        [HttpPut("{id:int}/activar")]
        public async Task<IActionResult> Activar(int id) =>
            Ok(await _turnService.ActivateTurnAsync(id));

        [HttpPut("estado")]
        public async Task<IActionResult> ActualizarEstado([FromBody] UpdateTurnDto dto) =>
            Ok(await _turnService.UpdateStatusAsync(dto));
    }
}

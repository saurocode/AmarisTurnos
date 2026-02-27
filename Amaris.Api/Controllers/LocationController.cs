using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Amaris.Domain.Interfaces.Repositories;

namespace Amaris.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LocationController : ControllerBase
{
    private readonly ILocationRepository _sucursalRepository;
    public LocationController(ILocationRepository repo) => _sucursalRepository = repo;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _sucursalRepository.GetAllActivasAsync());
}
using Amaris.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Amaris.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("authenticated")]
[Authorize]
public class LocationController : ControllerBase
{
    private readonly ILocationRepository _sucursalRepository;
    public LocationController(ILocationRepository repo) => _sucursalRepository = repo;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _sucursalRepository.GetAllActiveAsync());
}
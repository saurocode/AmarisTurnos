using Amaris.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amaris.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ServiceController : ControllerBase
{
    private readonly IServiceRepository _serviceRepository;
    public ServiceController(IServiceRepository repo) => _serviceRepository = repo;

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _serviceRepository.GetAllActiveAsync());
}
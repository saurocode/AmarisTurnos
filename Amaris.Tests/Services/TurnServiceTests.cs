using Amaris.Application.DTOs.Turn;
using Amaris.Application.Services;
using Amaris.Domain.Entities;
using Amaris.Domain.Enums;
using Amaris.Domain.Interfaces.Repositories;
using Amaris.Tests.Helpers;
using FluentAssertions;
using Moq;
using Xunit;

namespace Amaris.Tests.Services
{
    public class TurnServiceTests
    {
        private readonly Mock<ITurnRepository> _turnRepoMock;
        private readonly Mock<ILocationRepository> _locationRepoMock;
        private readonly Mock<IServiceRepository> _serviceRepoMock;
        private readonly TurnService _turnService;

        public TurnServiceTests()
        {
            _turnRepoMock = new Mock<ITurnRepository>();
            _locationRepoMock = new Mock<ILocationRepository>();
            _serviceRepoMock = new Mock<IServiceRepository>();

            _turnService = new TurnService(
                _turnRepoMock.Object,
                _locationRepoMock.Object,
                _serviceRepoMock.Object);
        }

        [Fact]
        public async Task CreateTurnAsync_ValidData_ReturnsTurnResponse()
        {
            var dto = new CreateTurnDto
            {
                Identification = "1234567890",
                IdLocation = 1,
                ServiceId = 1
            };

            _turnRepoMock.Setup(r => r.CountTurnTodayByCedulaAsync("1234567890")).ReturnsAsync(0);
            _locationRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(MockHelper.CreateLocation());
            _serviceRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(MockHelper.CreateService());
            _turnRepoMock.Setup(r => r.CreateAsync(It.IsAny<Turn>()))
                .ReturnsAsync((Turn t) => { t.Id = 1; return t; });

            var result = await _turnService.CreateTurnAsync(dto);

            result.Should().NotBeNull();
            result.Identification.Should().Be("1234567890");
            result.Status.Should().Be("Pending");
            result.MinutesRemaining.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task CreateTurnAsync_ExceedsDailyLimit_ThrowsInvalidOperationException()
        {
            var dto = new CreateTurnDto
            {
                Identification = "1234567890",
                IdLocation = 1,
                ServiceId = 1
            };

            _turnRepoMock.Setup(r => r.CountTurnTodayByCedulaAsync("1234567890")).ReturnsAsync(5);

            var act = async () => await _turnService.CreateTurnAsync(dto);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*límite*");
        }

        [Fact]
        public async Task CreateTurnAsync_LocationNotFound_ThrowsKeyNotFoundException()
        {
            var dto = new CreateTurnDto
            {
                Identification = "1234567890",
                IdLocation = 99,
                ServiceId = 1
            };

            _turnRepoMock.Setup(r => r.CountTurnTodayByCedulaAsync("1234567890")).ReturnsAsync(0);
            _locationRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Location?)null);

            var act = async () => await _turnService.CreateTurnAsync(dto);

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task ActivateTurnAsync_PendingTurn_ReturnsActivatedTurn()
        {
            
            var turn = MockHelper.CreateTurn(status: StatusTurn.Pendiente);
            _turnRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(turn);
            _turnRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Turn>()))
                .ReturnsAsync((Turn t) => t);

            
            var result = await _turnService.ActivateTurnAsync(1);

            
            result.Status.Should().Be("Active");
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task ActivateTurnAsync_ExpiredTurn_ThrowsInvalidOperationException()
        {

            var turn = MockHelper.CreateTurn(status: StatusTurn.Pendiente);
            turn.DateExpiration = DateTime.UtcNow.AddMinutes(-1); 

            _turnRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(turn);
            _turnRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Turn>()))
                .ReturnsAsync((Turn t) => t);

  
            var act = async () => await _turnService.ActivateTurnAsync(1);

    
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("*expiró*");
        }

        [Fact]
        public async Task ActivateTurnAsync_TurnNotFound_ThrowsKeyNotFoundException()
        {

            _turnRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Turn?)null);

            var act = async () => await _turnService.ActivateTurnAsync(99);

            await act.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllTurns()
        {
            var turns = new List<Turn>
        {
            MockHelper.CreateTurn(1),
            MockHelper.CreateTurn(2),
            MockHelper.CreateTurn(3)
        };

            _turnRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(turns);

            var result = await _turnService.GetAllAsync();

            result.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetByIdentificationAsync_ValidCedula_ReturnsTurns()
        {
            var turns = new List<Turn>
        {
            MockHelper.CreateTurn(1, "1234567890"),
            MockHelper.CreateTurn(2, "1234567890")
        };

            _turnRepoMock.Setup(r => r.GetByIdentificationAsync("1234567890")).ReturnsAsync(turns);

            var result = await _turnService.GetByIdentificationAsync("1234567890");

            result.Should().HaveCount(2);
            result.All(t => t.Identification == "1234567890").Should().BeTrue();
        }

        [Fact]
        public async Task UpdateStatusAsync_CancelPendingTurn_ReturnsCancelledTurn()
        {

            var turn = MockHelper.CreateTurn(status: StatusTurn.Pendiente);
            var dto = new UpdateTurnDto { Id = 1, NewStatus = "Cancelado" };

            _turnRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(turn);
            _turnRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Turn>()))
                .ReturnsAsync((Turn t) => t);

            var result = await _turnService.UpdateStatusAsync(dto);
            result.Status.Should().Be("Cancelado");
        }
    }
}

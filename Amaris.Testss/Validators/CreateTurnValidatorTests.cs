using Amaris.Application.DTOs.Turn;
using Amaris.Application.Validators;
using FluentAssertions;

namespace Amaris.Tests.Validators
{
    public class CreateTurnValidatorTests
    {
        private readonly CreateTurnValidator _validator = new();

        [Fact]
        public void Validate_ValidDto_ShouldPass()
        {
            var dto = new CreateTurnDto
            {
                Identification = "1234567890",
                IdLocation = 1,
                ServiceId = 1
            };

            var result = _validator.Validate(dto);
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("123")]
        [InlineData("abc123")]
        public void Validate_InvalidIdentification_ShouldFail(string identification)
        {
            var dto = new CreateTurnDto
            {
                Identification = identification,
                IdLocation = 1,
                ServiceId = 1
            };

            var result = _validator.Validate(dto);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_InvalidLocation_ShouldFail()
        {
            var dto = new CreateTurnDto
            {
                Identification = "1234567890",
                IdLocation = 0,
                ServiceId = 1
            };

            var result = _validator.Validate(dto);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "IdLocation");
        }

        [Fact]
        public void Validate_InvalidService_ShouldFail()
        {
            var dto = new CreateTurnDto
            {
                Identification = "1234567890",
                IdLocation = 1,
                ServiceId = 0
            };

            var result = _validator.Validate(dto);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "ServiceId");
        }
    }
}

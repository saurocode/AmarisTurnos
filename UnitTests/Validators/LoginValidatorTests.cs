using Amaris.Application.DTOs.Auth;
using Amaris.Application.Validators;
using FluentAssertions;
using Xunit;

namespace UnitTests.Validators
{
    public class LoginValidatorTests
    {
        private readonly LoginValidator _validator = new();

        [Fact]
        public void Validate_ValidCredentials_ShouldPass()
        {
            var dto = new LoginDto { Username = "admin", Password = "Admin123" };
            var result = _validator.Validate(dto);
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("", "Admin123")]
        [InlineData("ab", "Admin123")]
        public void Validate_InvalidUsername_ShouldFail(string username, string password)
        {
            var dto = new LoginDto { Username = username, Password = password };
            var result = _validator.Validate(dto);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Username");
        }

        [Theory]
        [InlineData("admin", "")]
        [InlineData("admin", "123")]
        public void Validate_InvalidPassword_ShouldFail(string username, string password)
        {
            var dto = new LoginDto { Username = username, Password = password };
            var result = _validator.Validate(dto);
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Password");
        }
    }
}

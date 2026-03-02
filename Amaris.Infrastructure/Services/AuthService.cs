using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Amaris.Application.DTOs.Auth;
using Amaris.Application.Interfaces;
using Amaris.Domain.Entities;
using Amaris.Domain.Interfaces.Repositories;
using BCrypt.Net;

namespace Amaris.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;   
            _configuration = configuration;           
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var usuario = await _usuarioRepository.GetByUsernameAsync(dto.Username)
                ?? throw new UnauthorizedAccessException("Credenciales inválidas.");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
                throw new UnauthorizedAccessException("Credenciales inválidas.");

            return GenerarToken(usuario);
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var existe = await _usuarioRepository.GetByUsernameAsync(dto.Username);
            if (existe is not null)
                throw new InvalidOperationException("El usuario ya existe.");

            var usuario = new Usuario
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Rol = "Cliente"
            };

            var creado = await _usuarioRepository.CreateAsync(usuario);
            return GenerarToken(creado);
        }

        private AuthResponseDto GenerarToken(Usuario usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key no configurada")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddHours(8);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, usuario.Username),
            new Claim(ClaimTypes.Role, usuario.Rol),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiracion,
                signingCredentials: creds);

            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Username = usuario.Username,
                Expiration = expiracion
            };
        }
    }
}

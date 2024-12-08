using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MT.Domain;
using MT.Dtos.Auth;
using MT.Infrastructure;

namespace MT.Applicationservices.Module.Implements
{
    public class AuthenticationService
    {
        private readonly MovieTheaterDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            MovieTheaterDbContext dbContext,
            IConfiguration configuration,
            ILogger<AuthenticationService> logger)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<(User user, string token)> LoginAsync(LoginDto loginDto)
        {
            try
            {
                _logger.LogInformation("Attempting to login user with Email: {Email}", loginDto.Email);

                var user = await _dbContext.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.Password == loginDto.Password);

                if (user == null)
                {
                    _logger.LogWarning("Invalid login attempt for Email: {Email}", loginDto.Email);
                    throw new Exception("Invalid email or password.");
                }

                if (user.Role == null)
                {
                    _logger.LogWarning("User with Email: {Email} does not have an assigned role.", loginDto.Email);
                    throw new Exception("User does not have an assigned role.");
                }

                _logger.LogInformation("User retrieved successfully. User ID: {Id}, Role: {RoleName}", user.Id, user.Role.RoleName);

                var token = GenerateJwtToken(user);
                _logger.LogInformation("JWT token generated successfully for user with Email: {Email}", loginDto.Email);

                return (user, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for Email: {Email}", loginDto.Email);
                throw;
            }
        }

        private string GenerateJwtToken(User user)
        {
            try
            {
                _logger.LogInformation("Generating JWT token for user with ID: {Id}", user.Id);

                if (user.Role == null)
                {
                    _logger.LogError("User does not have an assigned role.");
                    throw new Exception("User does not have an assigned role.");
                }

                var claims = new List<Claim>
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.RoleName)
                };

                var key = _configuration["JwtSettings:Key"];
                var issuer = _configuration["JwtSettings:Issuer"];
                var audience = _configuration["JwtSettings:Audience"];
                var durationInMinutes = _configuration["JwtSettings:DurationInMinutes"];

                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(durationInMinutes))
                {
                    throw new Exception("JWT configuration is missing or invalid.");
                }

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer,
                    audience,
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(durationInMinutes)),
                    signingCredentials: credentials);

                _logger.LogInformation("JWT token successfully created for user with ID: {Id}", user.Id);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate JWT token.");
                throw;
            }
        }
    }
}

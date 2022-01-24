using BugTracker2.Configuration;
using BugTracker2.Models.DTOs.Requests;
using BugTracker2.Models.DTOs.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthManagementController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;

        public AuthManagementController(
            UserManager<IdentityUser> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto userRegistration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new UserRegistrationResponse
                {
                    Errors = new List<string>
                    {
                        "Invalid Payload"
                    },
                    Success = false
                });
            }

            var existingUser = await _userManager.FindByEmailAsync(userRegistration.Email);

            if (existingUser is not null)
            {
                return BadRequest(new UserRegistrationResponse
                {
                    Errors = new List<string>
                    {
                        "Email already in use"
                    },
                    Success = false
                });
            }

            var newUser = new IdentityUser
            {
                Email = userRegistration.Email,
                UserName = userRegistration.Name
            };

            var isCreated = await _userManager.CreateAsync(newUser, userRegistration.Password);

            if (!isCreated.Succeeded)
            {
                return BadRequest(new UserRegistrationResponse
                {
                    Errors = isCreated.Errors.Select(x => x.Description).ToList(),
                    Success = false
                });
            }

            var jwtToken = GenerateJwtToken(newUser);

            return Ok(new UserRegistrationResponse
            {
                Success = true,
                Token = jwtToken
            });
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new UserRegistrationResponse
                {
                    Errors = new List<string>
                    {
                        "Invalid Payload"
                    },
                    Success = false
                });
            }

            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser is null)
            {
                return BadRequest(new UserRegistrationResponse
                {
                    Errors = new List<string>
                    {
                        "Invalid login request"
                    },
                    Success = false
                });
            }

            var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

            if (!isCorrect)
            {
                return BadRequest(new UserRegistrationResponse
                {
                    Errors = new List<string>
                    {
                        "Invalid login request"
                    },
                    Success = false
                });
            }

            var jwtToken = GenerateJwtToken(existingUser);

            return Ok(new UserRegistrationResponse
            {
                Success = true,
                Token = jwtToken
            });
        }
        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}

using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;
using ETA.API.Services;
using ETA_API.Helpers;
using ETA_API.Models.StoreProcContextModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SimpleJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        IConfiguration configuration;
        public AuthController(IConfiguration configuration, IUserRepository userRepository)
        {
            this.configuration = configuration;
            _userRepository = userRepository ??
           throw new ArgumentNullException(nameof(userRepository));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Auth([FromBody] User user)
        {
            IActionResult response = Unauthorized();

            if (user != null)
            {
                AuthorizeReponseProcModel authorizeReponseProcModel = new AuthorizeReponseProcModel();

                authorizeReponseProcModel = await _userRepository.GetJwtToken(user.UserName, user.Password);

                if (authorizeReponseProcModel.puser_id != 0)
                {
                    var issuer = configuration["Jwt:Issuer"];
                    var audience = configuration["Jwt:Audience"];
                    var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
                    var signingCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha512Signature
                    );

                    var subject = new ClaimsIdentity(new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                    });

                    var expires = DateTime.UtcNow.AddMinutes(10);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = subject,
                        Expires = expires,
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = signingCredentials
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var refreshToken = GenerateRefreshToken();

                    UserAuthorizeReponseProcModel userAuthorizeReponseProcModel = new UserAuthorizeReponseProcModel();
                    userAuthorizeReponseProcModel.UserId = authorizeReponseProcModel.puser_id;
                    userAuthorizeReponseProcModel.JWtToken = tokenHandler.WriteToken(token);
                    userAuthorizeReponseProcModel.IsFirstTimeLogin = authorizeReponseProcModel.pis_first_time_login;
                    userAuthorizeReponseProcModel.RefreshToken = refreshToken;
                    userAuthorizeReponseProcModel.Email = authorizeReponseProcModel.pemail;

                    return Ok(userAuthorizeReponseProcModel);
                }
            }

            return response;
        }


        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenResponse tokenResponse)
        {
            // For simplicity, assume the refresh token is valid and stored securely
            // var storedRefreshToken = _userService.GetRefreshToken(userId);

            // Verify refresh token (validate against the stored token)
            // if (storedRefreshToken != tokenResponse.RefreshToken)
            //    return Unauthorized();

            // For demonstration, let's just generate a new access token
            var newAccessToken =  GenerateAccessTokenFromRefreshToken(tokenResponse.RefreshToken, configuration["Jwt:Key"]);

            var response = new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = tokenResponse.RefreshToken // Return the same refresh token
            };

            return Ok(response);
        }

        public static string GenerateAccessTokenFromRefreshToken(string refreshToken, string secret)
        {
            // Implement logic to generate a new access token from the refresh token
            // Verify the refresh token and extract necessary information (e.g., user ID)
            // Then generate a new access token

            // For demonstration purposes, return a new token with an extended expiry
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(15), // Extend expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
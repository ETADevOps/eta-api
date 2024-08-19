using AutoMapper;
using ETA.API.Controllers;
using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;
using ETA.API.Services;
using ETA_API.Helpers;
using ETA_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ETA_API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class GeneratePasswordController : ControllerBase
    {
        private readonly IGeneratePasswordService _generatePasswordService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IStorageService _storageService;

        public GeneratePasswordController(IMapper mapper, IGeneratePasswordService generatePasswordService, IConfiguration configuration, IStorageService storageService, ILogger<UsersController> logger)
        {
            _generatePasswordService = generatePasswordService ??
            throw new ArgumentNullException(nameof(generatePasswordService));
            _mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ??
            throw new ArgumentNullException(nameof(configuration));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));

        }

        [Produces("application/json")]
        [HttpGet("GeneratePassword", Name = "GeneratePassword")]
        [Authorize]
        public async Task<IActionResult> GeneratePassword()
        {
            var password = await _generatePasswordService.GeneratePassword(10, 3);
            return Ok(password);
        }

        [Produces("application/json")]
        [HttpPut("RestPassword", Name = "RestPassword")]
        //[Authorize]
        public async Task<IActionResult> RestPassword(int userId, ApiVersion version)
        {

            ServiceResponse obj = new ServiceResponse();

            string password = await _generatePasswordService.GeneratePassword(10, 3);

            int result = 0;
            try
            {
                // Call the service to update the password
                result = await _generatePasswordService.UpdatePassword(userId, password, obj);

            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
                return Ok(obj);
            }
            if (result > 0)
            {
                obj.Data = "Password has been reset successfully and New Password is " + password;
                obj.StatusMessage = "Success";
                return Ok(obj);
            }
            else
            {
                obj.StatusCode = 500;
                obj.StatusMessage = "Failed";
                return Ok("Password reset failed.");
            }
        }

    }
}

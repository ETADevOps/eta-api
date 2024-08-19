using AutoMapper;
using ETA.API.Models.StoreProcContextModel;
using ETA_API.Helpers;
using ETA_API.Models.Referance;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;
using ETA_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETA_API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IStorageService _storageService;

        public AdminController(IAdminRepository adminRepository, IConfiguration configuration, IMapper mapper, IStorageService storageService)
        {
            _adminRepository = adminRepository ?? throw new ArgumentNullException(nameof(adminRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));

           
        }

        [Produces("application/json")]
        [HttpGet("GetRoleList", Name = "GetRoleList")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetRoleList()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                List<RoleListProcModel> roleListProcModel = new List<RoleListProcModel>();
                roleListProcModel = await _adminRepository.GetRoleList();

                var rolesDetailsEntity = _mapper.Map<List<RoleListDto>>(roleListProcModel);

                serviceResponse.Data = rolesDetailsEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }
            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetRoleById/{roleId}", Name = "GetRoleById")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetRoleById(int roleId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                RoleByIdData roleByIdData = new RoleByIdData();
                roleByIdData = await _adminRepository.GetRoleById(roleId);

                serviceResponse.Data = roleByIdData;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }

            return Ok(serviceResponse);
        }

       
        [Produces("application/json")]
        [HttpDelete("DeleteRole", Name = "DeleteRole")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> DeleteRole(List<DeleteByIdModel> delete, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();

            try
            {
                foreach (var item in delete)
                {
                    int result = await _adminRepository.DeleteRole(item);
                }

                obj.Data = "Success";
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
            }

            return Ok(obj);
        }

        [Produces("application/json")]
        [HttpGet("GetRoleOpeningData", Name = "GetRoleOpeningData")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetRoleOpeningData()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                RoleOpeningData roleOpeningData = new RoleOpeningData();
                roleOpeningData = await _adminRepository.GetRoleOpeningData();

                serviceResponse.Data = roleOpeningData;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpPost("CreateRole", Name = "CreateRole")]
        [Authorize]

        public async Task<ActionResult<ServiceResponse>> CreateRole(CreateRole createRole, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _adminRepository.CreateRole(createRole, obj);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
                return Ok(obj);
            }
            if (result != 0)
            {
                obj.Data = result;
                return Ok(obj);
            }
            else
            {
                obj.StatusCode = 500;
                obj.StatusMessage = "Failed";
                return Ok(obj);
            }
        }

        [Produces("application/json")]
        [HttpPost("UpdateRole", Name = "UpdateRole")]
        [Authorize]

        public async Task<ActionResult<ServiceResponse>> UpdateRole(UpdateRole updateRole, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _adminRepository.UpdateRole(updateRole, obj);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
                return Ok(obj);
            }
            if (result != 0)
            {
                obj.Data = result;
                return Ok(obj);
            }
            else
            {
                obj.StatusCode = 500;
                obj.StatusMessage = "Failed";
                return Ok(obj);
            }
        }
    }
}

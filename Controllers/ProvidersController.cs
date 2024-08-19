using AutoMapper;
using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;
using ETA_API.Helpers;
using ETA_API.Models.Referance;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;
using ETA_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETA_API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ProvidersController : ControllerBase
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IMapper _mapper;

        public ProvidersController(IMapper mapper, IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository ??
            throw new ArgumentNullException(nameof(providerRepository));
            _mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
        }

        [Produces("application/json")]
        [HttpGet("GetProvidersList/{clientId}/{providerId}", Name = "GetProvidersList")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetProvidersList(int clientId, int providerId)
        {
            try
            {
                ServiceResponse serviceResponse = new ServiceResponse();

                List<ProvidersProcModel> userList = new List<ProvidersProcModel>();

                userList = await _providerRepository.GetProvidersList(clientId, providerId);

                //var providerDetailsEntity = _mapper.Map<ProvidersDto>(userList);

                serviceResponse.Data = userList;

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Produces("application/json")]
        [HttpGet("GetProvidersById/{providerId}", Name = "GetProvidersById")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetProvidersById(int providerId)
        {
            try
            {
                ProvidersProcModel providerByIdProcModel = new ProvidersProcModel();
                providerByIdProcModel = await _providerRepository.GetProvidersById(providerId);

                var providerDetailsEntity = _mapper.Map<ProvidersDto>(providerByIdProcModel);
                ServiceResponse serviceResponse = new ServiceResponse();
                serviceResponse.Data = providerDetailsEntity;
                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Produces("application/json")]
        [HttpPost("CreateProvider", Name = "CreateProvider")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> CreateProvider(CreateProviderDto createProvider, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _providerRepository.CreateProvider(createProvider, obj);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message.Contains("_idx") ? "Provider Already Exists" : ex.Message;
                return Ok(obj);
            }
            if (result != 0)
            {
                obj.Data = "Success";
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
        [HttpPut("UpdateProvider", Name = "UpdateProvider")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> UpdateProvider(CreateProviderDto createProvider, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _providerRepository.UpdateProvider(createProvider, obj);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message.Contains("_idx") ? "Provider Already Exists" : ex.Message;
                return Ok(obj);
            }
            if (result != 0)
            {
                obj.Data = "Success";
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
        [HttpDelete("DeleteProvider", Name = "DeleteProvider")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> DeleteProvider(List<DeleteByIdModel> delete, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();

            try
            {
                foreach (var item in delete)
                {
                    int result = await _providerRepository.DeleteProvider(item);
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
        [HttpGet("GetProviderOpeningData/{clientId}", Name = "GetProviderOpeningData")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetProviderOpeningData(int clientId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                ProviderOpeningData providerOpeningData = new ProviderOpeningData();
                providerOpeningData = await _providerRepository.GetProviderOpeningData(clientId);

                serviceResponse.Data = providerOpeningData;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetProviderListByClientId/{clientId}", Name = "GetProviderListByClientId")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetProviderListByClientId(int clientId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                List<ProviderListByClientIdProcModel> clientIdModel = new List<ProviderListByClientIdProcModel>();
                clientIdModel = await _providerRepository.GetProviderListByClientId(clientId);

                var clientIdDetailsEntity = _mapper.Map<List<ProviderListByClientId>>(clientIdModel);

                serviceResponse.Data = clientIdDetailsEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }
            return Ok(serviceResponse);
        }
    }
}

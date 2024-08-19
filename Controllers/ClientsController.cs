using AutoMapper;
using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;
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
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IStorageService _storageService;
        private readonly string _azureContainer;
        private readonly string _azureContainerFolder;

        public ClientsController(IMapper mapper, IClientRepository clientRepository, IConfiguration configuration, IStorageService storageService)
        {
            _clientRepository = clientRepository ??
            throw new ArgumentNullException(nameof(clientRepository));
            _mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ??
            throw new ArgumentNullException(nameof(configuration));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));

            _azureContainer = _configuration["AzureBlob:container"];
            _azureContainerFolder = _configuration["AzureBlob:containerFolder"];
        }

        [Produces("application/json")]
        [HttpPost("CreateClients", Name = "CreateClients")]
        [Authorize]

        public async Task<ActionResult<ServiceResponse>> CreateClients(CreateClientDto createClientDto, ApiVersion version)
        {
            if(createClientDto.ClientLogo != null)
            {
                string downloadUrl, path;
                _storageService.UploadAttachment(createClientDto.ClientLogo, "image/png", out downloadUrl, out path, "", ".png");
                createClientDto.ClientLogo = downloadUrl;
            }

            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _clientRepository.CreateClients(createClientDto, obj);
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
        [HttpGet("GetClientOpeningData", Name = "GetClientOpeningData")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetClientOpeningData()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                ClientOpeningData clientOpeningData = new ClientOpeningData();
                clientOpeningData = await _clientRepository.GetClientOpeningData();

                serviceResponse.Data = clientOpeningData;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetClientList/{clientId}", Name = "GetClientList")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetClientList(int clientId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                List<ClientProcModel> clientProcModel = new List<ClientProcModel>();
                clientProcModel = await _clientRepository.GetClientList(clientId);

                var clientDetailsEntity = _mapper.Map<List<ClientDto>>(clientProcModel);

                serviceResponse.Data = clientDetailsEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }
            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpDelete("DeleteClient", Name = "DeleteClient")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> DeleteClient(List<DeleteByIdModel> delete, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();

            try
            {
                foreach (var item in delete)
                {
                    int result = await _clientRepository.DeleteClient(item);
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
        [HttpGet("GetClientById/{clientId}", Name = "GetClientById")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetClientById(int clientId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                ClientByIdOpeningData clientByIdOpeningData = new ClientByIdOpeningData();
                clientByIdOpeningData = await _clientRepository.GetClientById(clientId);

                serviceResponse.Data = clientByIdOpeningData;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpPut("UpdateClient", Name = "UpdateClient")]
        [Authorize]

        public async Task<ActionResult<ServiceResponse>> UpdateClient(CreateClientDto createClientDto, ApiVersion version)
        {
            if (!string.IsNullOrEmpty(createClientDto.ClientLogo) && createClientDto.ClientLogo.Contains("base64"))
            {
                string downloadUrl, path;
                _storageService.UploadAttachment(createClientDto.ClientLogo, "image/png", out downloadUrl, out path, "", ".png");
                createClientDto.ClientLogo = downloadUrl;
            }

            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _clientRepository.UpdateClient(createClientDto, obj);
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

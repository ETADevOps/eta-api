using AutoMapper;
using ETA.API.Models.StoreProcContextModel;
using ETA_API.Helpers;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;
using ETA_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace ETA_API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IStorageService _storageService;
        private readonly string _azureContainer;
        private readonly string _azureContainerFolder;
        public AccountsController(IAccountRepository accountRepository, IConfiguration configuration, IMapper mapper, IStorageService storageService)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));

            _azureContainer = _configuration["AzureBlob:container"];
            _azureContainerFolder = _configuration["AzureBlob:containerFolder"];
        }

        [Produces("application/json")]
        [HttpGet("GetAccountList", Name = "GetAccountList")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetAccountList()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                List<AccountsProcModel> accountProcModel = new List<AccountsProcModel>();
                accountProcModel = await _accountRepository.GetAccountList();

                var accountDetailsEntity = _mapper.Map<List<AccountsDto>>(accountProcModel);

                serviceResponse.Data = accountDetailsEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }
            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetAccountById/{accountId}", Name = "GetAccountById")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetAccountById(int accountId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                AccountsByIdProcModel accountByIdProcModel = new AccountsByIdProcModel();
                accountByIdProcModel = await _accountRepository.GetAccountById(accountId);

                var accountDetailsEntity = _mapper.Map<AccountByIdDto>(accountByIdProcModel);

                
                serviceResponse.Data = accountDetailsEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpPut("UpdateAccount", Name = "UpdateAccount")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> UpdateAccount(UpdateAccountDto updateAccountDto, ApiVersion version)
        {
            if (!string.IsNullOrEmpty(updateAccountDto.AccountLogo) && updateAccountDto.AccountLogo.Contains("base64"))
            {
                string downloadUrl, path;
                _storageService.UploadAttachment(updateAccountDto.AccountLogo, "image/png", out downloadUrl, out path, "", ".png");
                updateAccountDto.AccountLogo = downloadUrl;
            }


            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _accountRepository.UpdateAccount(updateAccountDto, obj);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
                return Ok(obj);
            }
            if (result != 0)
            {
                obj.Data = updateAccountDto.AccountLogo;
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

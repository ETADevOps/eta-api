using AutoMapper;
using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;
using ETA.API.Services;
using ETA_API.Helpers;
using ETA_API.Models.Referance;
using ETA_API.Models.StoreProcModelDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using static ETA_API.Services.Commonservices;
using Microsoft.Graph;
using Microsoft.Extensions.Logging;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext.User;
using ETA_API.Services;
using ETA_API.Models.StoreProcContextModel;
using static System.Net.WebRequestMethods;


namespace ETA.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IStorageService _storageService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMapper mapper, IUserRepository userRepository, IConfiguration configuration, IStorageService storageService, ILogger<UsersController> logger)
        {
            _userRepository = userRepository ??
            throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ??
            throw new ArgumentNullException(nameof(configuration));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            _logger = logger ??
           throw new ArgumentNullException("Logger");
           
        }

        [Produces("application/json")]
        [HttpGet("GetLoginUserDetails", Name = "GetLoginUserDetails")]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetLoginUserDetails([FromQuery] string userIp, int userid)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {

                LoginUsersData loginUsersData = new LoginUsersData();
                loginUsersData = await _userRepository.GetLoginUserDetails(userid);

                int userId = 0;

                if (loginUsersData.UserDetails != null)
                {
                    userId = loginUsersData.UserDetails.user_id;
                }
                else
                {
                    serviceResponse.StatusCode = 100;
                    serviceResponse.StatusMessage = "Failed";
                    serviceResponse.Data = "Invalid User";
                    return serviceResponse;
                }

                bool userIpValid = await _userRepository.GetIpVerification(loginUsersData.UserDetails.user_id, userIp);

                if (userIpValid == false)
                {
                    serviceResponse.StatusCode = 100;
                    serviceResponse.StatusMessage = "Failed";
                    serviceResponse.Data = "The application you have attempted to reach has blocked your access. Please contact Lab for More Information.";
                    return serviceResponse;
                }

                serviceResponse.Data = loginUsersData;
                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"Error occurred while Getting a LoginUserdetails at {System.DateTime.UtcNow}");
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
                return Ok(serviceResponse);
            }

        }

        [Produces("application/json")]
        [HttpGet("GetUsersList/{userId}/{clientId}/{providerId}/{patientId}", Name = "GetUsersList")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetUsersList(int userId, int clientId, int providerId, int patientId)
        {
            try
            {
                //_logger.LogInformation($"Getting UserList Successfully at {System.DateTime.UtcNow}");

                ServiceResponse serviceResponse = new ServiceResponse();

                List<UsersStoreProcModel> userList = new List<UsersStoreProcModel>();

                userList = await _userRepository.GetUsersList(userId, clientId, providerId, patientId);

                //var userDetailsEntity = _mapper.Map<List<UsersMapDto>>(userList);

                serviceResponse.Data = userList;

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"Error occurred while Getting a UserList at {System.DateTime.UtcNow}");
                throw ex;
            }
        }

        [Produces("application/json")]
        [HttpGet("GetUserById/{userId}", Name = "GetUserById")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetUserById(int userId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                UsersByIdStoreProcModel userByIdProcModel = new UsersByIdStoreProcModel();
                userByIdProcModel = await _userRepository.GetUserById(userId, "");

                var userDetailsEntity = _mapper.Map<UsersByIdMapDto>(userByIdProcModel);

                serviceResponse.Data = userDetailsEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }
            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpPost("CreateUser", Name = "CreateUser")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> CreateUser(CreateUserDto createUser, ApiVersion version) //PasswordProfile passwordProfile)
        {
            ServiceResponse obj = new ServiceResponse();

            string redirectURL = _configuration.GetSection("urls").GetSection("webUrl").Value;

            bool status = await _userRepository.CheckEmailAddress(createUser.Email);

            if (status == true)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = "User already exist with this Email Address.";
                return Ok(obj);
            }

            int result = 0;
            try
            {
                result = await _userRepository.CreateUser(createUser, obj);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message.Contains("_idx") ? "User Already Exists" : ex.Message;
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
        [HttpPut("UpdateUser", Name = "UpdateUser")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> UpdateUser(CreateUserDto createUser, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();

            string checkEmailExist = await _userRepository.GetEmailUserId(createUser.UserId);

            if (checkEmailExist != createUser.Email)
            {
                //Check If user exist by Email
                bool status = await _userRepository.CheckEmailAddress(createUser.Email);

                if (status == true)
                {
                    obj.StatusCode = 500;
                    obj.StatusMessage = "User already exist with this Email Address.";
                    return Ok(obj);
                }
            }

            int result = 0;

            try
            {
                result = await _userRepository.UpdateUser(createUser, obj);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message.Contains("_idx") ? "User Already Exists" : ex.Message;
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
        [HttpDelete("DeleteUser", Name = "DeleteUser")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> DeleteUser(List<DeleteByIdModel> delete, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            try
            {
                foreach (var item in delete)
                {
                    int result = await _userRepository.DeleteUser(item);
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
        [HttpPost("CreateUserSignInLog", Name = "CreateUserSignInLog")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> CreateUserSignInLog(CreateUserSignInLog createUserSignInLog, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _userRepository.CreateUserSignInLog(createUserSignInLog, obj);
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
        [HttpPut("UpdateUserSignInLog", Name = "UpdateUserSignInLog")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> UpdateUserSignInLog(UpdateUserSignInLogDto updateUserSignInLogDto, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _userRepository.UpdateUserSignInLog(updateUserSignInLogDto, obj);
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
        [HttpGet("GetUserSignInLogList/{userId}/{fromDate}/{toDate}", Name = "GetUserSignInLogList")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetUserSignInLogList(int userId, DateTime? fromDate, DateTime? toDate)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                List<UserSignInLogProcModel> userSignInLogProcModel = new List<UserSignInLogProcModel>();
                userSignInLogProcModel = await _userRepository.GetUserSignInLogList(userId, fromDate, toDate);

                var userDetailsEntity = _mapper.Map<List<UserSignInLogDto>>(userSignInLogProcModel);

                serviceResponse.Data = userDetailsEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }
            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetAuditOpeningData", Name = "GetAuditOpeningData")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetAuditOpeningData()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                AuditOpeningData auditLogOpeningData = new AuditOpeningData();
                auditLogOpeningData = await _userRepository.GetAuditOpeningData();

                serviceResponse.Data = auditLogOpeningData;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpPost("CreateAuditLog", Name = "CreateAuditLog")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> CreateAuditLog(CreateAuditLog createAuditLog, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _userRepository.CreateAuditLog(createAuditLog, obj);
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
        [HttpGet("GetAuditLogList/{userId}/{fromDate}/{toDate}/{categoryId}/{patientId}/{providerId}/{reportId}/{clientId}", Name = "GetAuditLogList")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetAuditLogList(int userId, DateTime? fromDate, DateTime? toDate, int categoryId, int patientId, int providerId, int reportId, int clientId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                List<AuditLogProcModel> auditLogProcModel = new List<AuditLogProcModel>();
                auditLogProcModel = await _userRepository.GetAuditLogList(userId, fromDate, toDate, categoryId, patientId, providerId, reportId, clientId);

                var userDetailsEntity = _mapper.Map<List<AuditLogDto>>(auditLogProcModel);

                serviceResponse.Data = userDetailsEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }
            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetRecycleBinOpeningData", Name = "GetRecycleBinOpeningData")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetRecycleBinOpeningData()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                List<RecycleBinOpeningDataProcModel> recycleBinOpeningData = new List<RecycleBinOpeningDataProcModel>();
                recycleBinOpeningData = await _userRepository.GetRecycleBinOpeningData();
                var recycleEntity = _mapper.Map<List<RecycleBinOpeningData>>(recycleBinOpeningData);

                serviceResponse.Data = recycleEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetRecycleBinData/{categoryId}", Name = "GetRecycleBinData")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetRecycleBinData(int categoryId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                RecycleBinData recycleData = new RecycleBinData();
                recycleData = await _userRepository.GetRecycleBinData(categoryId);

                serviceResponse.Data = recycleData;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpPut("UpdateRecycleBinData", Name = "UpdateRecycleBinData")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> UpdateRecycleBinData(UpdateRecycleBinDataModel updateRecycleBinDataModel, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _userRepository.UpdateRecycleBinData(updateRecycleBinDataModel, obj);
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
        [HttpGet("GetUserOpeningData/{clientId}/{userId}", Name = "GetUserOpeningData")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetUserOpeningData(int clientId, int userId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                UserOpeningData userOpeningData = new UserOpeningData();
                userOpeningData = await _userRepository.GetUserOpeningData(clientId, userId);

                serviceResponse.Data = userOpeningData;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetSecurityOpeningData", Name = "GetSecurityOpeningData")]
        //[Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetSecurityOpeningData()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                SecurityOpeningData securityOpeningData = new SecurityOpeningData();
                securityOpeningData = await _userRepository.GetSecurityOpeningData();

                serviceResponse.Data = securityOpeningData;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpPut("UpdateOtp", Name = "UpdateOtp")]
        //[Authorize]
        [GzipCompression]
        public async Task<ActionResult> UpdateOtp(UpdateOtpDataModel updateOtpDataModel, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;

            string otp = await _storageService.SendVerificationEmail(updateOtpDataModel.Email);

            try
            {
                result = await _userRepository.UpdateOtp(updateOtpDataModel, obj, otp);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
                return Ok(obj);
            }
            if (result != 0)
            {
                obj.Data = "OTP sent Successfully to your registered email ";
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
        [HttpPut("VerifyOtp", Name = "VerifyOtp")]
        //[Authorize]
        [GzipCompression]
        public async Task<ActionResult> VerifyOtp(VerifyOtpDataModel verifyOtpDataModel, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _userRepository.VerifyOtp(verifyOtpDataModel, obj);
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
        [HttpPut("UpdatePassword", Name = "UpdatePassword")]
        //[Authorize]
        [GzipCompression]
        public async Task<ActionResult> UpdatePassword(UpdatePasswordDataModel updatePasswordDataModel, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _userRepository.UpdatePassword(updatePasswordDataModel, obj);
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
        [HttpPut("UpdateSecurityAnswer", Name = "UpdateSecurityAnswer")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> UpdateSecurityAnswer(UpdateSecurityAnswer updateSecurityAnswer, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _userRepository.UpdateSecurityAnswer(updateSecurityAnswer, obj);
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
        [HttpPut("VerifySecurityAnswer", Name = "VerifySecurityAnswer")]
        //[Authorize]
        [GzipCompression]
        public async Task<ActionResult> VerifySecurityAnswer(VerifySecurityAnswer verifySecurityAnswer, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _userRepository.VerifySecurityAnswer(verifySecurityAnswer, obj);
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
        [HttpGet("VerifyUserName", Name = "VerifyUserName")]
        //[Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> VerifyUserName(string userName)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                VerifyUserName verifyUserName = new VerifyUserName();
                verifyUserName = await _userRepository.VerifyUserName(userName);

                serviceResponse.Data = verifyUserName;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }
            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpPost("CreateDomain", Name = "CreateDomain")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> CreateDomain(CreateDomainDto createDomainDto, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _userRepository.CreateDomain(createDomainDto, obj);
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
        [HttpGet("GetDomainList", Name = "GetDomainList")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetDomainList()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                List<DomainListProcModel> domainListProcModel = new List<DomainListProcModel>();
                domainListProcModel = await _userRepository.GetDomainList();

                var domainDetailsEntity = _mapper.Map<List<DomainListDto>>(domainListProcModel);

                serviceResponse.Data = domainDetailsEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }
            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetDomainById/{domainId}", Name = "GetDomainById")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetDomainById(int domainId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                DomainbyIdProcModel domainbyIdProcModel = new DomainbyIdProcModel();
                domainbyIdProcModel = await _userRepository.GetDomainById(domainId);

                var domainByIdDetailsEntity = _mapper.Map<DomainbyIdDto>(domainbyIdProcModel);


                serviceResponse.Data = domainByIdDetailsEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpDelete("DeleteDomain", Name = "DeleteDomain")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> DeleteDomain(List<DeleteByIdModel> delete, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();

            try
            {
                foreach (var item in delete)
                {
                    int result = await _userRepository.DeleteDomain(item);
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
        [HttpPut("UpdateDomain", Name = "UpdateDomain")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> UpdateDomain(UpdateDomainDto updateDomainDto, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _userRepository.UpdateDomain(updateDomainDto, obj);
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
        [HttpGet("GetDomainClientOpeningData", Name = "GetDomainClientOpeningData")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetDomainClientOpeningData()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                DomainClientOpeningData domainClientOpeningData = new DomainClientOpeningData();
                domainClientOpeningData = await _userRepository.GetDomainClientOpeningData();

                serviceResponse.Data = domainClientOpeningData;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpPost("CreateClientDomainMapping", Name = "CreateClientDomainMapping")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> CreateClientDomainMapping(CreateClientDomainMapping createClientDomainMapping, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _userRepository.CreateClientDomainMapping(createClientDomainMapping, obj);
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
        [HttpGet("GetDomainClientMappingList", Name = "GetDomainClientMappingList")]
        [Authorize]
        [GzipCompression]

        public async Task<ActionResult<ServiceResponse>> GetDomainClientMappingList()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                List<DomainClientMappingListProcModel> domainClientMappingListProcModels = new List<DomainClientMappingListProcModel>();
                domainClientMappingListProcModels = await _userRepository.GetDomainClientMappingList();

                var domainClientDetailsEntity = _mapper.Map<List<DomainClientMappingListDto>>(domainClientMappingListProcModels);

                serviceResponse.Data = domainClientDetailsEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }
            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpDelete("DeleteClientDomain", Name = "DeleteClientDomain")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> DeleteClientDomain(List<DeleteByIdModel> delete, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();

            try
            {
                foreach (var item in delete)
                {
                    int result = await _userRepository.DeleteClientDomain(item);
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
        [HttpGet("GetDomainClientMappingByClientId/{clientId}", Name = "GetDomainClientMappingByClientId")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetDomainClientMappingByClientId(int clientId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                DomainClientMappingByClientIdData domainClientMappingByClientId = new DomainClientMappingByClientIdData();
                domainClientMappingByClientId = await _userRepository.GetDomainClientMappingByClientId(clientId);

                serviceResponse.Data = domainClientMappingByClientId;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpPut("UpdateClientDomainMapping", Name = "UpdateClientDomainMapping")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> UpdateClientDomainMapping(UpdateClientDomainMapping updateClientDomainMappingg, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _userRepository.UpdateClientDomainMapping(updateClientDomainMappingg, obj);
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
        [HttpGet("GetDomainClientMappingByClientIdActive/{clientId}", Name = "GetDomainClientMappingByClientIdActive")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetDomainClientMappingByClientIdActive(int clientId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                DomainClientMappingByClientIdData domainClientMappingByClientId = new DomainClientMappingByClientIdData();
                domainClientMappingByClientId = await _userRepository.GetDomainClientMappingByClientIdActive(clientId);

                serviceResponse.Data = domainClientMappingByClientId;
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
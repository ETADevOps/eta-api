using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;
using ETA_API.Models.Referance;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA.API.Services
{
    public interface IUserRepository
    {
        Task<List<UsersStoreProcModel>> GetUsersList(int userId, int clientId, int providerId, int patientId);
        Task<UsersByIdStoreProcModel> GetUserById(int userId, string subjectId);
        Task<int> CreateUser(CreateUserDto createUser, ServiceResponse obj);
        Task<int> UpdateUser(CreateUserDto createUser, ServiceResponse obj);
        Task<bool> CheckEmailAddress(string email);
        Task<string> GetEmailUserId(int userId);
        Task<int> UpdateSubjectIdByUserId(int userId, string subjectId);
        Task<int> DeleteUser(DeleteByIdModel deleteUser);
        Task<int> CreateUserSignInLog(CreateUserSignInLog createUserSignInLog, ServiceResponse obj);
        Task<int> UpdateUserSignInLog(UpdateUserSignInLogDto updateUserSignInLogDto, ServiceResponse obj);
        Task<List<UserSignInLogProcModel>> GetUserSignInLogList(int userId, DateTime? fromDate, DateTime? toDate);
        Task<bool> GetIpVerification(int userId, string ip);
        Task<AuditOpeningData> GetAuditOpeningData();
        Task<int> CreateAuditLog(CreateAuditLog createAuditLog, ServiceResponse obj);
        Task<List<AuditLogProcModel>> GetAuditLogList(int userId, DateTime? fromDate, DateTime? toDate, int categoryId, int patientId, int providerId, int reportId, int clientId);
        Task<List<RecycleBinOpeningDataProcModel>> GetRecycleBinOpeningData();
        Task<RecycleBinData> GetRecycleBinData(int categoryId);
        Task<int> UpdateRecycleBinData(UpdateRecycleBinDataModel updateRecycleBinDataModel, ServiceResponse obj);
        Task<UserOpeningData> GetUserOpeningData(int clientId, int userId);
        Task<LoginUsersData> GetLoginUserDetails(int userid);
        Task<SecurityOpeningData> GetSecurityOpeningData();
        Task<int> UpdateOtp(UpdateOtpDataModel updateOtpDataModel, ServiceResponse obj, string otp);
        Task<int> VerifyOtp(VerifyOtpDataModel verifyOtpDataModel, ServiceResponse obj);
        Task<AuthorizeReponseProcModel> GetJwtToken(string userName, string password);
        Task<int> UpdatePassword(UpdatePasswordDataModel updatePasswordDataModel, ServiceResponse obj);
        Task<int> UpdateSecurityAnswer(UpdateSecurityAnswer updateSecurityAnswer, ServiceResponse Obj);
        Task<int> VerifySecurityAnswer(VerifySecurityAnswer verifySecurityAnswer, ServiceResponse Obj);
        Task<VerifyUserName> VerifyUserName(string userName);
       
        Task<int> CreateDomain(CreateDomainDto createDomainDto, ServiceResponse obj);
        Task<List<DomainListProcModel>> GetDomainList();
        Task<DomainbyIdProcModel> GetDomainById(int domainId);
        Task<int> DeleteDomain(DeleteByIdModel deleteDomain);
        Task<int> UpdateDomain(UpdateDomainDto updateDomainDto, ServiceResponse obj);
        Task<DomainClientOpeningData> GetDomainClientOpeningData();
        Task<int> CreateClientDomainMapping(CreateClientDomainMapping createClientDomainMapping, ServiceResponse Obj);
        Task<List<DomainClientMappingListProcModel>> GetDomainClientMappingList();
        Task<int> DeleteClientDomain(DeleteByIdModel deleteClientDomain);
        Task<DomainClientMappingByClientIdData> GetDomainClientMappingByClientId(int clientId);
        Task<DomainClientMappingByClientIdData> GetDomainClientMappingByClientIdActive(int clientId);
        Task<int> UpdateClientDomainMapping(UpdateClientDomainMapping updateClientDomainMapping, ServiceResponse Obj);

    }
}

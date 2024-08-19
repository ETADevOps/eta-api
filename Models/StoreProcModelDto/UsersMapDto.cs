using ETA_API.Models.StoreProcModelDto;

namespace ETA.API.Models.StoreProcModelDto
{
    public class UsersMapDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string StartIp { get; set; }
        public string EndIp { get; set; }
        public string ClientName { get; set; }
        public string RoleName { get; set; }
        public Int16 Status { get; set; }
    }

    public class UsersByIdMapDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string StartIp { get; set; }
        public string EndIp { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int RoleId { get; set; }
        public int ProviderId { get; set; }
        public int PatientId { get; set; }
        public Int16 Status { get; set; }
    }
    public class CreateUserDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Subject { get; set; }
        public string StartIp { get; set; }
        public string EndIp { get; set; }
        public int ClientId { get; set; }
        public int ProviderId { get; set; }
        public int PatientId { get; set; }
        public int RoleId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Int16 Status { get; set; }
    }

    public class CreateUserSignInLog
    {
        public int UserId { get; set; }
        public DateTime? SignInDate { get; set; }
        public string IpAddress { get; set; }
        public string Location { get; set; }
        public string Action { get; set; }
    }

    public class UpdateUserSignInLogDto
    {
        public int UserId { get; set; }
        public Int16 Status { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class UserSignInLogDto
    {
        public int SignInLogId { get; set; }
        public string SignInDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string IpAddress { get; set; }
        public string Location { get; set; }
        public string Action { get; set; }
    }

    public class AuditMaster
    {
        public int audit_category_master_id { get; set; }
        public string audit_category_name { get; set; }
    }

    public class UserMaster
    {
        public int user_id { get; set; }
        public string username { get; set; }

    }

    public class PatientMaster
    {
        public int patient_id { get; set; }
        public string patient_name { get; set; }
    }

    public class ProviderMaster
    {
        public int provider_id { get; set; }
        public string provider_name { get; set; }
    }

    public class ReportMasterAudit
    {
        public int report_master_id { get; set; }
        public string report_name { get; set; }
    }

    public class ClientMasterAudit
    {
        public int client_id { get; set; }
        public string client_name { get; set; }
    }
    public class AuditOpeningData
    {
        public List<AuditMaster> AuditMaster { get; set; }
        public List<UserMaster> UserMaster { get; set; }
        public List<PatientMaster> PatientMaster { get; set; }
        public List<ProviderMaster> ProviderMaster { get; set; }
        public List<ReportMasterAudit> ReportMasterAudit { get; set; }
        public List<ClientMasterAudit> ClientMasterAudit { get; set; }
    }

    public class RecycleUserMaster
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string modified_date { get; set; }
        public string start_ip { get; set; }
        public string end_ip { get; set; }
    }

    public class RecyclePatientMaster
    {
        public int patient_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string patient_name { get; set; }
        public string gender { get; set; }
        public string date_of_birth { get; set; }
        public string address { get; set; }
        public string specimen_type { get; set; }
        public string collection_method { get; set; }
        public string collection_date { get; set; }
        public string provider_name { get; set; }
        public string sample_id { get; set; }
        public string user_name { get; set; }
        public string modified_date { get; set; }
    }

    public class RecycleProviderMaster
    {
        public int provider_id { get; set; }
        public string provider_name { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string user_name { get; set; }
        public string modified_date { get; set; }
    }

    public class RecycleClientMaster
    {
        public int client_id { get; set; }
        public string client_name { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string contact_person_name { get; set; }
        public string contact_person_phone { get; set; }
        public string contact_person_email { get; set; }
        public string user_name { get; set; }

        public string modified_date { get; set; }
    }

    public class RecycleBinData
    {
        public List<RecycleUserMaster> RecycleUserMaster { get; set; }
        public List<RecyclePatientMaster> RecyclePatientMaster { get; set; }
        public List<RecycleProviderMaster> RecycleProviderMaster { get; set; }
        public List<RecycleClientMaster> RecycleClientMaster { get; set; }

    }
    public class CreateAuditLog
    {
        public int UserId { get; set; }
        public int AuditCategoryMasterId { get; set; }
        public DateTime? AuditDate { get; set; }
        public string Activity { get; set; }
        public int PatientId { get; set; }
        public int ProviderId { get; set; }
        public int ReportId { get; set; }
        public int ClientId { get; set; }
        public int AuditUserId { get; set; }
        public Int16 Status { get; set; }
    }

    public class AuditLogDto
    {
        public int AuditLogId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string AuditDate { get; set; }
        public string Activity { get; set; }
        public string Status { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }
        public int ReportId { get; set; }
        public string ReportName { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string FromText { get; set; }
        public string ToText { get; set; }
    }

    public class RecycleBinOpeningData
    {
        public int CategoryId { get; set; }
        public string CategoryValue { get; set; }
    }

    public class UpdateRecycleBinDataModel
    {
        public int CategoryId { get; set; }
        public int ModifiedId { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class UserOpeningMaster
    {
        public int client_id { get; set; }
        public string client_name { get; set; }
    }

    public class RoleOpeningMaster
    {
        public int role_id { get; set; }
        public string role_name { get; set; }
    }

    public class UserOpeningData
    {
        public List<RoleOpeningMaster> RoleOpeningMaster { get; set; }
        public List<UserOpeningMaster> UserOpeningMaster { get; set; }

    }

    public class UserDetails
    {
        public int user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string user_name { get; set; }
        public int client_id { get; set; }
        public int provider_id { get; set; }
        public int patient_id { get; set; }

    }

    public class UserAccounts
    {
        public int account_id { get; set; }
        public string account_name { get; set; }
        public string account_logo { get; set; }

    }

    public class UserRoles
    {
        public string role_name { get; set; }
        public string permission_name { get; set; }
        public string associated_to { get; set; }
    }

    public class ClientMasterDetails
    {
        public int user_id { get; set; }
        public int client_id { get; set; }
        public string client_name { get; set; }
    }

    public class LoginUsersData
    {
        public UserDetails UserDetails { get; set; }
        public List<UserAccounts> UserAccounts { get; set; }
        public List<UserRoles> UserRoles { get; set; }
        public List<ClientMasterDetails> ClientMasterDetails { get; set; }
    }

    public class SecurityOpeningData
    {
        public List<SecurityQuestionMaster> SecurityQuestionMasters { get; set; }
    }

    public class SecurityQuestionMaster
    {
        public int question_id { get; set; }
        public string question { get; set; }
    }

    public class UpdateOtpDataModel
    {
        public string Email { get; set; }
        
    }

    public class VerifyOtpDataModel
    {
        public string Email { get; set; }
        public string Otp { get; set; }

    }

    public class UpdatePasswordDataModel
    {
        public int puser_id { get; set; }
        public string ppassword { get; set; }

    }

    public class ResetPasswordDataModel
    {
        public int puser_id { get; set; }
        //public string ppassword { get; set; }

    }

    public class UpdateSecurityAnswer
    {
        public int UserId { get; set; }
        public List<SecurityAnswerRelation> SecurityAnswerRelation { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }

    public class SecurityAnswerRelation
    {
        public int question_id { get; set; }
        public string security_answer { get; set; }
        
    }

    public class VerifySecurityAnswer
    {
        public int UserId { get; set; }
        public List<SecurityAnswerRelation> SecurityAnswerRelation { get; set; }

    }

    public class VerifyUserName
    {
        public int puser_id { get; set; }
        public string pemail { get; set; }

    }

    public class CreateDomainDto
    {
        public string DomainName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class DomainListDto
    {
        public int DomainMasterId { get; set; }
        public string DomainName { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public DateTime? CreatedDate { get; set; }

    }

    public class DomainbyIdDto
    {
        public int DomainMasterId { get; set; }
        public string DomainName { get; set; }
        public Int16 Status { get; set; }

    }

    public class UpdateDomainDto
    {
        public int DomainMasterId { get; set; }
        public string DomainName { get; set; }
        public Int16 Status { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class DomainClientOpeningData
    {
        public List<ClientData> ClientData { get; set; }
        public List<DomainData> DomainData { get; set; }
    }

    public class ClientData
    {
        public int client_id { get; set; }
        public string client_name { get; set; }
    }

    public class DomainData
    {
        public int domain_master_id { get; set; }
        public string domain_name { get; set; }
    }

    public class CreateClientDomainMapping
    {
        public int ClientId { get; set; }
        public List<ClientDomainRelation> ClientDomainRelation { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }

    public class ClientDomainRelation
    {
        public int domain_master_id { get; set; }
        public int domain_client_mapping_id { get; set; }

    }

    public class UpdateClientDomainMapping
    {
        public int ClientId { get; set; }
        public List<ClientDomainRelation> ClientDomainRelation { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Int16 Status { get; set; }


    }

    public class DomainClientMappingListDto
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string DomainNames { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string Status { get; set; }

    }

    public class ClientDomainMaster
    {
        public int domain_client_mapping_id { get; set; }
        public int domain_master_id { get; set; }
        public int client_id { get; set; }
        public string client_name { get; set; }
        public string domain_name { get; set; }
        public int status { get; set; }


    }

    public class DomainClientMappingByClientIdData
    {
        public List<ClientDomainMaster> ClientDomainMaster { get; set; }

    }
}
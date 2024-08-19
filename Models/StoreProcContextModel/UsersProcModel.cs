namespace ETA.API.Models.StoreProcContextModel
{
    public class UsersStoreProcModel
    {
        public int puser_id { get; set; }
        public string pfirst_name { get; set; }
        public string plast_name { get; set; }
        public string puser_name { get; set; }
        public string paddress { get; set; }
        public string pcity { get; set; }
        public string pstate { get; set; }
        public string pzip { get; set; }
        public string pphone { get; set; }
        public string pemail { get; set; }
        public string pstart_ip { get; set; }
        public string pend_ip { get; set; }
        public string pclient_name { get; set; }
        public string prole_name { get; set; }
        public Int16 pstatus { get; set; }
    }

    public class UsersByIdStoreProcModel
    {
        public int puser_id { get; set; }
        public string pfirst_name { get; set; }
        public string plast_name { get; set; }
        public string puser_name { get; set; }
        public string paddress { get; set; }
        public string pcity { get; set; }
        public string pstate { get; set; }
        public string pzip { get; set; }
        public string pphone { get; set; }
        public string pemail { get; set; }
        public string pstart_ip { get; set; }
        public string pend_ip { get; set; }
        public int pclient_id { get; set; }
        public string pclient_name { get; set; }
        public int prole_id { get; set; }
        public int pprovider_id { get; set; }
        public int ppatient_id { get; set; }
        public Int16 pstatus { get; set; }
    }

    public class UserSignInLogProcModel
    {
        public int psign_in_log_id { get; set; }
        public string psign_in_date { get; set; }
        public int p_user_id { get; set; }
        public string puser_name { get; set; }
        public string pemail { get; set; }
        public string pip_address { get; set; }
        public string plocation { get; set; }
        public string paction { get; set; }
    }

    public class CreateAuditLogProcModel
    {
        public int puser_id { get; set; }
        public int paudit_category_master_id { get; set; }
        public DateTime? paudit_date { get; set; }
        public string pactivity { get; set; }
        public int ppatient_id { get; set; }
        public int pprovider_id { get; set; }
        public int preport_id { get; set; }
        public Int16 pstatus { get; set; }

    }

    public class AuditLogProcModel
    {
        public int audit_log_id { get; set; }
        public int user_id { get; set; }
        public string username { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }
        public string audit_date { get; set; }
        public string activity { get; set; }
        public string status { get; set; }
        public int patient_id { get; set; }
        public string patient_name { get; set; }
        public int provider_id { get; set; }
        public string provider_name { get; set; }
        public int report_id { get; set; }
        public string report_name { get; set; }
        public int client_id { get; set; }
        public string client_name { get; set; }
        public string from_text { get; set; }
        public string to_text { get; set; }
    }

    public class RecycleBinOpeningDataProcModel
    {
        public int category_id { get; set; }
        public string category_value { get; set; }
    }

    public class UserAuthorizeReponseProcModel
    {
        public int UserId { get; set; }
        public string JWtToken { get; set; }
        public bool IsFirstTimeLogin { get; set; }
        public string RefreshToken { get; set; }
        public string Email { get; set; }

    }

    public class AuthorizeReponseProcModel
    {
        public int puser_id { get; set; }
        public bool pis_first_time_login { get; set; }
        public string pemail { get; set; }

    }

    public class DomainListProcModel
    {
        public int pdomain_master_id { get; set; }
        public string pdomain_name { get; set; }
        public string pstatus { get; set; }
        public string puser_name { get; set; }
        public DateTime? pcreated_date { get; set; }

    }

    public class DomainbyIdProcModel
    {
        public int pdomain_master_id { get; set; }
        public string pdomain_name { get; set; }
        public Int16 pstatus { get; set; }

    }

    public class DomainClientMappingListProcModel
    {
        public int pclient_id { get; set; }
        public string pclient_name { get; set; }
        public string pdomain_names { get; set; }
        public string pcreated_by { get; set; }
        public string pcreated_date { get; set; }
        public string pstatus { get; set; }

    }
}
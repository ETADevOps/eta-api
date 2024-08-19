using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Models.StoreProcContextModel
{
    public class AdminProcModel
    {
        public int ppermission_id { get; set; }
        public string ppermission_name { get; set; }
        public string pdescription { get; set; }
        public Int16 pstatus { get; set; }
    }

    public class PermissionByIdProcModel
    {
        public int ppermission_id { get; set; }
        public string ppermission_name { get; set; }
        public string pdescription { get; set; }
        public Int16 pstatus { get; set; }
    }

    public class RoleListProcModel
    {
        public int prole_id { get; set; }
        public string prole_name { get; set; }
        public string pdescription { get; set; }
        public string pstatus { get; set; }
        public string pcreated_by { get; set; }
        public string pmodified_by { get; set; }
        public int active_users { get; set; }
        public int inactive_users { get; set; }


    }

    
}

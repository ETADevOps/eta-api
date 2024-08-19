namespace ETA_API.Models.StoreProcModelDto
{
    public class AdminDto
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public Int16 Status { get; set; }
    }

    public class PermissionByIdDto
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public Int16 Status { get; set; }
    }

    public class RoleListDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int ActiveUsers { get; set; }
        public int InactiveUsers { get; set; }
    }

    public class RoleByIdDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public Int16 Status { get; set; }
    }

    public class CreateRoleDto
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class UpdateRoleDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public Int16 Status { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class CreatePermissionDto
    {
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class UpdatePermissionDto
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public Int16 Status { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class PermissionAdminMaster
    {
        public int permission_id { get; set; }
        public string permission_name { get; set; }
    }

    public class RoleOpeningData
    {
        public List<PermissionAdminMaster> PermissionAdminMasters { get; set; }
    }

    public class CreateRole
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
        public List<RolePermissionRelation> RolePermissionRelations { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }

    public class RolePermissionRelation
    {
        public int role_permission_id { get; set; }
        public int permission_id { get; set; }
    }

    public class UpdateRole
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public Int16 Status { get; set; }
        public List<RolePermissionRelation> RolePermissionRelations { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }

    public class RoleByIdData
    {
        public RoleByIdMasterData RoleByIdMasterData { get; set; }
        public List<PermissionByIdMasterData> PermissionByIdMasterData { get; set; }

    }

    public class RoleByIdMasterData
    {
        public int role_id { get; set; }
        public string role_name { get; set; }
        public string description { get; set; }
        public Int16 status { get; set; }
    }

    public class PermissionByIdMasterData
    {
        public int role_permission_id { get; set; }
        public int role_id { get; set; }
        public int permission_id { get; set; }
        public string permission_name { get; set; }
    }
}

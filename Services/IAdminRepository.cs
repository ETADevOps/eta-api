using ETA.API.Models.StoreProcContextModel;
using ETA_API.Models.Referance;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Services
{
    public interface IAdminRepository
    {
        Task<RoleOpeningData> GetRoleOpeningData();
        Task<List<RoleListProcModel>> GetRoleList();
        Task<RoleByIdData> GetRoleById(int roleId);
        Task<int> CreateRole(CreateRole createRole, ServiceResponse Obj);
        Task<int> UpdateRole(UpdateRole updateRole, ServiceResponse Obj);
        Task<int> DeleteRole(DeleteByIdModel deleteRole);

    }
}

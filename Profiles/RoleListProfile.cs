using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class RoleListProfile : Profile
    {
        public RoleListProfile()
        {
            CreateMap<RoleListDto, RoleListProcModel>()
           .ForMember(a => a.prole_id, b => b.MapFrom(c => c.RoleId))
           .ForMember(a => a.prole_name, b => b.MapFrom(c => c.RoleName))
           .ForMember(a => a.pdescription, b => b.MapFrom(c => c.Description))
           .ForMember(a => a.pstatus, b => b.MapFrom(c => c.Status))
           .ForMember(a => a.pcreated_by, b => b.MapFrom(c => c.CreatedBy))
           .ForMember(a => a.pmodified_by, b => b.MapFrom(c => c.ModifiedBy))
           .ForMember(a => a.active_users, b => b.MapFrom(c => c.ActiveUsers))
           .ForMember(a => a.inactive_users, b => b.MapFrom(c => c.InactiveUsers))

           .ReverseMap();
        }
    }
}

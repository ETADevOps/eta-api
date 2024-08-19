using AutoMapper;
using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;

namespace ETA.API.UsersProfile
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<UsersMapDto, UsersStoreProcModel>()
            .ForMember(a => a.puser_id, b => b.MapFrom(c => c.UserId))
            .ForMember(a => a.pfirst_name, b => b.MapFrom(c => c.FirstName))
            .ForMember(a => a.plast_name, b => b.MapFrom(c => c.LastName))
            .ForMember(a => a.puser_name, b => b.MapFrom(c => c.UserName))
            .ForMember(a => a.paddress, b => b.MapFrom(c => c.Address))
            .ForMember(a => a.pcity, b => b.MapFrom(c => c.City))
            .ForMember(a => a.pstate, b => b.MapFrom(c => c.State))
            .ForMember(a => a.pzip, b => b.MapFrom(c => c.Zip))
            .ForMember(a => a.pphone, b => b.MapFrom(c => c.Phone))
            .ForMember(a => a.pemail, b => b.MapFrom(c => c.Email))
            .ForMember(a => a.pstart_ip, b => b.MapFrom(c => c.StartIp))
            .ForMember(a => a.pend_ip, b => b.MapFrom(c => c.EndIp))
            .ForMember(a => a.pclient_name, b => b.MapFrom(c => c.ClientName))
            .ForMember(a => a.prole_name, b => b.MapFrom(c => c.RoleName))
            .ForMember(a => a.pstatus, b => b.MapFrom(c => c.Status))
            .ReverseMap();
        }
    }
}

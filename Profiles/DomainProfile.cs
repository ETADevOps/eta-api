using AutoMapper;
using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<DomainListDto, DomainListProcModel>()
           .ForMember(a => a.pdomain_master_id, b => b.MapFrom(c => c.DomainMasterId))
           .ForMember(a => a.pdomain_name, b => b.MapFrom(c => c.DomainName))
           .ForMember(a => a.pstatus, b => b.MapFrom(c => c.Status))
           .ForMember(a => a.puser_name, b => b.MapFrom(c => c.UserName))
           .ForMember(a => a.pcreated_date, b => b.MapFrom(c => c.CreatedDate))

           .ReverseMap();
        }
    }
}

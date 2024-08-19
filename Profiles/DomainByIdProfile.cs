using AutoMapper;
using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class DomainByIdProfile : Profile
    {
        public DomainByIdProfile()
        {
            CreateMap<DomainbyIdDto, DomainbyIdProcModel>()
           .ForMember(a => a.pdomain_master_id, b => b.MapFrom(c => c.DomainMasterId))
           .ForMember(a => a.pdomain_name, b => b.MapFrom(c => c.DomainName))
           .ForMember(a => a.pstatus, b => b.MapFrom(c => c.Status))

           .ReverseMap();
        }
    }
}

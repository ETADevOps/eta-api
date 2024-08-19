using AutoMapper;
using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class DomainClientMappingList : Profile
    {
        public DomainClientMappingList()
        {
            CreateMap<DomainClientMappingListDto, DomainClientMappingListProcModel>()
           .ForMember(a => a.pclient_id, b => b.MapFrom(c => c.ClientId))
           .ForMember(a => a.pclient_name, b => b.MapFrom(c => c.ClientName))
           .ForMember(a => a.pdomain_names, b => b.MapFrom(c => c.DomainNames))
           .ForMember(a => a.pcreated_by, b => b.MapFrom(c => c.CreatedBy))
           .ForMember(a => a.pcreated_date, b => b.MapFrom(c => c.CreatedDate))
           .ForMember(a => a.pstatus, b => b.MapFrom(c => c.Status))

           .ReverseMap();
        }
    }
}

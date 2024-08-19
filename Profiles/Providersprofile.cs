using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class Providersprofile : Profile
    {
        public Providersprofile()
        {
            CreateMap<ProvidersDto, ProvidersProcModel>()
            .ForMember(a => a.pprovider_id, b => b.MapFrom(c => c.ProviderId))
            .ForMember(a => a.pprovider_name, b => b.MapFrom(c => c.Providername))
            .ForMember(a => a.paddress, b => b.MapFrom(c => c.Address))
            .ForMember(a => a.pcity, b => b.MapFrom(c => c.City))
            .ForMember(a => a.pstate, b => b.MapFrom(c => c.State))
            .ForMember(a => a.pzip, b => b.MapFrom(c => c.Zip))
            .ForMember(a => a.pphone, b => b.MapFrom(c => c.Phone))
            .ForMember(a => a.pemail, b => b.MapFrom(c => c.Email))
            .ForMember(a => a.pclient_id, b => b.MapFrom(c => c.ClientId))
            .ForMember(a => a.pclient_name, b => b.MapFrom(c => c.ClientName))
            .ForMember(a => a.pstatus, b => b.MapFrom(c => c.Status))
            .ReverseMap();
        }
    }
}

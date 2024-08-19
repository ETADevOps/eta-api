using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class ProviderListByClientIdProfile : Profile
    {
        public ProviderListByClientIdProfile()
        {
            CreateMap<ProviderListByClientId, ProviderListByClientIdProcModel>()
            .ForMember(a => a.pprovider_id, b => b.MapFrom(c => c.ProviderId))
            .ForMember(a => a.pprovider_name, b => b.MapFrom(c => c.ProviderName))
            .ReverseMap();
        }
    }
}

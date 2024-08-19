using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {

            CreateMap<ClientDto, ClientProcModel>()
            .ForMember(a => a.pclient_id, b => b.MapFrom(c => c.ClientId))
            .ForMember(a => a.pclient_name, b => b.MapFrom(c => c.ClientName))
            .ForMember(a => a.pclient_logo, b => b.MapFrom(c => c.ClientLogo))
            .ForMember(a => a.paddress, b => b.MapFrom(c => c.Address))
            .ForMember(a => a.pcity, b => b.MapFrom(c => c.City))
            .ForMember(a => a.pstate, b => b.MapFrom(c => c.State))
            .ForMember(a => a.pzip, b => b.MapFrom(c => c.Zip))
            .ForMember(a => a.pphone, b => b.MapFrom(c => c.Phone))
            .ForMember(a => a.pfax, b => b.MapFrom(c => c.Fax))
            .ForMember(a => a.pcontact_person_name, b => b.MapFrom(c => c.ContactPersonName))
            .ForMember(a => a.ptimezone, b => b.MapFrom(c => c.TimeZone))
            .ForMember(a => a.pdomains, b => b.MapFrom(c => c.Domains))
            .ReverseMap();
        }
    }
}

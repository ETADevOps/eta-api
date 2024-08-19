using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class PatientListByProviderIdProfile : Profile
    {
        public PatientListByProviderIdProfile()
        {
            CreateMap<PatientListByProviderIdData, PatientListByProviderIdProcModel>()
            .ForMember(a => a.ppatient_id, b => b.MapFrom(c => c.PatientId))
            .ForMember(a => a.pfirst_name, b => b.MapFrom(c => c.FirstName))
            .ForMember(a => a.plast_name, b => b.MapFrom(c => c.LastName))
            .ReverseMap();
        }
    }
}

using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class GeneDumpProfile : Profile
    {
        public GeneDumpProfile()
        {
            CreateMap<PatientGeneDumpModel, PatientGeneDumpProcModel>()
           .ForMember(a => a.psample_id, b => b.MapFrom(c => c.SampleId))
           .ForMember(a => a.psnp_name, b => b.MapFrom(c => c.SnpName))
           .ForMember(a => a.pallele1, b => b.MapFrom(c => c.Allele1))
           .ForMember(a => a.pallele2, b => b.MapFrom(c => c.Allele2))
           .ReverseMap();

        }
    }
}

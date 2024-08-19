using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class GeneSnpOpeningDataProfile : Profile
    {
        public GeneSnpOpeningDataProfile()
        {
            CreateMap<CategoryGeneOpeningDataDto, CategoryGeneOpeningDataProcModel>()
        .ForMember(a => a.pcategory_id, b => b.MapFrom(c => c.CategoryId))
        .ForMember(a => a.pcategory_name, b => b.MapFrom(c => c.CategoryName))

        .ReverseMap();
        }
    }
}

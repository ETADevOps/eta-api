using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class SubCategoryOpeningDataProfile : Profile
    {
        public SubCategoryOpeningDataProfile()
        {
            CreateMap<SubCategoryOpeningDataDto, SubCategoryOpeningDataProcModel>()
            .ForMember(a => a.pcategory_id, b => b.MapFrom(c => c.CategoryId))
            .ForMember(a => a.pcategory_name, b => b.MapFrom(c => c.CategoryName))

            .ReverseMap();
        }
    }
}

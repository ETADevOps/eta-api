using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class CategoryOpeningDataProfile : Profile
    {
        public CategoryOpeningDataProfile()
        {
            CreateMap<CategoryOpeningDataDto, CategoryOpeningDataProcModel>()
            .ForMember(a => a.category_id, b => b.MapFrom(c => c.CategoryId))
            .ForMember(a => a.category_name, b => b.MapFrom(c => c.CategoryName))
            
            .ReverseMap();
        }
    }
}

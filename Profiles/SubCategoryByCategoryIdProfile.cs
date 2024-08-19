using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class SubCategoryByCategoryIdProfile : Profile
    {
        public SubCategoryByCategoryIdProfile()
        {
            CreateMap<SubCategoryByCategoryIdDto, SubCategoryByCategoryIdProcModel>()
           .ForMember(a => a.psub_category_id, b => b.MapFrom(c => c.SubCategoryId))
           .ForMember(a => a.psub_category_name, b => b.MapFrom(c => c.SubCategoryName))

           .ReverseMap();
        }
    }
}

using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class SubCategoryByIdProfile : Profile
    {
        public SubCategoryByIdProfile()
        {
            CreateMap<SubCategoryByIdDto, SubCategoryByIdProcModel>()
            .ForMember(a => a.psub_category_by_id, b => b.MapFrom(c => c.SubCategoryId))
            .ForMember(a => a.pcategory_id, b => b.MapFrom(c => c.CategoryId))
            .ForMember(a => a.psub_category_name, b => b.MapFrom(c => c.SubCategoryName))
            .ForMember(a => a.psub_category_introduction, b => b.MapFrom(c => c.SubCategoryIntroduction))
            .ForMember(a => a.pstatus, b => b.MapFrom(c => c.Status))

            .ReverseMap();
        }
    }
}

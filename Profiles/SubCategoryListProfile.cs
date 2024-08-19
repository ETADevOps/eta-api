using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class SubCategoryListProfile : Profile
    {
        public SubCategoryListProfile()
        {
            CreateMap<SubCategoryListDto, SubCategoryListProcModel>()
            .ForMember(a => a.psub_category_by_id, b => b.MapFrom(c => c.SubCategoryById))
            .ForMember(a => a.preport_name, b => b.MapFrom(c => c.ReportName))
            .ForMember(a => a.pcategory_name, b => b.MapFrom(c => c.CategoryName))
            .ForMember(a => a.psub_category_name, b => b.MapFrom(c => c.SubCategoryName))
            .ForMember(a => a.psub_category_introduction, b => b.MapFrom(c => c.SubCategoryIntroduction))
            .ForMember(a => a.pstatus, b => b.MapFrom(c => c.Status))

            .ReverseMap();
        }
    }
}

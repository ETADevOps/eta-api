using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class ReportCategoryListProfile : Profile
    {
        public ReportCategoryListProfile()
        {
            CreateMap<ReportCategoryListDto, ReportCategoryListProcModel>()
            .ForMember(a => a.pcategory_id, b => b.MapFrom(c => c.CategoryId))
            .ForMember(a => a.pcategory_name, b => b.MapFrom(c => c.CategoryName))
            .ForMember(a => a.pcategory_introduction, b => b.MapFrom(c => c.CategoryIntroduction))
            .ForMember(a => a.preport_name, b => b.MapFrom(c => c.ReportName))
            .ForMember(a => a.pstatus, b => b.MapFrom(c => c.Status))

            .ReverseMap();
        }
    }
}

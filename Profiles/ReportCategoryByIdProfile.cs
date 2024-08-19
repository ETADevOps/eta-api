using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class ReportCategoryByIdProfile : Profile
    {
        public ReportCategoryByIdProfile()
        {
            CreateMap<ReportCategoryByIdDto, ReportCategoryByIdProcModel>()
            .ForMember(a => a.pcategory_id, b => b.MapFrom(c => c.CategoryId))
            .ForMember(a => a.preport_master_id, b => b.MapFrom(c => c.ReportMasterId))
            .ForMember(a => a.pcategory_name, b => b.MapFrom(c => c.CategoryName))
            .ForMember(a => a.pcategory_introduction, b => b.MapFrom(c => c.CategoryIntroduction))
            .ForMember(a => a.pstatus, b => b.MapFrom(c => c.Status))

            .ReverseMap();
        }
    }
}

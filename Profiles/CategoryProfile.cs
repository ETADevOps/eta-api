using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile() 
        {
            CreateMap<CategoryMapDto, CategoryProcModel>()
            .ForMember(a => a.preport_master_id, b => b.MapFrom(c => c.ReportMasterId))
            .ForMember(a => a.preport_name, b => b.MapFrom(c => c.ReportName))
            .ForMember(a => a.preport_gene_predisposition, b => b.MapFrom(c => c.ReportGenePredisposition))
            .ForMember(a => a.preport_important_takeways, b => b.MapFrom(c => c.ReportImportantTakeways))
            .ForMember(a => a.pstatus, b => b.MapFrom(c => c.Status))

            .ReverseMap();
        }
    }
}

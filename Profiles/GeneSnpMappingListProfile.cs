using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class GeneSnpMappingListProfile : Profile
    {
        public GeneSnpMappingListProfile()
        {
            CreateMap<CategoryGeneMappingListDto, CategoryGeneMappingListProcModel>()
            .ForMember(a => a.pcategory_gene_mapping_id, b => b.MapFrom(c => c.CategoryGeneMappingId))
            .ForMember(a => a.preport_name, b => b.MapFrom(c => c.ReportName))
            .ForMember(a => a.pcategory_name, b => b.MapFrom(c => c.CategoryName))
            .ForMember(a => a.psub_category_name, b => b.MapFrom(c => c.SubCategoryName))
            .ForMember(a => a.pgene, b => b.MapFrom(c => c.Gene))
            .ForMember(a => a.pgene_description, b => b.MapFrom(c => c.GeneDescription))
            .ForMember(a => a.pstatus, b => b.MapFrom(c => c.Status))

            .ReverseMap();
        }
    }
}

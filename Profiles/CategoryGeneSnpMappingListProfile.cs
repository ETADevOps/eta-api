using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class CategoryGeneSnpMappingListProfile : Profile
    {
        public CategoryGeneSnpMappingListProfile()
        {
            CreateMap<CategoryGeneSnpMappingListDto, CategoryGeneSnpMappingListProcModel>()
            .ForMember(a => a.pcategory_gene_snp_mapping_id, b => b.MapFrom(c => c.CategoryGeneSnpMappingId))
            .ForMember(a => a.preport_name, b => b.MapFrom(c => c.ReportName))
            .ForMember(a => a.pcategory_name, b => b.MapFrom(c => c.CategoryName))
            .ForMember(a => a.psub_category_name, b => b.MapFrom(c => c.SubCategoryName))
            .ForMember(a => a.pgene, b => b.MapFrom(c => c.Gene))
            .ForMember(a => a.psnp, b => b.MapFrom(c => c.Snp))
            .ForMember(a => a.pstatus, b => b.MapFrom(c => c.Status))

            .ReverseMap();
        }
    }
}

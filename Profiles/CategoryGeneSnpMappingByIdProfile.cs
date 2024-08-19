using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class CategoryGeneSnpMappingByIdProfile : Profile
    {
        public CategoryGeneSnpMappingByIdProfile()
        {
            CreateMap<CategoryGeneSnpMappingByIdDto, CategoryGeneSnpMappingByIdProcModel>()
            .ForMember(a => a.pcategory_gene_snp_mapping_id, b => b.MapFrom(c => c.CategoryGeneSnpMappingId))
            .ForMember(a => a.pcategory_gene_mapping_id, b => b.MapFrom(c => c.CategoryGeneMappingId))
            .ForMember(a => a.psnp, b => b.MapFrom(c => c.Snp))
            .ForMember(a => a.pstatus, b => b.MapFrom(c => c.Status))

            .ReverseMap();
        }
    }
}

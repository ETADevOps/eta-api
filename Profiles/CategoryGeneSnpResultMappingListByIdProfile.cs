using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class CategoryGeneSnpResultMappingListByIdProfile : Profile
    {
        public CategoryGeneSnpResultMappingListByIdProfile()
        {
            CreateMap<CategoryGeneSnpResultMappingByIdDto, CategoryGeneSnpResultMappingByIdProcModel>()
            .ForMember(a => a.pcategory_gene_snps_result_mapping_id, b => b.MapFrom(c => c.CategoryGeneSnpResultMappingId))
            .ForMember(a => a.pcategory_gene_snps_mapping_id, b => b.MapFrom(c => c.CategoryGeneSnpMappingId))
            .ForMember(a => a.pgenotype, b => b.MapFrom(c => c.Genotype))
            .ForMember(a => a.pgenotype_result, b => b.MapFrom(c => c.GenotypeResult))
            .ForMember(a => a.pstudy_name, b => b.MapFrom(c => c.StudyName))
            .ForMember(a => a.pstudy_description, b => b.MapFrom(c => c.StudyDescription))
            .ForMember(a => a.pstudy_link, b => b.MapFrom(c => c.StudyLink))
            .ForMember(a => a.pstatus, b => b.MapFrom(c => c.Status))

            .ReverseMap();
        }
    }
}

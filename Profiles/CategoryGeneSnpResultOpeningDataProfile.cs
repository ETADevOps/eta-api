using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;


namespace ETA_API.Profiles
{
    public class CategoryGeneSnpResultOpeningDataProfile : Profile
    {
        public CategoryGeneSnpResultOpeningDataProfile()
        {
            CreateMap<CategoryGeneSnpResultOpeningDataDto, CategoryGeneSnpResultOpeningDataProcModel>()
            .ForMember(a => a.pcategory_gene_snps_mapping_id, b => b.MapFrom(c => c.CategoryGeneSnpsMappingId))
            .ForMember(a => a.pcategory_name, b => b.MapFrom(c => c.CategoryName))
            .ForMember(a => a.psub_category_name, b => b.MapFrom(c => c.SubCategoryName))
            .ForMember(a => a.pgene, b => b.MapFrom(c => c.Gene))
            .ForMember(a => a.psnp, b => b.MapFrom(c => c.Snp))
            .ReverseMap();
        }
    }
}

using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class CategoryGeneSnpResultMappingListProfile : Profile
    {
        public CategoryGeneSnpResultMappingListProfile()
        {
            CreateMap<CategoryGeneSnpResultMappingListDto, CategoryGeneSnpResultMappingListProcModel>()
           .ForMember(a => a.pcategory_gene_snps_result_mapping_id, b => b.MapFrom(c => c.CategoryGeneSnpResultMappingId))
           .ForMember(a => a.preport_name, b => b.MapFrom(c => c.ReportName))
           .ForMember(a => a.pcategory_name, b => b.MapFrom(c => c.CategoryName))
           .ForMember(a => a.psub_category_name, b => b.MapFrom(c => c.SubCategoryName))
           .ForMember(a => a.pgene, b => b.MapFrom(c => c.Gene))
           .ForMember(a => a.psnp, b => b.MapFrom(c => c.Snp))
           .ForMember(a => a.pgenotype, b => b.MapFrom(c => c.GenoType))
           .ForMember(a => a.pgenotype_result, b => b.MapFrom(c => c.GenoTypeResult))
           .ForMember(a => a.pstudy_name, b => b.MapFrom(c => c.StudyName))
           .ForMember(a => a.pstudy_description, b => b.MapFrom(c => c.StudyDescription))
           .ForMember(a => a.pstudy_link, b => b.MapFrom(c => c.Studylink))
           .ForMember(a => a.pstatus, b => b.MapFrom(c => c.Status))

           .ReverseMap();
        }
    }
}

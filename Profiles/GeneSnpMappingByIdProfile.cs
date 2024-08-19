using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class GeneSnpMappingByIdProfile : Profile
    {
        public GeneSnpMappingByIdProfile()
        {
            CreateMap<CatgeoryGeneMappingByIdDto, CategoryGeneMappingByIdProcModel>()
           .ForMember(a => a.pcategory_gene_mapping_id, b => b.MapFrom(c => c.CategoryGeneMappingId))
           .ForMember(a => a.pcategory_id, b => b.MapFrom(c => c.CategoryId))
           .ForMember(a => a.psub_category_id, b => b.MapFrom(c => c.SubCategoryId))
           .ForMember(a => a.pgene, b => b.MapFrom(c => c.Gene))
           .ForMember(a => a.pgene_image, b => b.MapFrom(c => c.GeneImage))
           .ForMember(a => a.pgene_description, b => b.MapFrom(c => c.GeneDescription))
           .ForMember(a => a.pstatus, b => b.MapFrom(c => c.Status))


           .ReverseMap();
        }
    }
}

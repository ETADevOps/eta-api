namespace ETA_API.Models.StoreProcModelDto
{
    public class CategoryMapDto
    {
        public int ReportMasterId { get; set; }
        public string ReportName { get; set; }
        public string ReportGenePredisposition { get; set; }
        public string ReportImportantTakeways { get; set; }
        public Int16 Status { get; set; }


    }

    public class CreateCategoryDto
    {
        public string CategoryName { get; set; }
        public string ReportGenePredisposition { get; set; }
        public string ReportImportantTakeways { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class UpdateCategoryDto
    {
        public int ReportMasterId { get; set; }
        public string CategoryName { get; set; }
        public string ReportGenePredisposition { get; set; }
        public string ReportImportantTakeways { get; set; }
        public Int16 Status { get; set; }	
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class CategoryOpeningDataDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public class SubCategoryOpeningDataDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public class CreateReportCategoryDto
    {
        public string CategoryName { get; set; }
        public string CategoryIntroduction { get; set; }
        public int ReportMasterId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class ReportCategoryListDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryIntroduction { get; set; }
        public string ReportName { get; set; }
        public Int16 Status { get; set; }

    }

    public class ReportCategoryByIdDto
    {
        public int CategoryId { get; set; }
        public int ReportMasterId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryIntroduction { get; set; }
        public Int16 Status { get; set; }

    }

    public class UpdateReportCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public int ReportMasterId { get; set; }
        public Int16 Status { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class CreateSubCategoryDto
    {
        public string SubCategoryName { get; set; }
        public string SubCategoryIntroduction { get; set; }
        public int CategoryId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class UpdateSubCategoryDto
    {
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryIntroduction { get; set; }
        public int CategoryId { get; set; }
        public Int16 Status { get; set; }	
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class SubCategoryByIdDto
    {
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryIntroduction { get; set; }
        public Int16 Status { get; set; }

    }

    public class SubCategoryListDto
    {
        public int SubCategoryById { get; set; }
        public string ReportName { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryIntroduction { get; set; }
        public Int16 Status { get; set; }

    }

    public class CategoryGeneOpeningDataDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public class SubCategoryByCategoryIdDto
    {
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
    }

    public class CategoryGeneMappingListDto
    {
        public int CategoryGeneMappingId { get; set; }
        public string ReportName { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string Gene { get; set; }
        public string GeneDescription { get; set; }
        public Int16 Status { get; set; }
    }

    public class CatgeoryGeneMappingByIdDto
    {
        public int CategoryGeneMappingId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string Gene { get; set; }
        public string GeneImage { get; set; }
        public string GeneDescription { get; set; }
        public Int16 Status { get; set; }


    }

    public class CreateCategoryGeneMappingDto
    {
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string Gene { get; set; }
        public string GeneDescription { get; set; }
        public string GeneImage { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class UpdateCategoryGeneMappingByIdDto
    {
        public int CategoryGeneMappingId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string Gene { get; set; }
        public string GeneDescription { get; set; }
        public string GeneImage { get; set; }
        public Int16 Status { get; set; }	
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class CategoryGeneSnpOpeningDataDto
    {
        public int CategoryGeneMappingId { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string Gene { get; set; }

    }

    public class CategoryGeneSnpMappingListDto
    {
        public int CategoryGeneSnpMappingId { get; set; }
        public string ReportName { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string Gene { get; set; }
        public string Snp { get; set; }
        public Int16 Status { get; set; }

    }

    public class CategoryGeneSnpMappingByIdDto
    {
        public int CategoryGeneSnpMappingId { get; set; }
        public int CategoryGeneMappingId { get; set; }
        public string Snp { get; set; }
        public Int16 Status { get; set; }


    }

    public class CreateCategoryGeneSnpMappingDto
    {
        public int CategoryGeneSnpMappingId { get; set; }
        public int CategoryGeneMappingId { get; set; }
        public string Snp { get; set; }
        public Int16 Status { get; set; }	
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class UpdateCategoryGeneSnpMappingDto
    {
        public int CategoryGeneSnpMappingId { get; set; }
        public int CategoryGeneMappingId { get; set; }
        public string Snp { get; set; }
        public Int16 Status { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class CategoryGeneSnpResultOpeningDataDto
    {
        public int CategoryGeneSnpsMappingId { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string Gene { get; set; }
        public string Snp { get; set; }

    }

    public class CategoryGeneSnpResultMappingListDto
    {
        public int CategoryGeneSnpResultMappingId { get; set; }
        public string ReportName { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string Gene { get; set; }
        public string Snp { get; set; }
        public string GenoType { get; set; }
        public string GenoTypeResult { get; set; }
        public string StudyName { get; set; }
        public string StudyDescription { get; set; }
        public string Studylink { get; set; }
        public Int16 Status { get; set; }

    }

    public class CreateCategoryGeneSnpResult
    {
        public List<CategoryGeneSnpResultRelation> CategoryGeneSnpResultRelation { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }

    public class CategoryGeneSnpResultRelation
    {
        public int pcategory_gene_snps_mapping_id { get; set; }
        public string pgenotype { get; set; }
        public string pgenotype_result { get; set; }
        public string pstudy_name { get; set; }
        public string pstudy_description { get; set; }
        public string pstudy_link { get; set; }
    }

    public class UpdateCategoryGeneSnpResultDto
    {
        public int CategoryGeneSnpResultMappingId { get; set; }
        public int CategoryGeneSnpMappingId { get; set; }
        public string GenoType { get; set; }
        public string GenoTypeResult { get; set; }
        public string StudyName { get; set; }
        public string StudyDescription { get; set; }
        public string StudyLink { get; set; }
        public Int16 Status { get; set; }	
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class CategoryGeneSnpResultMappingByIdDto
    {
        public int CategoryGeneSnpResultMappingId { get; set; }
        public int CategoryGeneSnpMappingId { get; set; }
        public string Genotype { get; set; }
        public string GenotypeResult { get; set; }
        public string StudyName { get; set; }
        public string StudyDescription { get; set; }
        public string StudyLink { get; set; }
        public Int16 Status { get; set; }

    }
}

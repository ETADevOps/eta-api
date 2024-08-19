namespace ETA_API.Models.StoreProcContextModel
{
    public class CategoryProcModel
    {
        public int preport_master_id { get; set; }
        public string preport_name { get; set; }
        public string preport_gene_predisposition { get; set; }
        public string preport_important_takeways { get; set; }
        public Int16 pstatus { get; set; }

    }

    public class CategoryOpeningDataProcModel
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
    }

    public class SubCategoryOpeningDataProcModel
    {
        public int pcategory_id { get; set; }
        public string pcategory_name { get; set; }
    }
    public class ReportCategoryListProcModel
    {
        public int pcategory_id { get; set; }
        public string pcategory_name { get; set; }
        public string pcategory_introduction { get; set; }
        public string preport_name { get; set; }
        public Int16 pstatus { get; set; }
    }

    public class ReportCategoryByIdProcModel
    {
        public int pcategory_id { get; set; }
        public int preport_master_id { get; set; }
        public string pcategory_name { get; set; }
        public string pcategory_introduction { get; set; }
        public Int16 pstatus { get; set; }

    }
    public class SubCategoryByIdProcModel
    {
        public int psub_category_by_id { get; set; }
        public int pcategory_id { get; set; }
        public string psub_category_name { get; set; }
        public string psub_category_introduction { get; set; }
        public Int16 pstatus { get; set; }

    }

    public class SubCategoryListProcModel
    {
        public int psub_category_by_id { get; set; }
        public string preport_name { get; set; }
        public string pcategory_name { get; set; }
        public string psub_category_name { get; set; }
        public string psub_category_introduction { get; set; }
        public Int16 pstatus { get; set; }

    }

    public class CategoryGeneOpeningDataProcModel
    {
        public int pcategory_id { get; set; }
        public string pcategory_name { get; set; }
    }

    public class SubCategoryByCategoryIdProcModel
    {
        public int psub_category_id { get; set; }
        public string psub_category_name { get; set; }
    }

    public class CategoryGeneMappingListProcModel
    {
        public int pcategory_gene_mapping_id { get; set; }
        public string preport_name { get; set; }
        public string pcategory_name { get; set; }
        public string psub_category_name { get; set; }
        public string pgene { get; set; }
        public string pgene_description { get; set; }
        public Int16 pstatus { get; set; }
    }

    public class CategoryGeneMappingByIdProcModel
    {
        public int pcategory_gene_mapping_id { get; set; }
        public int pcategory_id { get; set; }
        public int psub_category_id { get; set; }
        public string pgene { get; set; }
        public string pgene_image { get; set; }
        public string pgene_description { get; set; }
        public Int16 pstatus { get; set; }

    }

    public class CategoryGeneSnpOpeningDataProcModel
    {
        public int pcategory_gene_mapping_id { get; set; }
        public string pcategory_name { get; set; }
        public string psub_category_name { get; set; }
        public string pgene { get; set; }
    }

    public class CategoryGeneSnpMappingListProcModel
    {
        public int pcategory_gene_snp_mapping_id { get; set; }
        public string preport_name { get; set; }
        public string pcategory_name { get; set; }
        public string psub_category_name { get; set; }
        public string pgene { get; set; }
        public string psnp { get; set; }
        public Int16 pstatus { get; set; }

    }

    public class CategoryGeneSnpMappingByIdProcModel
    {
        public int pcategory_gene_snp_mapping_id { get; set; }
        public int pcategory_gene_mapping_id { get; set; }
        public string psnp { get; set; }
        public Int16 pstatus { get; set; }


    }

    public class CategoryGeneSnpResultOpeningDataProcModel
    {
        public int pcategory_gene_snps_mapping_id { get; set; }
        public string pcategory_name { get; set; }
        public string psub_category_name { get; set; }
        public string pgene { get; set; }
        public string psnp { get; set; }
    }

    public class CategoryGeneSnpResultMappingListProcModel
    {
        public int pcategory_gene_snps_result_mapping_id { get; set; }
        public string preport_name { get; set; }
        public string pcategory_name { get; set; }
        public string psub_category_name { get; set; }
        public string pgene { get; set; }
        public string psnp { get; set; }
        public string pgenotype { get; set; }
        public string pgenotype_result { get; set; }
        public string pstudy_name { get; set; }
        public string pstudy_description { get; set; }
        public string pstudy_link { get; set; }
        public Int16 pstatus { get; set; }

    }

    public class CategoryGeneSnpResultMappingByIdProcModel
    {
        public int pcategory_gene_snps_result_mapping_id { get; set; }
        public int pcategory_gene_snps_mapping_id { get; set; }
        public string pgenotype { get; set; }
        public string pgenotype_result { get; set; }
        public string pstudy_name { get; set; }
        public string pstudy_description { get; set; }
        public string pstudy_link { get; set; }
        public Int16 pstatus { get; set; }


    }
}

namespace ETA_API.Models.StoreProcContextModel
{
    public class PatientsProcModel
    {
        public int ppatient_id { get; set; }
        public string pfirst_name { get; set; }
        public string plast_name { get; set; }
        public string pgender { get; set; }
        public string pdate_of_birth { get; set; }
        public string paddress { get; set; }
        public string pcity { get; set; }
        public string pstate { get; set; }
        public string pzip { get; set; }
        public string pemail { get; set; }
        public string pspecimen_type { get; set; }
        public string pcollection_method { get; set; }
        public string pcollection_date { get; set; }
        public string pimport_gene_date { get; set; }
        public int pprovider_id { get; set; }
        public string pprovider_name { get; set; }
        public int pclient_id { get; set; }
        public string pclient_name { get; set; }
        public string psample_id { get; set; }
        public string pgene_file_url { get; set; }
        public string preport_file_urls { get; set; }
        public bool preport_generated { get; set; }
        public Int16 pstatus { get; set; }
    }
    public class PatientGeneDumpProcModel
    {
        public string psample_id { get; set; }
        public string psnp_name { get; set; }
        public string pallele1 { get; set; }
        public string pallele2 { get; set; }
    }
    public class PatientsReportDataModel
    {
        public List<ReportCategoryInfo> ReportCategoryInfo { get; set; }
        public ReportBasicInfo ReportBasicInfo { get; set; }
        public ReportDynamicInfo ReportDynamicInfo { get; set; }
        public List<PatientGeneInfo> PatientGenes { get; set; }
    }

    public class ReportCategoryInfo
    {
        public string category { get; set; }
    }
    public class ReportBasicInfo
    {
        public string patient_name { get; set; }
        public string gender { get; set; }
        public DateTime? date_of_birth { get; set; }
        public string specimen_type { get; set; }
        public string collection_method { get; set; }
        public DateTime? collection_date { get; set; }
        public string provider_name { get; set; }
        public string client_logo { get; set; }
        public string report_bg_color { get; set; }
        public string report_heading_color { get; set; }
        public string report_sub_heading_color { get; set; }
        public string report_bg_font_color { get; set; }
        public string report_heading_font_color { get; set; }
        public string report_sub_heading_font_color { get; set; }
        public string provider_address { get; set; }
        public string sample_id { get; set; }
        public string PageLinkUrl { get; set; }
    }
    public class PatientGeneInfo
    {
        public int category_id { get; set; }
        public string category { get; set; }
        public int sub_category_id { get; set; }
        public string sub_category { get; set; }
        public string gene { get; set; }
        public string snp { get; set; }
        public string genotype { get; set; }
        public string result { get; set; }
        public string result_color { get; set; }
        public string genotype_result { get; set; }
        public string report_introduction { get; set; }
        public string study_name { get; set; }
        public string study_description { get; set; }
        public string study_link { get; set; }
    }

    public class ReportDynamicInfo
    {
        public string report_gene_predisposition { get; set; }
        public string report_important_takeways { get; set; }
       
    }

    //public class PatientReportDetailsModel
    //{
    //    public int preport_master_id { get; set; }
    //    public string preport_name { get; set; }
    //    public string preport_create_date { get; set; }
    //    public string preport_card_url { get; set; }
    //    public string preport_file_url { get; set; }
    //}

    public class PatientReportHistoryModel
    {
        public int ppatient_id { get; set; }
        public string pfirst_name { get; set; }
        public string plast_name { get; set; }
        public string pgender { get; set; }
        public string pdate_of_birth { get; set; }
        public int pprovider_id { get; set; }
        public string pprovider_name { get; set; }
        public string pclient_name { get; set; }
        public string psample_id { get; set; }
        public string pimport_gene_date { get; set; }
        public string? pgene_file_url { get; set; }
        public string? preport_file_urls { get; set; }
        public string preport_ids { get; set; }
    }

    public class PatientHistoryModel
    {
        public int preport_master_id { get; set; }
        public string preport_name { get; set; }
        public string psample_id { get; set; }
        public string preport_create_date { get; set; }
        public string pimport_gene_date { get; set; }
        public bool pis_interpretation_exist { get; set; }
        public string pgene_file_url { get; set; }
        public string preport_file_url { get; set; }
    }

    public class PatientListByProviderIdProcModel
    {
        public int ppatient_id { get; set; }
        public string pfirst_name { get; set; }
        public string plast_name { get; set; }
    }

    public class ReportColoursProcModel
    {
        public string preport_bg_color { get; set; }
        public string preport_heading_color { get; set; }
        public string preport_sub_heading_color { get; set; }
        public string preport_bg_font_color { get; set; }
        public string preport_heading_font_color { get; set; }
        public string preport_sub_heading_font_color { get; set; }
    }

   

}

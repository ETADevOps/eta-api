namespace ETA_API.Models.StoreProcModelDto
{
    public class PatientReportDetails
    {
        public string ReportLogo { get; set; }
        public string ProviderName { get; set; }
        public string ProviderAddress { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public string SampleId { get; set; }
        public string Specimen { get; set; }
        public string DateOfBirth { get; set; }
        public string CollectionDate { get; set; }
        public string ReceivedDate { get; set; }
        public string ReportedDate { get; set; }
        public string ReportBgColor { get; set; }
        public string ReportHeadingBgColor { get; set; }
        public string ReportSubHeadingBgColor { get; set; }
        public string ReportBgFontColor { get; set; }
        public string ReportHeadingFontColor { get; set; }
        public string ReportSubHeadingFontColor { get; set; }
    }
    public class GeneReportDetails
    {
        public string Category { get; set; }
        public string Gene { get; set; }
        public string Snp { get; set; }
        public string Genotype { get; set; }
        public string SubCategory { get; set; }
        public string SubCategory1 { get; set; }
        public string SubCategory2 { get; set; }
        public string SubCategory3 { get; set; }
        public string SubCategory4 { get; set; }
        public string SubCategory5 { get; set; }
        public string SubCategory6 { get; set; }
        public string SubCategoryResultColor1 { get; set; }
        public string SubCategoryResultColor2 { get; set; }
        public string SubCategoryResultColor3 { get; set; }
        public string SubCategoryResultColor4 { get; set; }
        public string SubCategoryResultColor5 { get; set; }
        public string SubCategoryResultColor6 { get; set; }
        public string Results { get; set; }
        public string ReportIntroduction { get; set; }
        public string StudyName { get; set; }
        public string StudyDescription { get; set; }
        public string ReferenceLink { get; set; }
        public string ReportBgColor { get; set; }
        public string ReportHeadingColor { get; set; }
        public string ReportSubHeadingColor { get; set; }
    }
    public class JobParameters
    {
        public int patient_id { get; set; }
        public string dna_file_url { get; set; }
    }
    public class PatientGeneJobRequestDto
    {
        public string job_id { get; set; }
        public JobParameters job_parameters { get; set; }
    }
    public class PatientGeneJobResponse
    {
        public long run_id { get; set; }
        public long number_in_job { get; set; }
    }
    public class GetPatientGeneJobStatus
    {
        public long job_id { get; set; }
        public long run_id { get; set; }
        public string creator_user_name { get; set; }
        public long number_in_job { get; set; }
        public long original_attempt_run_id { get; set; }
        public State state { get; set; }
    }
    public class State
    {
        public string life_cycle_state { get; set; }
        public string result_state { get; set; }
        public string state_message { get; set; }
        public bool user_cancelled_or_timedout { get; set; }
    }

    public class SubCategoryReportIntroduction
    {
        public string SubCategoryName { get; set; }
        public string SubCategoryIntroduction { get; set; }
    }

    public class ReportDetails
    {
        public string ReportGenePredisPosition { get; set; }
        public string ReportImportantTakeways { get; set; }
    }
}

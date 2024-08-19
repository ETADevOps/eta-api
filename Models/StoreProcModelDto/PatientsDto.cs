namespace ETA_API.Models.StoreProcModelDto
{
    public class PatientsDto
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string SpecimenType { get; set; }
        public string CollectionMethod { get; set; }
        public string CollectionDate { get; set; }
        public string ImportGeneDate { get; set; }
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string SampleId { get; set; }
        public string PatientGeneFile { get; set; }
        public string PatientReportUrls { get; set; }
        public bool ReportGenerated { get; set; }
        public Int16 Status { get; set; }
    }

    public class PatientDetailsReportMaster
    {
        public int patient_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string patient_full_name { get; set; }
        public string gender { get; set; }
        public string date_of_birth { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string email { get; set; }
        public string specimen_type { get; set; }
        public string collection_method { get; set; }
        public string collection_date { get; set; }
        public int provider_id { get; set; }
        public string provider_name { get; set; }
        public int client_id { get; set; }
        public string client_name { get; set; }
        public string sample_id { get; set; }
        public string gene_file_url { get; set; }
        public string report_url { get; set; }
        public bool isreportgenerated { get; set; }
        public Int16 status { get; set; }
    }

    public class PatientAttachmentReportMaster
    {
        public int patient_id { get; set; }
        public string report_name { get; set; }
        public string report_url { get; set; }
    }

    public class PatientByIdData
    {
        public List<PatientDetailsReportMaster> PatientDetailsReportMasters { get; set; }
        public List<PatientAttachmentReportMaster> PatientAttachmentReportMasters { get; set; }
    }
    public class CreatePatientDto
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }
        public string SpecimenType { get; set; }
        public string CollectionMethod { get; set; }
        public string PatientGeneFile { get; set; }
        public DateTime? CollectionDate { get; set; }
        public int ProviderId { get; set; }
        public int ClientId { get; set; }
        public Int16 Status { get; set; }
        public List<PatientAttachment> PatientAttachments { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }

    public class PatientAttachment
    {
        public int ppatient_id { get; set; }
        public string preport_name { get; set; }
        public string preport_url { get; set; }
        public string preport_type { get; set; }
        public int ppatient_attachments_id { get; set; }

    }
    public class PatientCityState
    {
        public string zip { get; set; }
        public string state { get; set; }
        public string city { get; set; }
    }

    public class ClientPatientOpeningData
    {
        public int client_id { get; set; }
        public string client_name { get; set; }
    }
    public class PatientsOpeningDataModel
    {
        public List<ClientPatientOpeningData> ClientPatientOpeningDatas { get; set; }
    }

    public class ClientReportMasterOpening
    {
        public int client_id { get; set; }
        public string client_name { get; set; }
    }

    public class ReportOpeningData
    {
        public List<ClientReportMasterOpening> ClientReportMasterOpening { get; set; }
    }

    public class PatientReportDetailsModel
    {
        public int report_master_id { get; set; }
        public string report_name { get; set; }
        public string report_create_date { get; set; }
        public string report_card_url { get; set; }
        public string report_url { get; set; }
    }

    public class PatientReportAttachmentDetailsModel
    {
        public int patient_id { get; set; }
        public string report_name { get; set; }
        public string report_url { get; set; }
        public DateTime? created_date { get; set; }
    }

    public class PatientReportDetailsData
    {
        public List<PatientReportDetailsModel> PatientReportDetailsModels { get; set; }
        public List<PatientReportAttachmentDetailsModel> PatientReportAttachmentDetailsModels { get; set; }
    }

    public class PatientGeneDumpModel
    {
        public string SampleId { get; set; }
        public string SnpName { get; set; }
        public string Allele1 { get; set; }
        public string Allele2 { get; set; }
    }

    public class PatientReportDetailsUrl
    {
        public int ReportMasterId { get; set; }
        public string ReportName { get; set; }
        public string ReportCreateDate { get; set; }
        public string ReportCardUrl { get; set; }
        public string ReportFileUrl { get; set; }
    }

    public class PatientReportHistoryDto
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string ClientName { get; set; }
        public string SampleId { get; set; }
        public string ImportGeneDate { get; set; }
        public string? PatientGeneFile { get; set; }
        public string ReportFileUrls { get; set; }
    }

    public class PatientHistoryDto
    {
        public int ReportMasterId { get; set; }
        public string ReportName { get; set; }
        public string SampleId { get; set; }
        public string ReportCreateDate { get; set; }
        public string ImportGeneDate { get; set; }
        public bool IsInterpretationExist { get; set; }
        public string GeneFileUrl { get; set; }
        public string ReportFileUrl { get; set; }
    }

    public class PatientData
    {
        public int patient_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string gender { get; set; }
        public string provider_name { get; set; }
    }

    public class CategoryData
    {
        public int report_master_id { get; set; }
        public string report_name { get; set; }
    }

    public class PatientListByClientIdData
    {
        public List<PatientData> PatientData { get; set; }
        public List<CategoryData> CategoryData { get; set; }

    }

    public class PatientListByProviderIdData
    {
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }

    public class CategoryReportDetails
    {
        public int report_master_id { get; set; }
        public string report_name { get; set; }
    }

    public class CategoryReportData
    {
        public List<CategoryReportDetails> CategoryReportDetails { get; set; }
    }

    public class ReportColoursDto
    {
        public string ReportBgColor { get; set; }
        public string ReportHeadingColor { get; set; }
        public string ReportSubHeadingColor { get; set; }
        public string ReportBgFontColor { get; set; }
        public string ReportHeadingFontColor { get; set; }
        public string ReportSubHeadingFontColor { get; set; }
    }

    public class UpdateReportColorDto
    {
        public int ClientId { get; set; }
        public string ReportBgColor { get; set; }
        public string ReportHeadingColor { get; set; }
        public string ReportSubHeadingColor { get; set; }
        public string ReportBgFontColor { get; set; }
        public string ReportHeadingFontColor { get; set; }
        public string ReportSubHeadingFontColor { get; set; }
    }
}
using ETA.API.Models.StoreProcContextModel;
using ETA_API.Models.Referance;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Services
{
    public interface IPatientrepository
    {
        Task<List<PatientsProcModel>> GetPatientsList(int clientid, int providerId, int patientId);
        Task<PatientByIdData> GetPatientsbyId(int patientId);
        Task<int> CreatePatient(CreatePatientDto createpatient, ServiceResponse obj);
        Task<int> UpdatePatient(CreatePatientDto createpatient, ServiceResponse obj);
        Task<List<PatientCityState>> GetZIPSearch(string searchText);
        Task<int> DeletePatient(DeleteByIdModel deletePatient);
        Task<PatientsOpeningDataModel> GetPatientsOpeningData(int clientId);
        Task<List<PatientGeneDumpProcModel>> GetPatientsGeneDumpbyId(int patientId, int reportMasterId);
        Task<PatientsReportDataModel> GetPatientsReportData(int patientId, int reportMasterId);
        Task<string> GetClientsReportTemplate(int patientId, int reportMasterId);
        Task<int> UpdatePatientReportUrl(int patientId, string reportUrl, int reportMasterId, int generatedBy, DateTime? generatedDate);
        Task<string> GetColorUrlByResult(int categoryId, int subcategoryId, string geneResult, string resultColor);
        Task<PatientReportDetailsData> GetPatientReportDetails(int patientId);
        Task<List<PatientReportHistoryModel>> GetPatientReportHistoryList(int clientId);
        Task<ReportOpeningData> GenerateReportOpeningData(int clientId);
        Task<List<PatientHistoryModel>> GetPatientHistory(int patientId, int reportId);
        Task<PatientListByClientIdData> GetPatientListByClientId(int clientId);
        Task<List<PatientListByProviderIdProcModel>> GetPatientListByProviderId(int providerId);
        Task<CategoryReportData> GetCategoryByPatientId(int patientId);
        Task<ReportColoursProcModel> GetReportColorByClientId(int clientId);
        Task<int> UpdateReportColorByClientId(UpdateReportColorDto updateReportColor, ServiceResponse obj);

    }
}

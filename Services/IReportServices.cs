using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Services
{
    public interface IReportServices
    {
        Task<string> GenerateGeneReport(int count, PatientsReportDataModel patientsReportData, List<GeneReportDetails> reportDetails, int reportMasterId, DateTime? generatedDate, int categoryCount, string templateDesign);
    }
}

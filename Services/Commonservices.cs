using ETA_API.Helpers;
using ETA_API.Models.StoreProcModelDto;
using Hangfire;
using Hangfire.Storage;
using Microsoft.IdentityModel.Tokens;

namespace ETA_API.Services
{
    public class CommonServices : ICommonService
    {
        private readonly IPatientrepository _patientrepository;
        private readonly IStorageService _storageService;
        private IConfiguration _configuration;
        public CommonServices(IPatientrepository patientrepository, IStorageService storageService, IConfiguration configuration)
        {
            _patientrepository = patientrepository ?? throw new ArgumentNullException(nameof(patientrepository));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<string> GetHangfireJobStatus(string jobId)
        {
            try
            {
                string stateName = "";

                IStorageConnection connection = JobStorage.Current.GetConnection();
                JobData jobData = connection.GetJobData(jobId);
                if (jobData != null)
                    stateName = jobData.State;

                return stateName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

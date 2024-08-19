namespace ETA_API.Services
{
    public interface ICommonService
    {
        Task<string> GetHangfireJobStatus(string jobId);
    }
}

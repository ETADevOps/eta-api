using ETA_API.Models.Referance;
using ETA_API.Models.StoreProcModelDto;
using Microsoft.Graph;

namespace ETA_API.Helpers
{
    public interface IStorageService
    {
        void UploadAttachment(string source, string fileType, out string downloadURL, out string path, string location, string extension, string uuid = null);
        Task<byte[]> MergeMultiplePDF(string[] PDFfileNames);
        Task<string> UploadDNAAzureJob(int patientId, int createdBy, DateTime? createdDate, IFormFile file);
        Task<string> SendVerificationEmail(string toEmail);
        Task<string> GenerateOTP();
        void UploadImage(string source, string fileType, out string downloadURL, out string path, string location, string extension);
    }
}
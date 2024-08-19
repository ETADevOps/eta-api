using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Services
{
    public interface IGeneratePasswordService
    {
        Task<string> GeneratePassword(int length, int numberOfNonAlphanumericCharacters);
        Task<int> UpdatePassword(int userId, string password, ServiceResponse obj);

    }
}

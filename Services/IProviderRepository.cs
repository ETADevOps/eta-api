using ETA.API.Models.StoreProcContextModel;
using ETA_API.Models.Referance;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Services
{
    public interface IProviderRepository
    {
        Task<List<ProvidersProcModel>> GetProvidersList(int clientId, int providerId);
        Task<ProvidersProcModel> GetProvidersById(int providerId);
        Task<int> CreateProvider(CreateProviderDto createProvider, ServiceResponse obj);
        Task<int> UpdateProvider(CreateProviderDto createProvider, ServiceResponse obj);
        Task<int> DeleteProvider(DeleteByIdModel deleteProvider);
        Task<ProviderOpeningData> GetProviderOpeningData(int clientId);
        Task<List<ProviderListByClientIdProcModel>> GetProviderListByClientId(int clientId);

    }
}

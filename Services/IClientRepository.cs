using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;
using ETA_API.Models.Referance;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Services
{
    public interface IClientRepository
    {
        Task<int> CreateClients(CreateClientDto createClientDto, ServiceResponse Obj);
        Task<ClientOpeningData> GetClientOpeningData();
        Task<List<ClientProcModel>> GetClientList(int clientId);
        Task<int> DeleteClient(DeleteByIdModel deleteClient);
        Task<ClientByIdOpeningData> GetClientById(int clientId);
        Task<int> UpdateClient(CreateClientDto createClientDto, ServiceResponse obj);



    }
}

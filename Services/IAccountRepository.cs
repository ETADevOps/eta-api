using ETA.API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Services
{
    public interface IAccountRepository
    {
        Task<List<AccountsProcModel>> GetAccountList();
        Task<AccountsByIdProcModel> GetAccountById(int accountId);
        Task<int> UpdateAccount(UpdateAccountDto updateAccountDto, ServiceResponse obj);


    }
}

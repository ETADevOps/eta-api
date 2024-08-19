using ETA.API.ExtensionMethod;
using ETA.API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;
using Npgsql;

namespace ETA_API.Services
{
    public class AccountRepository : IAccountRepository
    {
        private IConfiguration _configuration;
        public AccountRepository(IConfiguration Configuration) 
        {
            _configuration = Configuration ??
           throw new ArgumentNullException(nameof(Configuration));
        }

        public async Task<List<AccountsProcModel>> GetAccountList()
        {
            try
            {
                List<AccountsProcModel> accountProcModel = new List<AccountsProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_accounts_list()", conn))

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<AccountsProcModel>(reader);
                        accountProcModel.Add(result);
                    }
                }

                return accountProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AccountsByIdProcModel> GetAccountById(int accountId)
        {
            try
            {
                AccountsByIdProcModel accountsByIdProcModel = new AccountsByIdProcModel();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_accounts_by_id({accountId})", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        accountsByIdProcModel = DataReaderExtensionMethod.ConvertToObject<AccountsByIdProcModel>(reader);
                    }
                }

                return accountsByIdProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateAccount(UpdateAccountDto updateAccountDto, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM update_account(:paccount_id, :paccount_name, :paccount_logo, :paddress, :pcity, :pstate, :pzip, :pphone, :pfax, :pcontact_person_name, :pcontact_person_phone, :pcontact_person_email, :ptimezone, :pcreated_by, :pcreated_date, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":paccount_id", updateAccountDto.AccountId);
                cmd.Parameters.AddWithValue(":paccount_name", updateAccountDto.AccountName);
                cmd.Parameters.AddWithValue(":paccount_logo", updateAccountDto.AccountLogo);
                cmd.Parameters.AddWithValue(":paddress", updateAccountDto.Address);
                cmd.Parameters.AddWithValue(":pcity", updateAccountDto.City);
                cmd.Parameters.AddWithValue(":pstate", updateAccountDto.State);
                cmd.Parameters.AddWithValue(":pzip", updateAccountDto.Zip);
                cmd.Parameters.AddWithValue(":pphone", updateAccountDto.Phone);
                cmd.Parameters.AddWithValue(":pfax", updateAccountDto.Fax);
                cmd.Parameters.AddWithValue(":pcontact_person_name", updateAccountDto.ContactPersonName);
                cmd.Parameters.AddWithValue(":pcontact_person_phone", updateAccountDto.ContactPersonPhone);
                cmd.Parameters.AddWithValue(":pcontact_person_email", updateAccountDto.ContactPersonEmail);
                cmd.Parameters.AddWithValue(":ptimezone", updateAccountDto.TimeZone);
                cmd.Parameters.AddWithValue(":pcreated_by", updateAccountDto.CreatedBy);
                cmd.Parameters.AddWithValue(":pcreated_date", updateAccountDto.CreatedDate);
                cmd.Parameters.AddWithValue(":pmodified_by", updateAccountDto.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", updateAccountDto.ModifiedDate);

                try
                {
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            result = reader.GetInt32(0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return result;
        }
    }
}

using ETA.API.ExtensionMethod;
using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;
using ETA_API.Models.Referance;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;
using Npgsql;


namespace ETA_API.Services
{
    public class ProviderRepository : IProviderRepository
    {
        private IConfiguration _configuration;
        public ProviderRepository(IConfiguration Configuration)
        {
            _configuration = Configuration ??
            throw new ArgumentNullException(nameof(Configuration));
        }
        public async Task<List<ProvidersProcModel>> GetProvidersList(int clientId, int providerId)
        {
            try
            {
                List<ProvidersProcModel> providerList = new List<ProvidersProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_providers_list({clientId}, {providerId})", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<ProvidersProcModel>(reader);
                        providerList.Add(result);
                    }
                }

                return providerList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ProvidersProcModel> GetProvidersById(int providerId)
        {
            try
            {
                ProvidersProcModel providersByIdProcModel = new ProvidersProcModel();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_provider_by_id({providerId})", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        providersByIdProcModel = DataReaderExtensionMethod.ConvertToObject<ProvidersProcModel>(reader);
                    }
                }

                return providersByIdProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> CreateProvider(CreateProviderDto createProvider, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM create_provider(:pprovider_name, :paddress, :pcity, :pstate, :pzip, :pphone, :pemail, :pclient_id, :pcreated_by, :pcreated_date, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pprovider_name", createProvider.Providername);
                cmd.Parameters.AddWithValue(":paddress", createProvider.Address);
                cmd.Parameters.AddWithValue(":pcity", createProvider.City);
                cmd.Parameters.AddWithValue(":pstate", createProvider.State);
                cmd.Parameters.AddWithValue(":pzip", createProvider.Zip);
                cmd.Parameters.AddWithValue(":pphone", createProvider.Phone);
                cmd.Parameters.AddWithValue(":pemail", createProvider.Email);
                cmd.Parameters.AddWithValue(":pclient_id", createProvider.ClientId);
                cmd.Parameters.AddWithValue(":pcreated_by", createProvider.CreatedBy);
                cmd.Parameters.AddWithValue(":pcreated_date", createProvider.CreatedDate);
                cmd.Parameters.AddWithValue(":pmodified_by", createProvider.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", createProvider.ModifiedDate);

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
        public async Task<int> UpdateProvider(CreateProviderDto createProvider, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM update_provider(:pprovider_id, :pprovider_name, :paddress, :pcity, :pstate, :pzip, :pphone, :pemail, :pclient_id, :pmodified_by, :pmodified_date, :pstatus)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pprovider_id", createProvider.ProviderId);
                cmd.Parameters.AddWithValue(":pprovider_name", createProvider.Providername);
                cmd.Parameters.AddWithValue(":paddress", createProvider.Address);
                cmd.Parameters.AddWithValue(":pcity", createProvider.City);
                cmd.Parameters.AddWithValue(":pstate", createProvider.State);
                cmd.Parameters.AddWithValue(":pzip", createProvider.Zip);
                cmd.Parameters.AddWithValue(":pphone", createProvider.Phone);
                cmd.Parameters.AddWithValue(":pemail", createProvider.Email);
                cmd.Parameters.AddWithValue(":pclient_id", createProvider.ClientId);
                //cmd.Parameters.AddWithValue(":pcreated_by", createProvider.CreatedBy);
                // cmd.Parameters.AddWithValue(":pcreated_date", createProvider.CreatedDate);
                cmd.Parameters.AddWithValue(":pmodified_by", createProvider.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", createProvider.ModifiedDate);
                cmd.Parameters.AddWithValue(":pstatus", createProvider.Status);

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
        public async Task<int> DeleteProvider(DeleteByIdModel deleteProvider)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM delete_provider(:pprovider_id, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pprovider_id", deleteProvider.Id);
                cmd.Parameters.AddWithValue(":pmodified_by", deleteProvider.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", deleteProvider.ModifiedDate);
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

        public async Task<ProviderOpeningData> GetProviderOpeningData(int clientId)
        {
            try
            {
                ProviderOpeningData providerOpeningData = new ProviderOpeningData();

                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                string query = DynamicFormRefProviderOpeningDataQuery(clientId, GetListOfProviderOpeningDataOpeningCursors());

                await using (var cmd = new NpgsqlCommand(query, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    reader.NextResult();
                    List<ProviderClientMaster> providerClientMasters = new List<ProviderClientMaster>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<ProviderClientMaster>(reader);
                        providerClientMasters.Add(result);
                    }
                    providerOpeningData.ProviderClientMaster = providerClientMasters;

                }

                return providerOpeningData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DynamicFormRefProviderOpeningDataQuery(int clientId, List<CursorMapping> cursors)
        {
            string command = string.Empty;

            command = $"SELECT * from public.get_provider_opening_data({clientId},";

            foreach (var str in cursors)
            {
                command = command + $"{"'" + str.cursorName + "'"},";
            }

            command = command.Substring(0, command.Length - 1) + ");";
            foreach (var str in cursors)
            {
                command = command + $"FETCH ALL IN {"" + str.cursorName + ""}" + ";";
            }

            return command;

        }
        public List<CursorMapping> GetListOfProviderOpeningDataOpeningCursors()
        {
            List<CursorMapping> str = new List<CursorMapping>()
            {
                new CursorMapping("pref_clients","pref_clients")
            };
            return str;
        }

        public async Task<List<ProviderListByClientIdProcModel>> GetProviderListByClientId(int clientId)
        {
            try
            {
                List<ProviderListByClientIdProcModel> clientByIdProcModel = new List<ProviderListByClientIdProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_provider_list_by_client_id({clientId})", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<ProviderListByClientIdProcModel>(reader);
                        clientByIdProcModel.Add(result);
                    }
                }

                return clientByIdProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

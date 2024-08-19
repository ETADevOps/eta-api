using AutoMapper;
using ETA.API.ExtensionMethod;
using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;
using ETA_API.Models.Referance;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;
using Npgsql.TypeMapping;
using System.Data;

namespace ETA_API.Services
{
    public class ClientRepository : IClientRepository
    {
        private IConfiguration _configuration;

        public ClientRepository(IConfiguration Configuration)
        {
            _configuration = Configuration ??
            throw new ArgumentNullException(nameof(Configuration));
        }

        public async Task<int> CreateClients(CreateClientDto createClientDto, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();           

            int result = 0;

            conn.TypeMapper.MapComposite<ClientDomainrelation>("type_client_domain");
            conn.TypeMapper.MapComposite<ClientReportTemplaterelation>("type_clients_report_template_relation");

            string query = $"SELECT * FROM create_client(:pclient_name, :pclient_logo, :paddress, :pcity, :pstate, :pzip, :pphone, :pfax, :pcontact_person_name, :pcontact_person_phone, :pcontact_person_email, :ptimezone, :parry_client_domains, :parray_clients_report_templates, :pcreated_by, :pcreated_date, :pmodified_by, :pmodified_date)";


            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pclient_name", createClientDto.ClientName);
                cmd.Parameters.AddWithValue(":pclient_logo", createClientDto.ClientLogo);
                cmd.Parameters.AddWithValue(":paddress", createClientDto.Address);
                cmd.Parameters.AddWithValue(":pcity", createClientDto.City);
                cmd.Parameters.AddWithValue(":pstate", createClientDto.State);
                cmd.Parameters.AddWithValue(":pzip", createClientDto.Zip);
                cmd.Parameters.AddWithValue(":pphone", createClientDto.Phone);
                cmd.Parameters.AddWithValue(":pfax", createClientDto.Fax);
                cmd.Parameters.AddWithValue(":pcontact_person_name", createClientDto.ContactPersonName);
                cmd.Parameters.AddWithValue(":pcontact_person_phone", createClientDto.ContactPersonPhone);
                cmd.Parameters.AddWithValue(":pcontact_person_email", createClientDto.ContactPersonEmail);
                cmd.Parameters.AddWithValue(":ptimezone", createClientDto.TimeZone);
                cmd.Parameters.AddWithValue(":parry_client_domains", createClientDto.ClientDomainrelations);
                cmd.Parameters.AddWithValue(":parray_clients_report_templates", createClientDto.ClientReportTemplaterelations);
                cmd.Parameters.AddWithValue(":pcreated_by", createClientDto.CreatedBy);
                cmd.Parameters.AddWithValue(":pcreated_date", createClientDto.CreatedDate);
                cmd.Parameters.AddWithValue(":pmodified_by", createClientDto.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", createClientDto.ModifiedDate);
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

        public async Task<ClientOpeningData> GetClientOpeningData()
        {
            try
            {
                ClientOpeningData clientOpeningData = new ClientOpeningData();

                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                string query = DynamicFormRefClientOpeningDataQuery(GetListOfClientOpeningDataOpeningCursors());

                await using (var cmd = new NpgsqlCommand(query, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    reader.NextResult();
                    List<ClientReportMaster> clientReportMaster = new List<ClientReportMaster>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<ClientReportMaster>(reader);
                        clientReportMaster.Add(result);
                    }
                    clientOpeningData.ClientReportMaster = clientReportMaster;

                    reader.NextResult();
                    List<ClientReportTemplateMapping> clientReportTemplateMapping = new List<ClientReportTemplateMapping>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<ClientReportTemplateMapping>(reader);
                        clientReportTemplateMapping.Add(result);
                    }
                    clientOpeningData.ClientReportTemplateMapping = clientReportTemplateMapping;
                }

                return clientOpeningData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DynamicFormRefClientOpeningDataQuery(List<CursorMapping> cursors)
        {
            string command = string.Empty;

            command = $"SELECT * from public.get_client_opening_data(";

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
        public List<CursorMapping> GetListOfClientOpeningDataOpeningCursors()
        {
            List<CursorMapping> str = new List<CursorMapping>()
            {
                new CursorMapping("pref_report_master","pref_report_master"),
                new CursorMapping("pref_report_templates","pref_report_templates")
            };
            return str;
        }

        public async Task<List<ClientProcModel>> GetClientList(int clientId)
        {
            try
            {
                List<ClientProcModel> clientProcModel = new List<ClientProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_clients_list({clientId})", conn))

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<ClientProcModel>(reader);
                        clientProcModel.Add(result);
                    }
                }

                return clientProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteClient(DeleteByIdModel deleteClient)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM delete_client(:pclient_id, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pclient_id", deleteClient.Id);
                cmd.Parameters.AddWithValue(":pmodified_by", deleteClient.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", deleteClient.ModifiedDate);
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

        public async Task<ClientByIdOpeningData> GetClientById(int clientId)
        {
            try
            {
                ClientByIdOpeningData clientByIdOpeningData = new ClientByIdOpeningData();

                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                string query = DynamicFormRefClientByIdOpeningDataQuery(clientId, GetListOfClientByIdOpeningDataOpeningCursors());

                await using (var cmd = new NpgsqlCommand(query, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    reader.NextResult();
                    ClientMaster clientMaster = new ClientMaster();
                    while (await reader.ReadAsync())
                    {
                        clientMaster = DataReaderExtensionMethod.ConvertToObject<ClientMaster>(reader);
                        
                    }
                    clientByIdOpeningData.ClientMasters = clientMaster;

                    reader.NextResult();
                    List<DomainMaster> domainMaster = new List<DomainMaster>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<DomainMaster>(reader);
                        domainMaster.Add(result);

                    }
                    clientByIdOpeningData.DomainMasters = domainMaster;

                    reader.NextResult();
                    List<ClientReportTemplateMaster> clientReportTemplateMaster = new List<ClientReportTemplateMaster>();
                    while (await reader.ReadAsync())
                    {
                       var result = DataReaderExtensionMethod.ConvertToObject<ClientReportTemplateMaster>(reader);
                        clientReportTemplateMaster.Add(result);
                        
                    }
                    clientByIdOpeningData.ClientReportTemplateMasters = clientReportTemplateMaster;
                }

                return clientByIdOpeningData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DynamicFormRefClientByIdOpeningDataQuery(int clienId, List<CursorMapping> cursors)
        {
            string command = string.Empty;

            command = $"SELECT * from public.get_client_by_id({clienId},";

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
        public List<CursorMapping> GetListOfClientByIdOpeningDataOpeningCursors()
        {
            List<CursorMapping> str = new List<CursorMapping>()
            {
                new CursorMapping("pref_clients","pref_clients"),
                new CursorMapping("pref_domains","pref_domains"),
                new CursorMapping("pref_clients_report_templates","pref_clients_report_templates")
            };
            return str;
        }

        public async Task<int> UpdateClient(CreateClientDto createClientDto, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();

            int result = 0;

            conn.TypeMapper.MapComposite<ClientDomainrelation>("type_client_domain");
            conn.TypeMapper.MapComposite<ClientReportTemplaterelation>("type_clients_report_template_relation");

            string query = $"SELECT * FROM update_client(:pclient_id, :pclient_name, :pclient_logo, :paddress, :pcity, :pstate, :pzip, :pphone, :pfax, :pcontact_person_name, :pcontact_person_phone, :pcontact_person_email, :ptimezone, :parray_client_domains, :parray_clients_report_templates, :pcreated_by, :pcreated_date, :pmodified_by, :pmodified_date)";


            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pclient_id", createClientDto.ClientId);
                cmd.Parameters.AddWithValue(":pclient_name", createClientDto.ClientName);
                cmd.Parameters.AddWithValue(":pclient_logo", createClientDto.ClientLogo);
                cmd.Parameters.AddWithValue(":paddress", createClientDto.Address);
                cmd.Parameters.AddWithValue(":pcity", createClientDto.City);
                cmd.Parameters.AddWithValue(":pstate", createClientDto.State);
                cmd.Parameters.AddWithValue(":pzip", createClientDto.Zip);
                cmd.Parameters.AddWithValue(":pphone", createClientDto.Phone);
                cmd.Parameters.AddWithValue(":pfax", createClientDto.Fax);
                cmd.Parameters.AddWithValue(":pcontact_person_name", createClientDto.ContactPersonName);
                cmd.Parameters.AddWithValue(":pcontact_person_phone", createClientDto.ContactPersonPhone);
                cmd.Parameters.AddWithValue(":pcontact_person_email", createClientDto.ContactPersonEmail);
                cmd.Parameters.AddWithValue(":ptimezone", createClientDto.TimeZone);
                cmd.Parameters.AddWithValue(":parray_client_domains", createClientDto.ClientDomainrelations);
                cmd.Parameters.AddWithValue(":parray_clients_report_templates", createClientDto.ClientReportTemplaterelations);
                cmd.Parameters.AddWithValue(":pcreated_by", createClientDto.CreatedBy);
                cmd.Parameters.AddWithValue(":pcreated_date", createClientDto.CreatedDate);
                cmd.Parameters.AddWithValue(":pmodified_by", createClientDto.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", createClientDto.ModifiedDate);
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

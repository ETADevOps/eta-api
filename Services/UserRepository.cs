using ETA.API.ExtensionMethod;
using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;
using ETA_API.Models.Referance;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;
using MailKit.Security;
using MimeKit;
using Npgsql;

namespace ETA.API.Services
{
    public class UserRepository : IUserRepository
    {
        //these declarations represent various dependencies and resources needed by the class.
        private IConfiguration _configuration;

        public UserRepository(IConfiguration Configuration)
        {
            _configuration = Configuration ??
            throw new ArgumentNullException(nameof(Configuration));

        }

        public async Task<List<UsersStoreProcModel>> GetUsersList(int userId, int clientId, int providerId, int patientId)
        {
            try
            {
                List<UsersStoreProcModel> userList = new List<UsersStoreProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_users_list({userId}, {clientId}, {providerId}, {patientId})", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<UsersStoreProcModel>(reader);
                        userList.Add(result);
                    }
                }

                return userList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<UsersByIdStoreProcModel> GetUserById(int userId, string subjectId)
        {
            try
            {
                UsersByIdStoreProcModel userByIdProcModel = new UsersByIdStoreProcModel();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_user_by_id({userId}, '{subjectId}')", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        userByIdProcModel = DataReaderExtensionMethod.ConvertToObject<UsersByIdStoreProcModel>(reader);
                    }
                }

                return userByIdProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> CreateUser(CreateUserDto createUser, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM create_user(:pfirst_name, :plast_name, :puser_name, :paddress, :pcity, :pstate, :pzip, :pemail, :ppassword, :pphone, :psubject, :pstart_ip, :pend_ip, :pclient_id, :pprovider_id, :ppatient_id, :prole_id, :pcreated_by, :pcreated_date, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pfirst_name", createUser.FirstName);
                cmd.Parameters.AddWithValue(":plast_name", createUser.LastName);
                cmd.Parameters.AddWithValue(":puser_name", createUser.UserName);
                cmd.Parameters.AddWithValue(":paddress", createUser.Address);
                cmd.Parameters.AddWithValue(":pcity", createUser.City);
                cmd.Parameters.AddWithValue(":pstate", createUser.State);
                cmd.Parameters.AddWithValue(":pzip", createUser.Zip);
                cmd.Parameters.AddWithValue(":pemail", createUser.Email);
                cmd.Parameters.AddWithValue(":ppassword", createUser.Password);
                cmd.Parameters.AddWithValue(":pphone", createUser.Phone);
                cmd.Parameters.AddWithValue(":psubject", createUser.Subject);
                cmd.Parameters.AddWithValue(":pstart_ip", createUser.StartIp);
                cmd.Parameters.AddWithValue(":pend_ip", createUser.EndIp);
                cmd.Parameters.AddWithValue(":pclient_id", createUser.ClientId);
                cmd.Parameters.AddWithValue(":pprovider_id", createUser.ProviderId);
                cmd.Parameters.AddWithValue(":ppatient_id", createUser.PatientId);
                cmd.Parameters.AddWithValue(":prole_id", createUser.RoleId);
                cmd.Parameters.AddWithValue(":pcreated_by", createUser.CreatedBy);
                cmd.Parameters.AddWithValue(":pcreated_date", createUser.CreatedDate);
                cmd.Parameters.AddWithValue(":pmodified_by", createUser.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", createUser.ModifiedDate);

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
        public async Task<int> UpdateUser(CreateUserDto createUser, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM update_user(:puser_id, :pfirst_name, :plast_name, :puser_name, :paddress, :pcity, :pstate, :pzip, :pphone, :pemail, :pstart_ip, :pend_ip, :pclient_id, :pprovider_id, :ppatient_id, :prole_id, :pmodified_by, :pmodified_date, :pstatus)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":puser_id", createUser.UserId);
                cmd.Parameters.AddWithValue(":pfirst_name", createUser.FirstName);
                cmd.Parameters.AddWithValue(":plast_name", createUser.LastName);
                cmd.Parameters.AddWithValue(":puser_name", createUser.UserName);
                cmd.Parameters.AddWithValue(":paddress", createUser.Address);
                cmd.Parameters.AddWithValue(":pcity", createUser.City);
                cmd.Parameters.AddWithValue(":pstate", createUser.State);
                cmd.Parameters.AddWithValue(":pzip", createUser.Zip);
                cmd.Parameters.AddWithValue(":pphone", createUser.Phone);
                cmd.Parameters.AddWithValue(":pemail", createUser.Email);
                cmd.Parameters.AddWithValue(":pstart_ip", createUser.StartIp);
                cmd.Parameters.AddWithValue(":pend_ip", createUser.EndIp);
                cmd.Parameters.AddWithValue(":pclient_id", createUser.ClientId);
                cmd.Parameters.AddWithValue(":pprovider_id", createUser.ProviderId);
                cmd.Parameters.AddWithValue(":ppatient_id", createUser.PatientId);
                cmd.Parameters.AddWithValue(":prole_id", createUser.RoleId);
                cmd.Parameters.AddWithValue(":pmodified_by", createUser.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", createUser.ModifiedDate);
                cmd.Parameters.AddWithValue(":pstatus", createUser.Status);

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
        public async Task<bool> CheckEmailAddress(string email)
        {
            try
            {
                bool result = false;
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();

                await using (var cmd = new NpgsqlCommand($"select * from public.check_email_address('{email}')", conn))

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result = reader.GetBoolean(0);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> GetEmailUserId(int userId)
        {
            try
            {
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                string result = "";

                await using (var cmd = new NpgsqlCommand($"select email from users where user_id = {userId}", conn))

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string email = reader.GetString(0);
                        result = email;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> UpdateSubjectIdByUserId(int userId, string subjectId)
        {
            try
            {
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:ImageGalleryDBConnectionString"]);
                await conn.OpenAsync();
                int result = 0;

                await using (var cmd = new NpgsqlCommand($"update users set subject = '{subjectId}' where user_id = {userId}", conn))

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result = reader.GetInt16(0);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteUser(DeleteByIdModel deleteUser)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM delete_user(:puser_id, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":puser_id", deleteUser.Id);
                cmd.Parameters.AddWithValue(":pmodified_by", deleteUser.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", deleteUser.ModifiedDate);
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

        public async Task<int> CreateUserSignInLog(CreateUserSignInLog createUserSignInLog, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM create_user_signin_log(:puser_id, :psign_in_date, :pipaddress, :plocation, :paction)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":puser_id", createUserSignInLog.UserId);
                cmd.Parameters.AddWithValue(":psign_in_date", createUserSignInLog.SignInDate);
                cmd.Parameters.AddWithValue(":pipaddress", createUserSignInLog.IpAddress);
                cmd.Parameters.AddWithValue(":plocation", createUserSignInLog.Location);
                cmd.Parameters.AddWithValue(":paction", createUserSignInLog.Action);
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

        public async Task<int> UpdateUserSignInLog(UpdateUserSignInLogDto updateUserSignInLogDto, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM update_user_signin_log(:puser_id, :pstatus, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":puser_id", updateUserSignInLogDto.UserId);
                cmd.Parameters.AddWithValue(":pstatus", updateUserSignInLogDto.Status);
                cmd.Parameters.AddWithValue(":pmodified_by", updateUserSignInLogDto.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", updateUserSignInLogDto.ModifiedDate);

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

        public async Task<List<UserSignInLogProcModel>> GetUserSignInLogList(int userId, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                List<UserSignInLogProcModel> userSignInLogProcModel = new List<UserSignInLogProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_user_signin_logs({userId}, '{fromDate}', '{toDate}')", conn))

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<UserSignInLogProcModel>(reader);
                        userSignInLogProcModel.Add(result);
                    }
                }

                return userSignInLogProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> GetIpVerification(int userId, string ip)
        {
            try
            {
                bool result = false;
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_ip_verification({userId}, '{ip}')", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        if (reader["get_ip_verification"] != null && reader["get_ip_verification"] != DBNull.Value)
                        {
                            result = reader.GetBoolean(0);
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<AuditOpeningData> GetAuditOpeningData()
        {
            try
            {
                AuditOpeningData auditOpeningData = new AuditOpeningData();

                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                string query = DynamicFormRefAuditOpeningDataQuery(GetListOfAuditOpeningDataOpeningCursors());

                await using (var cmd = new NpgsqlCommand(query, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    reader.NextResult();
                    List<AuditMaster> auditReport = new List<AuditMaster>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<AuditMaster>(reader);
                        auditReport.Add(result);
                    }
                    auditOpeningData.AuditMaster = auditReport;

                    reader.NextResult();
                    List<UserMaster> userReport = new List<UserMaster>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<UserMaster>(reader);
                        userReport.Add(result);
                    }
                    auditOpeningData.UserMaster = userReport;

                    reader.NextResult();
                    List<PatientMaster> patientReport = new List<PatientMaster>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<PatientMaster>(reader);
                        patientReport.Add(result);
                    }
                    auditOpeningData.PatientMaster = patientReport;

                    reader.NextResult();
                    List<ProviderMaster> providerReport = new List<ProviderMaster>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<ProviderMaster>(reader);
                        providerReport.Add(result);
                    }
                    auditOpeningData.ProviderMaster = providerReport;

                    reader.NextResult();
                    List<ReportMasterAudit> reportMasterAudits = new List<ReportMasterAudit>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<ReportMasterAudit>(reader);
                        reportMasterAudits.Add(result);
                    }
                    auditOpeningData.ReportMasterAudit = reportMasterAudits;

                    reader.NextResult();
                    List<ClientMasterAudit> clientMasterAudits = new List<ClientMasterAudit>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<ClientMasterAudit>(reader);
                        clientMasterAudits.Add(result);
                    }
                    auditOpeningData.ClientMasterAudit = clientMasterAudits;
                }

                return auditOpeningData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DynamicFormRefAuditOpeningDataQuery(List<CursorMapping> cursors)
        {
            string command = string.Empty;

            command = $"SELECT * from public.get_audit_log_opening_data(";

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
        public List<CursorMapping> GetListOfAuditOpeningDataOpeningCursors()
        {
            List<CursorMapping> str = new List<CursorMapping>()
            {
                new CursorMapping("pref_audit_category","pref_audit_category"),
                new CursorMapping("pref_users","pref_users"),
                new CursorMapping("pref_patients","pref_patients"),
                new CursorMapping("pref_providers","pref_providers"),
                new CursorMapping("pref_reports","pref_reports"),
                new CursorMapping("pref_clients","pref_clients")
            };
            return str;
        }

        public async Task<int> CreateAuditLog(CreateAuditLog createAuditLog, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM create_audit_log(:paudit_user_id, :paudit_date, :puser_id, :paudit_category_master_id, :pactivity, :ppatient_id, :pprovider_id, :preport_id, :pclient_id, :pstatus)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":paudit_user_id", createAuditLog.AuditUserId);
                cmd.Parameters.AddWithValue(":paudit_date", createAuditLog.AuditDate);
                cmd.Parameters.AddWithValue(":puser_id", createAuditLog.UserId);
                cmd.Parameters.AddWithValue(":paudit_category_master_id", createAuditLog.AuditCategoryMasterId);
                cmd.Parameters.AddWithValue(":pactivity", createAuditLog.Activity);
                cmd.Parameters.AddWithValue(":ppatient_id", createAuditLog.PatientId);
                cmd.Parameters.AddWithValue(":pprovider_id", createAuditLog.ProviderId);
                cmd.Parameters.AddWithValue(":preport_id", createAuditLog.ReportId);
                cmd.Parameters.AddWithValue(":pclient_id", createAuditLog.ClientId);
                cmd.Parameters.AddWithValue(":pstatus", createAuditLog.Status);

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

        public async Task<List<AuditLogProcModel>> GetAuditLogList(int userId, DateTime? fromDate, DateTime? toDate, int categoryId, int patientId, int providerId, int reportId, int clientId)
        {
            try
            {
                List<AuditLogProcModel> auditLogProcModel = new List<AuditLogProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_audit_log_list({userId}, '{fromDate}', '{toDate}', '{categoryId}', '{patientId}', '{providerId}', '{reportId}', '{clientId}')", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<AuditLogProcModel>(reader);
                        auditLogProcModel.Add(result);
                    }
                }

                return auditLogProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<RecycleBinOpeningDataProcModel>> GetRecycleBinOpeningData()
        {
            try
            {
                List<RecycleBinOpeningDataProcModel> recycleBinOpeningData = new List<RecycleBinOpeningDataProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"select * from public.get_recycle_bin_opening_data()", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<RecycleBinOpeningDataProcModel>(reader);
                        recycleBinOpeningData.Add(result);
                    }
                }

                return recycleBinOpeningData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RecycleBinData> GetRecycleBinData(int categoryId)
        {
            try
            {
                RecycleBinData recycleBinData = new RecycleBinData();

                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                string query = DynamicFormRefRecycleBinDataQuery(categoryId, GetListOfRecycleBinDataCursors());

                await using (var cmd = new NpgsqlCommand(query, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (categoryId == 1)
                    {
                        reader.NextResult();
                        List<RecycleUserMaster> recycleUserData = new List<RecycleUserMaster>();
                        while (await reader.ReadAsync())
                        {
                            var result = DataReaderExtensionMethod.ConvertToObject<RecycleUserMaster>(reader);
                            recycleUserData.Add(result);
                        }
                        recycleBinData.RecycleUserMaster = recycleUserData;
                    }
                    else if (categoryId == 2)
                    {
                        reader.NextResult();
                        List<RecyclePatientMaster> recyclePatientData = new List<RecyclePatientMaster>();
                        while (await reader.ReadAsync())
                        {
                            var result = DataReaderExtensionMethod.ConvertToObject<RecyclePatientMaster>(reader);
                            recyclePatientData.Add(result);
                        }
                        recycleBinData.RecyclePatientMaster = recyclePatientData;
                    }
                    else if (categoryId == 3)
                    {
                        reader.NextResult();
                        List<RecycleProviderMaster> recycleProviderData = new List<RecycleProviderMaster>();
                        while (await reader.ReadAsync())
                        {
                            var result = DataReaderExtensionMethod.ConvertToObject<RecycleProviderMaster>(reader);
                            recycleProviderData.Add(result);
                        }
                        recycleBinData.RecycleProviderMaster = recycleProviderData;
                    }
                    else if (categoryId == 4)
                    {
                        reader.NextResult();
                        List<RecycleClientMaster> recycleClientData = new List<RecycleClientMaster>();
                        while (await reader.ReadAsync())
                        {
                            var result = DataReaderExtensionMethod.ConvertToObject<RecycleClientMaster>(reader);
                            recycleClientData.Add(result);
                        }
                        recycleBinData.RecycleClientMaster = recycleClientData;
                    }
                }

                return recycleBinData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string DynamicFormRefRecycleBinDataQuery(int categoryId, List<CursorMapping> cursors)
        {
            string command = string.Empty;

            command = $"SELECT * from public.get_recycle_bin_data({categoryId},";

            foreach (var str in cursors)
            {
                command = command + $"{"'" + str.cursorName + "'"},";
            }

            command = command.Substring(0, command.Length - 1) + ");";

            if (categoryId == 1)
            {
                command = command + $"FETCH ALL IN pref_users" + ";";
            }
            else if (categoryId == 2)
            {
                command = command + $"FETCH ALL IN pref_patients" + ";";
            }
            else if (categoryId == 3)
            {
                command = command + $"FETCH ALL IN pref_providers" + ";";
            }
            else if (categoryId == 4)
            {
                command = command + $"FETCH ALL IN pref_clients" + ";";
            }


            return command;

        }
        public List<CursorMapping> GetListOfRecycleBinDataCursors()
        {
            List<CursorMapping> str = new List<CursorMapping>()
            {
                new CursorMapping("pref_users","pref_users"),
                new CursorMapping("pref_patients","pref_patients"),
                new CursorMapping("pref_providers","pref_providers"),
                new CursorMapping("pref_clients","pref_clients")
            };
            return str;
        }

        public async Task<int> UpdateRecycleBinData(UpdateRecycleBinDataModel updateRecycleBinDataModel, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM update_recycle_bin_data(:pcategory_id, :pmodified_id, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pcategory_id", updateRecycleBinDataModel.CategoryId);
                cmd.Parameters.AddWithValue(":pmodified_id", updateRecycleBinDataModel.ModifiedId);
                cmd.Parameters.AddWithValue(":pmodified_by", updateRecycleBinDataModel.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", updateRecycleBinDataModel.ModifiedDate);

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

        public async Task<UserOpeningData> GetUserOpeningData(int clientId, int userId)
        {
            try
            {
                UserOpeningData userOpeningData = new UserOpeningData();

                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                string query = DynamicFormRefUserOpeningDataQuery(clientId, userId, GetListOfUserOpeningDataOpeningCursors());

                await using (var cmd = new NpgsqlCommand(query, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {

                    reader.NextResult();
                    List<RoleOpeningMaster> roleReport = new List<RoleOpeningMaster>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<RoleOpeningMaster>(reader);
                        roleReport.Add(result);
                    }
                    userOpeningData.RoleOpeningMaster = roleReport;

                    reader.NextResult();
                    List<UserOpeningMaster> userReport = new List<UserOpeningMaster>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<UserOpeningMaster>(reader);
                        userReport.Add(result);
                    }
                    userOpeningData.UserOpeningMaster = userReport;
                    
                }

                return userOpeningData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DynamicFormRefUserOpeningDataQuery(int clientId, int userId, List<CursorMapping> cursors)
        {
            string command = string.Empty;

            command = $"SELECT * from public.get_users_opening_data({clientId},{userId},";

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
        public List<CursorMapping> GetListOfUserOpeningDataOpeningCursors()
        {
            List<CursorMapping> str = new List<CursorMapping>()
            {
                new CursorMapping("pref_roles","pref_roles"),
                new CursorMapping("pref_clients","pref_clients")


            };
            return str;
        }

        public async Task<LoginUsersData> GetLoginUserDetails(int userid)
        {
            try
            {
                LoginUsersData loginUserData = new LoginUsersData();

                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                string query = DynamicFormRefLoginUserDataQuery(userid, GetListOfLoginUserDataOpeningCursors());

                await using (var cmd = new NpgsqlCommand(query, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    reader.NextResult();
                    UserDetails userDetails = new UserDetails();
                    while (await reader.ReadAsync())
                    {
                        userDetails = DataReaderExtensionMethod.ConvertToObject<UserDetails>(reader);
                        
                    }
                    loginUserData.UserDetails = userDetails;

                    reader.NextResult();
                    List<UserAccounts> userAccounts = new List<UserAccounts>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<UserAccounts>(reader);
                        userAccounts.Add(result);
                    }
                    loginUserData.UserAccounts = userAccounts;

                    reader.NextResult();
                    List<UserRoles> userRoles = new List<UserRoles>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<UserRoles>(reader);
                        userRoles.Add(result);
                    }
                    loginUserData.UserRoles = userRoles;

                    reader.NextResult();
                    List<ClientMasterDetails> reportMasterDetails = new List<ClientMasterDetails>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<ClientMasterDetails>(reader);
                        reportMasterDetails.Add(result);
                    }
                    loginUserData.ClientMasterDetails = reportMasterDetails;
                }

                return loginUserData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DynamicFormRefLoginUserDataQuery(int userid, List<CursorMapping> cursors)
        {
            string command = string.Empty;

            command = $"SELECT * from public.get_login_users_details({userid},";

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
        public List<CursorMapping> GetListOfLoginUserDataOpeningCursors()
        {
            List<CursorMapping> str = new List<CursorMapping>()
            {
                new CursorMapping("pref_user_details","pref_user_details"),
                new CursorMapping("pref_user_accounts","pref_user_accounts"),
                new CursorMapping("pref_user_roles","pref_user_roles"),
                new CursorMapping("pref_client_details","pref_client_details")
            };
            return str;
        }

        public async Task<SecurityOpeningData> GetSecurityOpeningData()
        {
            try
            {
                SecurityOpeningData securityOpeningData = new SecurityOpeningData();

                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                string query = DynamicFormRefSecurityOpeningDataQuery(GetListOfSecurityOpeningDataOpeningCursors());

                await using (var cmd = new NpgsqlCommand(query, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {

                    reader.NextResult();
                    List<SecurityQuestionMaster> securityQuestionMaster = new List<SecurityQuestionMaster>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<SecurityQuestionMaster>(reader);
                        securityQuestionMaster.Add(result);
                    }
                    securityOpeningData.SecurityQuestionMasters = securityQuestionMaster;

                }

                return securityOpeningData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DynamicFormRefSecurityOpeningDataQuery(List<CursorMapping> cursors)
        {
            string command = string.Empty;

            command = $"SELECT * from public.get_security_opening_data(";

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
        public List<CursorMapping> GetListOfSecurityOpeningDataOpeningCursors()
        {
            List<CursorMapping> str = new List<CursorMapping>()
            {
                new CursorMapping("pref_security_master","pref_security_master")

            };
            return str;
        }

        public async Task<int> UpdateOtp(UpdateOtpDataModel updateOtpDataModel, ServiceResponse obj, string otp)
        {

            //string otp = await GenerateOTP();

            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM update_otp(:pemail, :potp)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pemail", updateOtpDataModel.Email);
                cmd.Parameters.AddWithValue(":potp", otp);

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



        
        public async Task<int> VerifyOtp(VerifyOtpDataModel verifyOtpDataModel, ServiceResponse obj)
        {

            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM verify_otp(:pemail, :potp)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pemail", verifyOtpDataModel.Email);
                cmd.Parameters.AddWithValue(":potp", verifyOtpDataModel.Otp);

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

        public async Task<AuthorizeReponseProcModel> GetJwtToken(string userName, string password)
        {
            try
            {
                AuthorizeReponseProcModel authorizeReponseProcModel = new AuthorizeReponseProcModel();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_login_verify_token('{userName}', '{password}')", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        authorizeReponseProcModel = DataReaderExtensionMethod.ConvertToObject<AuthorizeReponseProcModel>(reader);

                    }
                }

                return authorizeReponseProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdatePassword(UpdatePasswordDataModel updatePasswordDataModel, ServiceResponse obj)
        {

            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM update_password(:puser_id, :ppassword)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":puser_id", updatePasswordDataModel.puser_id);
                cmd.Parameters.AddWithValue(":ppassword", updatePasswordDataModel.ppassword);

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

        public async Task<int> UpdateSecurityAnswer(UpdateSecurityAnswer updateSecurityAnswer, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();

            int result = 0;

            conn.TypeMapper.MapComposite<SecurityAnswerRelation>("type_security_answers");

            string query = $"SELECT * FROM update_security_answers(:puser_id, :parry_security_answers, :pcreated_by, :pcreated_date, :pmodified_by, :pmodified_date)";


            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":puser_id", updateSecurityAnswer.UserId);
                cmd.Parameters.AddWithValue(":parry_security_answers", updateSecurityAnswer.SecurityAnswerRelation);
                cmd.Parameters.AddWithValue(":pcreated_by", updateSecurityAnswer.CreatedBy);
                cmd.Parameters.AddWithValue(":pcreated_date", updateSecurityAnswer.CreatedDate);
                cmd.Parameters.AddWithValue(":pmodified_by", updateSecurityAnswer.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", updateSecurityAnswer.ModifiedDate);
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

        public async Task<int> VerifySecurityAnswer(VerifySecurityAnswer verifySecurityAnswer, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();

            int result = 0;

            conn.TypeMapper.MapComposite<SecurityAnswerRelation>("type_security_answers");

            string query = $"SELECT * FROM verify_security_answers(:puser_id, :parry_security_answers)";


            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":puser_id", verifySecurityAnswer.UserId);
                cmd.Parameters.AddWithValue(":parry_security_answers", verifySecurityAnswer.SecurityAnswerRelation);
                
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

        public async Task<VerifyUserName> VerifyUserName(string userName)
        {

            VerifyUserName verifyUserName = new VerifyUserName();
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            await using (var cmd = new NpgsqlCommand($"SELECT * from public.verify_username('{userName}')", conn))
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    verifyUserName = DataReaderExtensionMethod.ConvertToObject<VerifyUserName>(reader);
                }
            }

            return verifyUserName;
        }

        public async Task<int> CreateDomain(CreateDomainDto createDomainDto, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM create_domain(:pdomain_name, :pcreated_by, :pcreated_date, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
               
                cmd.Parameters.AddWithValue(":pdomain_name", createDomainDto.DomainName);
                cmd.Parameters.AddWithValue(":pcreated_by", createDomainDto.CreatedBy);
                cmd.Parameters.AddWithValue(":pcreated_date", createDomainDto.CreatedDate);
                cmd.Parameters.AddWithValue(":pmodified_by", createDomainDto.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", createDomainDto.ModifiedDate);

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

        public async Task<List<DomainListProcModel>> GetDomainList()
        {
            try
            {
                List<DomainListProcModel> domainListProcModel = new List<DomainListProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_domain_list()", conn))

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<DomainListProcModel>(reader);
                        domainListProcModel.Add(result);
                    }
                }

                return domainListProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DomainbyIdProcModel> GetDomainById(int domainId)
        {
            try
            {
                DomainbyIdProcModel domainbyIdProcModel = new DomainbyIdProcModel();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_domain_by_id({domainId})", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        domainbyIdProcModel = DataReaderExtensionMethod.ConvertToObject<DomainbyIdProcModel>(reader);
                    }
                }

                return domainbyIdProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteDomain(DeleteByIdModel deleteDomain)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM delete_domain(:pdomain_id, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pdomain_id", deleteDomain.Id);
                cmd.Parameters.AddWithValue(":pmodified_by", deleteDomain.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", deleteDomain.ModifiedDate);
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

        public async Task<int> UpdateDomain(UpdateDomainDto updateDomainDto, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM update_domain(:pdomain_master_id, :pdomain_name, :pstatus, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pdomain_master_id", updateDomainDto.DomainMasterId);
                cmd.Parameters.AddWithValue(":pdomain_name", updateDomainDto.DomainName);
                cmd.Parameters.AddWithValue(":pstatus", updateDomainDto.Status);
                cmd.Parameters.AddWithValue(":pmodified_by", updateDomainDto.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", updateDomainDto.ModifiedDate);

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

        public async Task<DomainClientOpeningData> GetDomainClientOpeningData()
        {
            try
            {
                DomainClientOpeningData domainClientOpeningData = new DomainClientOpeningData();

                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                string query = DynamicFormRefClientDomainOpeningDataQuery(GetListOfClientDomainOpeningDataOpeningCursors());

                await using (var cmd = new NpgsqlCommand(query, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {

                    reader.NextResult();
                    List<ClientData> clientMaster = new List<ClientData>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<ClientData>(reader);
                        clientMaster.Add(result);
                    }
                    domainClientOpeningData.ClientData = clientMaster;

                    reader.NextResult();
                    List<DomainData> domainMaster = new List<DomainData>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<DomainData>(reader);
                        domainMaster.Add(result);
                    }
                    domainClientOpeningData.DomainData = domainMaster;

                }

                return domainClientOpeningData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DynamicFormRefClientDomainOpeningDataQuery(List<CursorMapping> cursors)
        {
            string command = string.Empty;

            command = $"SELECT * from public.get_domain_client_opening_data(";

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
        public List<CursorMapping> GetListOfClientDomainOpeningDataOpeningCursors()
        {
            List<CursorMapping> str = new List<CursorMapping>()
            {
                new CursorMapping("pref_clients","pref_clients"),
                new CursorMapping("pref_domains","pref_domains")

            };
            return str;
        }

        public async Task<int> CreateClientDomainMapping(CreateClientDomainMapping createClientDomainMapping, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();

            int result = 0;

            conn.TypeMapper.MapComposite<ClientDomainRelation>("type_client_domain");

            string query = $"SELECT * FROM create_client_domain_mapping(:pclient_id, :parry_client_domains, :pcreated_by, :pcreated_date, :pmodified_by, :pmodified_date)";


            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pclient_id", createClientDomainMapping.ClientId);
                cmd.Parameters.AddWithValue(":parry_client_domains", createClientDomainMapping.ClientDomainRelation);
                cmd.Parameters.AddWithValue(":pcreated_by", createClientDomainMapping.CreatedBy);
                cmd.Parameters.AddWithValue(":pcreated_date", createClientDomainMapping.CreatedDate);
                cmd.Parameters.AddWithValue(":pmodified_by", createClientDomainMapping.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", createClientDomainMapping.ModifiedDate);
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

        public async Task<List<DomainClientMappingListProcModel>> GetDomainClientMappingList()
        {
            try
            {
                List<DomainClientMappingListProcModel> domainClientMappingListProcModels = new List<DomainClientMappingListProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_domain_client_mapping_list()", conn))

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<DomainClientMappingListProcModel>(reader);
                        domainClientMappingListProcModels.Add(result);
                    }
                }

                return domainClientMappingListProcModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteClientDomain(DeleteByIdModel deleteClientDomain)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM delete_client_domain_mapping(:pclient_id, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pclient_id", deleteClientDomain.Id);
                cmd.Parameters.AddWithValue(":pmodified_by", deleteClientDomain.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", deleteClientDomain.ModifiedDate);
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

        public async Task<DomainClientMappingByClientIdData> GetDomainClientMappingByClientId(int clientId)
        {
            try
            {
                DomainClientMappingByClientIdData domainClientMappingByClientId = new DomainClientMappingByClientIdData();

                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                string query = DynamicFormRefDomainClientByClientIdOpeningDataQuery(clientId, GetListOfDomainClientByClientIdOpeningDataOpeningCursors());

                await using (var cmd = new NpgsqlCommand(query, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {

                    reader.NextResult();
                    List<ClientDomainMaster> clientDomainMaster = new List<ClientDomainMaster>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<ClientDomainMaster>(reader);
                        clientDomainMaster.Add(result);

                    }
                    domainClientMappingByClientId.ClientDomainMaster = clientDomainMaster;
                }

                return domainClientMappingByClientId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DynamicFormRefDomainClientByClientIdOpeningDataQuery(int clienId, List<CursorMapping> cursors)
        {
            string command = string.Empty;

            command = $"SELECT * from public.get_domain_client_mapping_by_client_id({clienId},";

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


        public List<CursorMapping> GetListOfDomainClientByClientIdOpeningDataOpeningCursors()
        {
            List<CursorMapping> str = new List<CursorMapping>()
            {
                new CursorMapping("pref_client_domains","pref_client_domains")
            };
            return str;
        }

        public async Task<DomainClientMappingByClientIdData> GetDomainClientMappingByClientIdActive(int clientId)
        {
            try
            {
                DomainClientMappingByClientIdData domainClientMappingByClientId = new DomainClientMappingByClientIdData();

                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                string query = DynamicFormRefDomainClientByClientIdActiveOpeningDataQuery(clientId, GetListOfDomainClientByClientIdActiveOpeningDataOpeningCursors());

                await using (var cmd = new NpgsqlCommand(query, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {

                    reader.NextResult();
                    List<ClientDomainMaster> clientDomainMaster = new List<ClientDomainMaster>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<ClientDomainMaster>(reader);
                        clientDomainMaster.Add(result);

                    }
                    domainClientMappingByClientId.ClientDomainMaster = clientDomainMaster;
                }

                return domainClientMappingByClientId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DynamicFormRefDomainClientByClientIdActiveOpeningDataQuery(int clienId, List<CursorMapping> cursors)
        {
            string command = string.Empty;

            command = $"SELECT * from public.get_domain_client_mapping_by_client_id_active({clienId},";

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


        public List<CursorMapping> GetListOfDomainClientByClientIdActiveOpeningDataOpeningCursors()
        {
            List<CursorMapping> str = new List<CursorMapping>()
            {
                new CursorMapping("pref_client_domains","pref_client_domains")
            };
            return str;
        }

        public async Task<int> UpdateClientDomainMapping(UpdateClientDomainMapping updateClientDomainMapping, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();

            int result = 0;

            conn.TypeMapper.MapComposite<ClientDomainRelation>("type_client_domain");

            string query = $"SELECT * FROM update_domain_client_mapping(:pclient_id, :parry_client_domains, :pmodified_by, :pmodified_date, :pstatus)";


            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pclient_id", updateClientDomainMapping.ClientId);
                cmd.Parameters.AddWithValue(":parry_client_domains", updateClientDomainMapping.ClientDomainRelation);
                cmd.Parameters.AddWithValue(":pmodified_by", updateClientDomainMapping.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", updateClientDomainMapping.ModifiedDate);
                cmd.Parameters.AddWithValue(":pstatus", updateClientDomainMapping.Status);
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
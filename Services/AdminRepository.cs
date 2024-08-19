using ETA.API.ExtensionMethod;
using ETA.API.Models.StoreProcContextModel;
using ETA_API.Models.Referance;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;
using Npgsql;

namespace ETA_API.Services
{
    public class AdminRepository : IAdminRepository
    {
        private IConfiguration _configuration;
        public AdminRepository(IConfiguration Configuration)
        {
            _configuration = Configuration ??
           throw new ArgumentNullException(nameof(Configuration));
        }

        public async Task<List<RoleListProcModel>> GetRoleList()
        {
            try
            {
                List<RoleListProcModel> roleListProcModel = new List<RoleListProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_roles_list()", conn))

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<RoleListProcModel>(reader);
                        roleListProcModel.Add(result);
                    }
                }

                return roleListProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<RoleByIdData> GetRoleById(int roleId)
        {
            try
            {
                RoleByIdData roleByIdData = new RoleByIdData();

                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                string query = DynamicFormRefRoleByIdDataQuery(roleId, GetListOfRoleByDataOpeningCursors());

                await using (var cmd = new NpgsqlCommand(query, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    reader.NextResult();
                    RoleByIdMasterData roleByIdMaster = new RoleByIdMasterData();
                    while (await reader.ReadAsync())
                    {
                        roleByIdMaster = DataReaderExtensionMethod.ConvertToObject<RoleByIdMasterData>(reader);
                        //roleByIdMaster.Add(result);
                    }
                    roleByIdData.RoleByIdMasterData = roleByIdMaster;

                    reader.NextResult();
                    List<PermissionByIdMasterData> permissionByIdMaster = new List<PermissionByIdMasterData>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<PermissionByIdMasterData>(reader);
                        permissionByIdMaster.Add(result);
                    }
                    roleByIdData.PermissionByIdMasterData = permissionByIdMaster;
                }

                return roleByIdData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DynamicFormRefRoleByIdDataQuery(int roleId, List<CursorMapping> cursors)
        {
            string command = string.Empty;

            command = $"SELECT * from public.get_role_by_id({roleId},";

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
        public List<CursorMapping> GetListOfRoleByDataOpeningCursors()
        {
            List<CursorMapping> str = new List<CursorMapping>()
            {
                new CursorMapping("pref_roles","pref_roles"),
                new CursorMapping("pref_permissions","pref_permissions")
            };
            return str;
        }

        
        public async Task<int> DeleteRole(DeleteByIdModel deleteRole)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM delete_role(:prole_id, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":prole_id", deleteRole.Id);
                cmd.Parameters.AddWithValue(":pmodified_by", deleteRole.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", deleteRole.ModifiedDate);
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

        public async Task<RoleOpeningData> GetRoleOpeningData()
        {
            try
            {
                RoleOpeningData roleOpeningData = new RoleOpeningData();

                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                string query = DynamicFormRefRolePermissionOpeningDataQuery(GetListOfRolePermissionOpeningDataOpeningCursors());

                await using (var cmd = new NpgsqlCommand(query, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    reader.NextResult();
                    List<PermissionAdminMaster> permissionAdminMaster = new List<PermissionAdminMaster>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<PermissionAdminMaster>(reader);
                        permissionAdminMaster.Add(result);
                    }
                    roleOpeningData.PermissionAdminMasters = permissionAdminMaster;
                }

                return roleOpeningData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DynamicFormRefRolePermissionOpeningDataQuery(List<CursorMapping> cursors)
        {
            string command = string.Empty;

            command = $"SELECT * from public.get_role_opening_data(";

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
        public List<CursorMapping> GetListOfRolePermissionOpeningDataOpeningCursors()
        {
            List<CursorMapping> str = new List<CursorMapping>()
            {
                new CursorMapping("pref_permissions","pref_permissions")
            };
            return str;
        }

        public async Task<int> CreateRole(CreateRole createRole, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();

            int result = 0;

            conn.TypeMapper.MapComposite<RolePermissionRelation>("type_role_permissions");

            string query = $"SELECT * FROM create_role(:prole_name, :pdescription, :parry_role_permissions, :pcreated_by, :pcreated_date, :pmodified_by, :pmodified_date)";


            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":prole_name", createRole.RoleName);
                cmd.Parameters.AddWithValue(":pdescription", createRole.Description);
                cmd.Parameters.AddWithValue(":parry_role_permissions", createRole.RolePermissionRelations);
                cmd.Parameters.AddWithValue(":pcreated_by", createRole.CreatedBy);
                cmd.Parameters.AddWithValue(":pcreated_date", createRole.CreatedDate);
                cmd.Parameters.AddWithValue(":pmodified_by", createRole.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", createRole.ModifiedDate);
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

        public async Task<int> UpdateRole(UpdateRole updateRole, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();

            int result = 0;

            conn.TypeMapper.MapComposite<RolePermissionRelation>("type_role_permissions");

            string query = $"SELECT * FROM update_role(:prole_id, :prole_name, :pdescription, :pstatus, :parry_role_permissions, :pmodified_by, :pmodified_date)";


            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":prole_id", updateRole.RoleId);
                cmd.Parameters.AddWithValue(":prole_name", updateRole.RoleName);
                cmd.Parameters.AddWithValue(":pdescription", updateRole.Description);
                cmd.Parameters.AddWithValue(":pstatus", updateRole.Status);
                cmd.Parameters.AddWithValue(":parry_role_permissions", updateRole.RolePermissionRelations);
                cmd.Parameters.AddWithValue(":pmodified_by", updateRole.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", updateRole.ModifiedDate);
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

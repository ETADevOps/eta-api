using ETA.API.DbContexts;
using ETA.API.ExtensionMethod;
using ETA.API.Models.StoreProcContextModel;
using ETA_API.Models.Referance;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Data;
using System.Linq.Dynamic.Core;

namespace ETA_API.Services
{
    public class PatientRepository : IPatientrepository
    {
        //these declarations represent various dependencies and resources needed by the class
        private IConfiguration _configuration;
        private CourseLibraryContext _context;


        // This pattern is often used to enforce the requirement of providing all necessary dependencies for the proper functioning of the class.
        public PatientRepository(IConfiguration Configuration, CourseLibraryContext userRepositoryContext)
        {
            _configuration = Configuration ??
            throw new ArgumentNullException(nameof(Configuration));
            _context = userRepositoryContext ??
               throw new ArgumentNullException(nameof(userRepositoryContext));
        }
        public async Task<List<PatientsProcModel>> GetPatientsList(int clientid, int providerId, int patientId)
        {
            try
            {
                List<PatientsProcModel> patientList = new List<PatientsProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_patients_list({clientid}, {providerId}, {patientId})", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<PatientsProcModel>(reader);
                        patientList.Add(result);
                    }
                }

                return patientList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PatientByIdData> GetPatientsbyId(int patientId)
        {
            try
            {
                PatientByIdData patientByIdData = new PatientByIdData();

                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                string query = DynamicFormRefpatientByIdDataQuery(patientId, GetPatientByIdCursors());

                await using (var cmd = new NpgsqlCommand(query, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    reader.NextResult();
                    List<PatientDetailsReportMaster> patientReportMaster = new List<PatientDetailsReportMaster>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<PatientDetailsReportMaster>(reader);
                        patientReportMaster.Add(result);
                    }
                    patientByIdData.PatientDetailsReportMasters = patientReportMaster;

                    reader.NextResult();
                    List<PatientAttachmentReportMaster> patientAttachment = new List<PatientAttachmentReportMaster>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<PatientAttachmentReportMaster>(reader);
                        patientAttachment.Add(result);
                    }
                    patientByIdData.PatientAttachmentReportMasters = patientAttachment;
                }

                return patientByIdData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DynamicFormRefpatientByIdDataQuery(int patientId, List<CursorMapping> cursors)
        {
            string command = string.Empty;

            command = $"SELECT * from public.get_patient_by_id({patientId},";

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
        public List<CursorMapping> GetPatientByIdCursors()
        {
            List<CursorMapping> str = new List<CursorMapping>()
            {
                new CursorMapping("pref_patient_details","pref_patient_details"),
                new CursorMapping("pref_patient_attachments","pref_patient_attachments")
            };
            return str;
        }

        public async Task<int> CreatePatient(CreatePatientDto createPatient, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            conn.TypeMapper.MapComposite<PatientAttachment>("type_patient_attachments");

            string query = $"SELECT * FROM create_patient(:pfirst_name, :plast_name, :pgender, :pdate_of_birth, :paddress, :pcity, :pstate, :pzip, :pemail, :pspecimen_type, :pcollection_method, :pcollection_date, :pprovider_id, :pclient_id, :pgene_file_url, :parray_attachments, :pcreated_by, :pcreated_date, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pfirst_name", createPatient.FirstName);
                cmd.Parameters.AddWithValue(":plast_name", createPatient.LastName);
                cmd.Parameters.AddWithValue(":pgender", createPatient.Gender);

                if (createPatient.DateOfBirth == null)
                    cmd.Parameters.AddWithValue(":pdate_of_birth", NpgsqlTypes.NpgsqlDbType.Date, default(DateTime));
                else
                    cmd.Parameters.AddWithValue(":pdate_of_birth", NpgsqlTypes.NpgsqlDbType.Date, createPatient.DateOfBirth);

                cmd.Parameters.AddWithValue(":paddress", createPatient.Address);
                cmd.Parameters.AddWithValue(":pcity", createPatient.City);
                cmd.Parameters.AddWithValue(":pstate", createPatient.State);
                cmd.Parameters.AddWithValue(":pzip", createPatient.Zip);
                cmd.Parameters.AddWithValue(":pemail", createPatient.Email);
                cmd.Parameters.AddWithValue(":pspecimen_type", createPatient.SpecimenType);
                cmd.Parameters.AddWithValue(":pcollection_method", createPatient.CollectionMethod);
                cmd.Parameters.AddWithValue(":pcollection_date", createPatient.CollectionDate);
                cmd.Parameters.AddWithValue(":pprovider_id", createPatient.ProviderId);
                cmd.Parameters.AddWithValue(":pclient_id", createPatient.ClientId);
                cmd.Parameters.AddWithValue(":pgene_file_url", String.IsNullOrEmpty(createPatient.PatientGeneFile) ? "" : "Processing");
                cmd.Parameters.AddWithValue(":parray_attachments", createPatient.PatientAttachments);
                cmd.Parameters.AddWithValue(":pcreated_by", createPatient.CreatedBy);
                cmd.Parameters.AddWithValue(":pcreated_date", createPatient.CreatedDate);
                cmd.Parameters.AddWithValue(":pmodified_by", createPatient.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", createPatient.ModifiedDate);

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
        public async Task<int> UpdatePatient(CreatePatientDto createpatient, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            conn.TypeMapper.MapComposite<PatientAttachment>("type_patient_attachments");

            string query = $"SELECT * FROM update_patient(:ppatient_id, :pfirst_name, :plast_name, :pgender, :pdate_of_birth, :paddress, :pcity, :pstate, :pzip, :pemail, :pspecimen_type, :pcollection_method, :pcollection_date, :pprovider_id, :pclient_id, :pgene_file_url, :parray_attachments, :pmodified_by, :pmodified_date, :pstatus)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":ppatient_id", createpatient.PatientId);
                cmd.Parameters.AddWithValue(":pfirst_name", createpatient.FirstName);
                cmd.Parameters.AddWithValue(":plast_name", createpatient.LastName);
                cmd.Parameters.AddWithValue(":pgender", createpatient.Gender);

                if (createpatient.DateOfBirth == null)
                    cmd.Parameters.AddWithValue(":pdate_of_birth", NpgsqlTypes.NpgsqlDbType.Date, default(DateTime));
                else
                    cmd.Parameters.AddWithValue(":pdate_of_birth", NpgsqlTypes.NpgsqlDbType.Date, createpatient.DateOfBirth);

                cmd.Parameters.AddWithValue(":paddress", createpatient.Address);
                cmd.Parameters.AddWithValue(":pcity", createpatient.City);
                cmd.Parameters.AddWithValue(":pstate", createpatient.State);
                cmd.Parameters.AddWithValue(":pzip", createpatient.Zip);
                cmd.Parameters.AddWithValue(":pemail", createpatient.Email);
                cmd.Parameters.AddWithValue(":pspecimen_type", createpatient.SpecimenType);
                cmd.Parameters.AddWithValue(":pcollection_method", createpatient.CollectionMethod);
                cmd.Parameters.AddWithValue(":pcollection_date", createpatient.CollectionDate);
                cmd.Parameters.AddWithValue(":pprovider_id", createpatient.ProviderId);
                cmd.Parameters.AddWithValue(":pclient_id", createpatient.ClientId);
                cmd.Parameters.AddWithValue(":pgene_file_url", String.IsNullOrEmpty(createpatient.PatientGeneFile) ? "" : createpatient.PatientGeneFile.Contains("blob.core") ? createpatient.PatientGeneFile : "Processing");
                cmd.Parameters.AddWithValue(":parray_attachments", createpatient.PatientAttachments);
                cmd.Parameters.AddWithValue(":pmodified_by", createpatient.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", createpatient.ModifiedDate);
                cmd.Parameters.AddWithValue(":pstatus", createpatient.Status);

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
        public async Task<int> DeletePatient(DeleteByIdModel deletePatient)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM delete_patient(:ppatient_id, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":ppatient_id", deletePatient.Id);
                cmd.Parameters.AddWithValue(":pmodified_by", deletePatient.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", deletePatient.ModifiedDate);
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
        public async Task<List<PatientCityState>> GetZIPSearch(string searchText)
        {
            try
            {
                if (searchText == null)
                {
                    throw new ArgumentNullException(nameof(PatientCityState));
                }
                var collection = _context.PatientCityState as IQueryable<PatientCityState>;
                collection = _context.PatientCityState.FromSqlRaw("select * from city_states");
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    string searchQuery = searchText.Trim().ToLower();
                    collection = collection.Where(u => EF.Functions.Like(u.zip.ToLower(), searchQuery + "%"));
                }
                List<PatientCityState> listSearchZip = new List<PatientCityState>();
                listSearchZip = collection.Skip(0).Take(10).ToList();
                return listSearchZip;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<PatientsOpeningDataModel> GetPatientsOpeningData(int clientId)
        {
            try
            {
                PatientsOpeningDataModel patientsOpeningData = new PatientsOpeningDataModel();

                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                string query = DynamicFormRefPatientsOpeningDataQuery(clientId, GetListOfPatientsOpeningDataOpeningCursors());

                await using (var cmd = new NpgsqlCommand(query, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    reader.NextResult();
                    List<ClientPatientOpeningData> lstClients = new List<ClientPatientOpeningData>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<ClientPatientOpeningData>(reader);
                        lstClients.Add(result);
                    }
                    patientsOpeningData.ClientPatientOpeningDatas = lstClients;
                }

                return patientsOpeningData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string DynamicFormRefPatientsOpeningDataQuery(int clientId, List<CursorMapping> cursors)
        {
            string command = string.Empty;

            command = $"SELECT * from public.get_patients_opening_data({clientId},";

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
        public List<CursorMapping> GetListOfPatientsOpeningDataOpeningCursors()
        {
            List<CursorMapping> str = new List<CursorMapping>()
            {
                new CursorMapping("pref_clients","pref_clients")
            };
            return str;
        }
        public async Task<List<PatientGeneDumpProcModel>> GetPatientsGeneDumpbyId(int patientId, int reportMasterId)
        {
            try
            {
                List<PatientGeneDumpProcModel> patientGeneDumpByIdProcModel = new List<PatientGeneDumpProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_patient_gene_dump_by_id({patientId}, {reportMasterId})", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<PatientGeneDumpProcModel>(reader);
                        patientGeneDumpByIdProcModel.Add(result);
                    }
                }

                return patientGeneDumpByIdProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<string> GetClientsReportTemplate(int patientId, int reportMasterId)
        {

            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            string result = "";

            string query = $"SELECT * FROM get_clients_report_templates(:ppatient_id, :preport_master_id)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":ppatient_id", patientId);
                cmd.Parameters.AddWithValue(":preport_master_id", reportMasterId);

                try
                {
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                                result = reader.GetString(0);
                            
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

        public async Task<PatientsReportDataModel> GetPatientsReportData(int patientId, int reportMasterId)
        {
            try
            {
                PatientsReportDataModel patientsReportData = new PatientsReportDataModel();

                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                string query = DynamicFormRefPatientsReportDataQuery(patientId, reportMasterId, GetListOfPatientsReportDataReportCursors());

                await using (var cmd = new NpgsqlCommand(query, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    reader.NextResult();
                    List<ReportCategoryInfo> categoryInfo = new List<ReportCategoryInfo>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<ReportCategoryInfo>(reader);
                        categoryInfo.Add(result);
                    }
                    patientsReportData.ReportCategoryInfo = categoryInfo;

                    reader.NextResult();
                    while (await reader.ReadAsync())
                    {
                        patientsReportData.ReportBasicInfo = DataReaderExtensionMethod.ConvertToObject<ReportBasicInfo>(reader);
                    }

                    reader.NextResult();
                    while (await reader.ReadAsync())
                    {
                        patientsReportData.ReportDynamicInfo = DataReaderExtensionMethod.ConvertToObject<ReportDynamicInfo>(reader);
                    }

                    reader.NextResult();
                    List<PatientGeneInfo> lstPatientGeneInfo = new List<PatientGeneInfo>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<PatientGeneInfo>(reader);
                        lstPatientGeneInfo.Add(result);
                    }
                    patientsReportData.PatientGenes = lstPatientGeneInfo;
                }

                return patientsReportData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string DynamicFormRefPatientsReportDataQuery(int patientId, int reportMasterId, List<CursorMapping> cursors)
        {
            string command = string.Empty;

            command = $"SELECT * from public.generate_patient_report_details({patientId}, {reportMasterId},";

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
        public List<CursorMapping> GetListOfPatientsReportDataReportCursors()
        {
            List<CursorMapping> str = new List<CursorMapping>()
            {
                new CursorMapping("pref_report_category_info","pref_report_category_info"),
                new CursorMapping("pref_report_basic_info","pref_report_basic_info"),
                new CursorMapping("pref_report_dynamic_info","pref_report_dynamic_info"),
                new CursorMapping("pref_patient_gene_info","pref_patient_gene_info")
            };
            return str;
        }
        public async Task<int> UpdatePatientReportUrl(int patientId, string reportUrl, int reportMasterId, int generatedBy, DateTime? generatedDate)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM update_patient_gene_report(:pgenerated_by, :pgenerated_date, :ppatient_id, :preport_master_id, :preport_url)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pgenerated_by", generatedBy);
                cmd.Parameters.AddWithValue(":pgenerated_date", generatedDate);
                cmd.Parameters.AddWithValue(":ppatient_id", patientId);
                cmd.Parameters.AddWithValue(":preport_master_id", reportMasterId);
                cmd.Parameters.AddWithValue(":preport_url", reportUrl);

                try
                {
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (reader["update_patient_gene_report"] != null && reader["update_patient_gene_report"] != DBNull.Value)
                            {
                                result = reader.GetInt32(0);
                            }
                            else
                            {
                                result = 0;
                            }
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
        public async Task<string> GetColorUrlByResult(int categoryId, int subcategoryId, string geneResult, string resultColor)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            string result = "";

            string query = $"SELECT * FROM get_color_url_by_result(:pcategory_id, :psub_category_id, :presult, :presult_color)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pcategory_id", categoryId);
                cmd.Parameters.AddWithValue(":psub_category_id", subcategoryId);
                cmd.Parameters.AddWithValue(":presult", geneResult);
                cmd.Parameters.AddWithValue(":presult_color", resultColor);

                try
                {
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (reader["get_color_url_by_result"] != null && reader["get_color_url_by_result"] != DBNull.Value)
                            {
                                result = reader.GetString(0);
                            }
                            else
                            {
                                //Do Nothing
                            }
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

        public async Task<PatientReportDetailsData> GetPatientReportDetails(int patientId)
        {
            try
            {
                PatientReportDetailsData patientReportData = new PatientReportDetailsData();

                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                string query = DynamicFormRefPatientReportDataQuery(patientId, GetPatientReportDetailsCursors());

                await using (var cmd = new NpgsqlCommand(query, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    reader.NextResult();
                    List<PatientReportDetailsModel> patientReportDetailsMaster = new List<PatientReportDetailsModel>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<PatientReportDetailsModel>(reader);
                        patientReportDetailsMaster.Add(result);
                    }
                    patientReportData.PatientReportDetailsModels = patientReportDetailsMaster;

                    reader.NextResult();
                    List<PatientReportAttachmentDetailsModel> patientReportAttachment = new List<PatientReportAttachmentDetailsModel>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<PatientReportAttachmentDetailsModel>(reader);
                        patientReportAttachment.Add(result);
                    }
                    patientReportData.PatientReportAttachmentDetailsModels = patientReportAttachment;
                }

                return patientReportData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DynamicFormRefPatientReportDataQuery(int patientId, List<CursorMapping> cursors)
        {
            string command = string.Empty;

            command = $"SELECT * from public.get_patient_report_details({patientId},";

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
        public List<CursorMapping> GetPatientReportDetailsCursors()
        {
            List<CursorMapping> str = new List<CursorMapping>()
            {
                new CursorMapping("pref_report_details","pref_report_details"),
                new CursorMapping("pref_patient_attachments","pref_patient_attachments")
            };
            return str;
        }

        public async Task<List<PatientReportHistoryModel>> GetPatientReportHistoryList(int clientId)
        {
            try
            {
                List<PatientReportHistoryModel> patientReportHistoryList = new List<PatientReportHistoryModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from  public.get_patient_report_history_list({clientId})", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<PatientReportHistoryModel>(reader);
                        patientReportHistoryList.Add(result);
                    }
                }

                return patientReportHistoryList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ReportOpeningData> GenerateReportOpeningData(int clientId)
        {
            try
            {
                ReportOpeningData reportOpeningData = new ReportOpeningData();

                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                string query = DynamicFormRefReportOpeningDataQuery(clientId, GetListOfReportOpeningDataOpeningCursors());

                await using (var cmd = new NpgsqlCommand(query, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    reader.NextResult();
                    List<ClientReportMasterOpening> clientReport = new List<ClientReportMasterOpening>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<ClientReportMasterOpening>(reader);
                        clientReport.Add(result);
                    }
                    reportOpeningData.ClientReportMasterOpening = clientReport;
                }

                return reportOpeningData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DynamicFormRefReportOpeningDataQuery(int clientId, List<CursorMapping> cursors)
        {
            string command = string.Empty;

            command = $"SELECT * from public.generate_report_opening_data({clientId},";

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
        public List<CursorMapping> GetListOfReportOpeningDataOpeningCursors()
        {
            List<CursorMapping> str = new List<CursorMapping>()
            {
                new CursorMapping("pref_client_master","pref_client_master")
            };
            return str;
        }

        public async Task<List<PatientHistoryModel>> GetPatientHistory(int patientId, int reportId)
        {
            try
            {
                List<PatientHistoryModel> patientHistory = new List<PatientHistoryModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from  public.get_patient_history({patientId}, {reportId})", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<PatientHistoryModel>(reader);
                        patientHistory.Add(result);
                    }
                }

                return patientHistory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PatientListByClientIdData> GetPatientListByClientId(int clientId)
        {
            try
            {
                PatientListByClientIdData patientClientOpeningData = new PatientListByClientIdData();

                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                string query = DynamicFormRefPatientListByClientIdDataQuery(clientId, GetListOfPatientListByClientIdOpeningDataOpeningCursors());

                await using (var cmd = new NpgsqlCommand(query, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    reader.NextResult();
                    List<PatientData> patientData = new List<PatientData>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<PatientData>(reader);
                        patientData.Add(result);
                    }
                    patientClientOpeningData.PatientData = patientData;

                    reader.NextResult();
                    List<CategoryData> categoryData = new List<CategoryData>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<CategoryData>(reader);
                        categoryData.Add(result);
                    }
                    patientClientOpeningData.CategoryData = categoryData;
                }

                return patientClientOpeningData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DynamicFormRefPatientListByClientIdDataQuery(int clientId, List<CursorMapping> cursors)
        {
            string command = string.Empty;

            command = $"SELECT * from public.get_patient_list_by_client_id({clientId},";

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

        public List<CursorMapping> GetListOfPatientListByClientIdOpeningDataOpeningCursors()
        {
            List<CursorMapping> str = new List<CursorMapping>()
            {
                new CursorMapping("pref_patients","pref_patients"),
                new CursorMapping("pref_category","pref_category")
            };
            return str;
        }

        public async Task<List<PatientListByProviderIdProcModel>> GetPatientListByProviderId(int providerId)
        {
            try
            {
                List<PatientListByProviderIdProcModel> patientsByProviderIdProcModel = new List<PatientListByProviderIdProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_patient_list_by_provider_id({providerId})", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<PatientListByProviderIdProcModel>(reader);
                        patientsByProviderIdProcModel.Add(result);
                    }
                }

                return patientsByProviderIdProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CategoryReportData> GetCategoryByPatientId(int patientId)
        {
            try
            {
                CategoryReportData reportOpeningData = new CategoryReportData();

                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                string query = DynamicFormRefReportDetailsOpeningDataQuery(patientId, GetListOfPatientByCategoryDataOpeningCursors());

                await using (var cmd = new NpgsqlCommand(query, conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    reader.NextResult();
                    List<CategoryReportDetails> categoryReport = new List<CategoryReportDetails>();
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<CategoryReportDetails>(reader);
                        categoryReport.Add(result);
                    }
                    reportOpeningData.CategoryReportDetails = categoryReport;
                }

                return reportOpeningData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DynamicFormRefReportDetailsOpeningDataQuery(int patientId, List<CursorMapping> cursors)
        {
            string command = string.Empty;

            command = $"SELECT * from public.get_category_by_paitent_id({patientId},";

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
        public List<CursorMapping> GetListOfPatientByCategoryDataOpeningCursors()
        {
            List<CursorMapping> str = new List<CursorMapping>()
            {
                new CursorMapping("pref_report_details","pref_report_details")
            };
            return str;
        }

        public async Task<ReportColoursProcModel> GetReportColorByClientId(int clientId)
        {
            try
            {
                ReportColoursProcModel reportColorByClientIdProcModel = new ReportColoursProcModel();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_report_colors_by_client_id({clientId})", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        reportColorByClientIdProcModel = DataReaderExtensionMethod.ConvertToObject<ReportColoursProcModel>(reader);
                    }
                }

                return reportColorByClientIdProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateReportColorByClientId(UpdateReportColorDto updateReportColor, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM update_report_colors_by_client_id(:pclient_id, :preport_bg_color, :preport_heading_color, :preport_sub_heading_color, :preport_bg_font_color, :preport_heading_font_color, :preport_sub_heading_font_color)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pclient_id", updateReportColor.ClientId);
                cmd.Parameters.AddWithValue(":preport_bg_color", updateReportColor.ReportBgColor);
                cmd.Parameters.AddWithValue(":preport_heading_color", updateReportColor.ReportHeadingColor);
                cmd.Parameters.AddWithValue(":preport_sub_heading_color", updateReportColor.ReportSubHeadingColor);
                cmd.Parameters.AddWithValue(":preport_bg_font_color", updateReportColor.ReportBgFontColor);
                cmd.Parameters.AddWithValue(":preport_heading_font_color", updateReportColor.ReportHeadingFontColor);
                cmd.Parameters.AddWithValue(":preport_sub_heading_font_color", updateReportColor.ReportSubHeadingFontColor);


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

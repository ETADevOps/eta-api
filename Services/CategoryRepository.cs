using ETA.API.ExtensionMethod;
using ETA.API.Models.StoreProcContextModel;
using ETA_API.Models.Referance;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;
using Npgsql;

namespace ETA_API.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private IConfiguration _configuration;
        public CategoryRepository(IConfiguration Configuration)
        {
            _configuration = Configuration ??
           throw new ArgumentNullException(nameof(Configuration));
        }

        public async Task<List<CategoryProcModel>> GetCategoryList()
        {
            try
            {
                List<CategoryProcModel> categoryProcModel = new List<CategoryProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_category_list()", conn))

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<CategoryProcModel>(reader);
                        categoryProcModel.Add(result);
                    }
                }

                return categoryProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CreateCategory(CreateCategoryDto createCategoryDto, ServiceResponse Obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();

            int result = 0;

            string query = $"SELECT * FROM create_category(:pcategory_name, :preport_gene_predisposition, :preport_important_takeways, :pcreated_by, :pcreated_date, :pmodified_by, :pmodified_date)";


            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pcategory_name", createCategoryDto.CategoryName);
                cmd.Parameters.AddWithValue(":preport_gene_predisposition", createCategoryDto.ReportGenePredisposition);
                cmd.Parameters.AddWithValue(":preport_important_takeways", createCategoryDto.ReportImportantTakeways);
                cmd.Parameters.AddWithValue(":pcreated_by", createCategoryDto.CreatedBy);
                cmd.Parameters.AddWithValue(":pcreated_date", createCategoryDto.CreatedDate);
                cmd.Parameters.AddWithValue(":pmodified_by", createCategoryDto.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", createCategoryDto.ModifiedDate);
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

        public async Task<int> DeleteCategory(DeleteByIdModel deleteCategory)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM delete_category(:pcategory_id, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pcategory_id", deleteCategory.Id);
                cmd.Parameters.AddWithValue(":pmodified_by", deleteCategory.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", deleteCategory.ModifiedDate);
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

        public async Task<CategoryProcModel> GetCategoryById(int categoryId)
        {
            try
            {
                CategoryProcModel categoryByIdProcModel = new CategoryProcModel();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_category_by_id({categoryId})", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        categoryByIdProcModel = DataReaderExtensionMethod.ConvertToObject<CategoryProcModel>(reader);
                    }
                }

                return categoryByIdProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> UpdateCategory(UpdateCategoryDto updateCategoryDto, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM update_category(:preport_master_id, :pcategory_name, :preport_gene_predisposition, :preport_important_takeways, :pstatus, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":preport_master_id", updateCategoryDto.ReportMasterId);
                cmd.Parameters.AddWithValue(":pcategory_name", updateCategoryDto.CategoryName);
                cmd.Parameters.AddWithValue(":preport_gene_predisposition", updateCategoryDto.ReportGenePredisposition);
                cmd.Parameters.AddWithValue(":preport_important_takeways", updateCategoryDto.ReportImportantTakeways);
                cmd.Parameters.AddWithValue(":pstatus", updateCategoryDto.Status);
                cmd.Parameters.AddWithValue(":pmodified_by", updateCategoryDto.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", updateCategoryDto.ModifiedDate);

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

        public async Task<List<CategoryOpeningDataProcModel>> GetCategoryOpeningData()
        {
            try
            {
                List<CategoryOpeningDataProcModel> categoryDataProcModel = new List<CategoryOpeningDataProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_category_opening_data()", conn))

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<CategoryOpeningDataProcModel>(reader);
                        categoryDataProcModel.Add(result);
                    }
                }

                return categoryDataProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CreateReportCategory(CreateReportCategoryDto createReportCategoryDto, ServiceResponse Obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();

            int result = 0;

            string query = $"SELECT * FROM create_report_category(:pcategory_name, :pcategory_introduction, :preport_master_id, :pcreated_by, :pcreated_date, :pmodified_by, :pmodified_date)";


            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pcategory_name", createReportCategoryDto.CategoryName);
                cmd.Parameters.AddWithValue(":pcategory_introduction", createReportCategoryDto.CategoryIntroduction);
                cmd.Parameters.AddWithValue(":preport_master_id", createReportCategoryDto.ReportMasterId);
                cmd.Parameters.AddWithValue(":pcreated_by", createReportCategoryDto.CreatedBy);
                cmd.Parameters.AddWithValue(":pcreated_date", createReportCategoryDto.CreatedDate);
                cmd.Parameters.AddWithValue(":pmodified_by", createReportCategoryDto.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", createReportCategoryDto.ModifiedDate);
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

        public async Task<List<ReportCategoryListProcModel>> GetReportCategoryList()
        {
            try
            {
                List<ReportCategoryListProcModel> reportCategoryProcModel = new List<ReportCategoryListProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_report_category_list()", conn))

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<ReportCategoryListProcModel>(reader);
                        reportCategoryProcModel.Add(result);
                    }
                }

                return reportCategoryProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ReportCategoryByIdProcModel> GetReportCategoryById(int categoryId)
        {
            try
            {
                ReportCategoryByIdProcModel reportCategoryByIdProcModel = new ReportCategoryByIdProcModel();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_report_category_by_id({categoryId})", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        reportCategoryByIdProcModel = DataReaderExtensionMethod.ConvertToObject<ReportCategoryByIdProcModel>(reader);
                    }
                }

                return reportCategoryByIdProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteReportCategory(DeleteByIdModel deleteReportCategory)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM delete_report_category(:pcategory_id, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pcategory_id", deleteReportCategory.Id);
                cmd.Parameters.AddWithValue(":pmodified_by", deleteReportCategory.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", deleteReportCategory.ModifiedDate);
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

        public async Task<int> UpdateReportCategory(UpdateReportCategoryDto updateReportCategoryDto, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM update_report_category(:pcategory_id, :pcategory_name, :pcategory_description, :preport_master_id, :pstatus, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pcategory_id", updateReportCategoryDto.CategoryId);
                cmd.Parameters.AddWithValue(":pcategory_name", updateReportCategoryDto.CategoryName);
                cmd.Parameters.AddWithValue(":pcategory_description", updateReportCategoryDto.CategoryDescription);
                cmd.Parameters.AddWithValue(":preport_master_id", updateReportCategoryDto.ReportMasterId);
                cmd.Parameters.AddWithValue(":pstatus", updateReportCategoryDto.Status);
                cmd.Parameters.AddWithValue(":pmodified_by", updateReportCategoryDto.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", updateReportCategoryDto.ModifiedDate);

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

        public async Task<List<SubCategoryOpeningDataProcModel>> GetSubCategoryOpeningData()
        {
            try
            {
                List<SubCategoryOpeningDataProcModel> subCategoryDataProcModel = new List<SubCategoryOpeningDataProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_sub_category_opening_data()", conn))

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<SubCategoryOpeningDataProcModel>(reader);
                        subCategoryDataProcModel.Add(result);
                    }
                }

                return subCategoryDataProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CreateSubCategory(CreateSubCategoryDto createSubCategoryDto, ServiceResponse Obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();

            int result = 0;

            string query = $"SELECT * FROM create_sub_category(:psub_category_name, :psub_category_introduction, :pcategory_id, :pcreated_by, :pcreated_date, :pmodified_by, :pmodified_date)";


            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":psub_category_name", createSubCategoryDto.SubCategoryName);
                cmd.Parameters.AddWithValue(":psub_category_introduction", createSubCategoryDto.SubCategoryIntroduction);
                cmd.Parameters.AddWithValue(":pcategory_id", createSubCategoryDto.CategoryId);
                cmd.Parameters.AddWithValue(":pcreated_by", createSubCategoryDto.CreatedBy);
                cmd.Parameters.AddWithValue(":pcreated_date", createSubCategoryDto.CreatedDate);
                cmd.Parameters.AddWithValue(":pmodified_by", createSubCategoryDto.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", createSubCategoryDto.ModifiedDate);
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

        public async Task<int> UpdateSubCategory(UpdateSubCategoryDto updateSubCategoryDto, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM update_sub_category(:psub_category_id, :psub_category_name, :psub_category_introduction, :pcategory_id, :pstatus, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":psub_category_id", updateSubCategoryDto.SubCategoryId);
                cmd.Parameters.AddWithValue(":psub_category_name", updateSubCategoryDto.SubCategoryName);
                cmd.Parameters.AddWithValue(":psub_category_introduction", updateSubCategoryDto.SubCategoryIntroduction);
                cmd.Parameters.AddWithValue(":pcategory_id", updateSubCategoryDto.CategoryId);
                cmd.Parameters.AddWithValue(":pstatus", updateSubCategoryDto.Status);
                cmd.Parameters.AddWithValue(":pmodified_by", updateSubCategoryDto.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", updateSubCategoryDto.ModifiedDate);

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

        public async Task<SubCategoryByIdProcModel> GetSubCategoryById(int subCategoryId)
        {
            try
            {
                SubCategoryByIdProcModel subCategoryByIdProcModel = new SubCategoryByIdProcModel();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_sub_category_by_id({subCategoryId})", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        subCategoryByIdProcModel = DataReaderExtensionMethod.ConvertToObject<SubCategoryByIdProcModel>(reader);
                    }
                }

                return subCategoryByIdProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SubCategoryListProcModel>> GetSubCategoryList()
        {
            try
            {
                List<SubCategoryListProcModel> subCategoryProcModel = new List<SubCategoryListProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_sub_category_list()", conn))

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<SubCategoryListProcModel>(reader);
                        subCategoryProcModel.Add(result);
                    }
                }

                return subCategoryProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteSubCategory(DeleteByIdModel deleteSubCategory)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM delete_sub_category(:psub_category_id, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":psub_category_id", deleteSubCategory.Id);
                cmd.Parameters.AddWithValue(":pmodified_by", deleteSubCategory.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", deleteSubCategory.ModifiedDate);
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

        public async Task<List<CategoryGeneOpeningDataProcModel>> GetCategoryGeneOpeningData()
        {
            try
            {
                List<CategoryGeneOpeningDataProcModel> categoryGeneDataProcModel = new List<CategoryGeneOpeningDataProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_category_gene_opening_data()", conn))

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<CategoryGeneOpeningDataProcModel>(reader);
                        categoryGeneDataProcModel.Add(result);
                    }
                }

                return categoryGeneDataProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<SubCategoryByCategoryIdProcModel>> GetSubCategoryByCategoryId(int categoryId)
        {
            try
            {
                List<SubCategoryByCategoryIdProcModel> subCategoryByCategoryIdProcModel = new List<SubCategoryByCategoryIdProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_sub_category_by_category_id({categoryId})", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<SubCategoryByCategoryIdProcModel>(reader);
                        subCategoryByCategoryIdProcModel.Add(result);
                    }
                }

                return subCategoryByCategoryIdProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CategoryGeneMappingListProcModel>> GetCategoryGeneMappingList()
        {
            try
            {
                List<CategoryGeneMappingListProcModel> categoryGeneProcModel = new List<CategoryGeneMappingListProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_category_gene_mapping_list()", conn))

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<CategoryGeneMappingListProcModel>(reader);
                        categoryGeneProcModel.Add(result);
                    }
                }

                return categoryGeneProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteCategoryGeneMapping(DeleteByIdModel deleteGeneSnp)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM delete_category_gene_mapping(:pcategory_gene_mapping_id, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pcategory_gene_mapping_id", deleteGeneSnp.Id);
                cmd.Parameters.AddWithValue(":pmodified_by", deleteGeneSnp.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", deleteGeneSnp.ModifiedDate);
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

        public async Task<CategoryGeneMappingByIdProcModel> GetCategoryGeneMappingById(int categoryGeneMappingId)
        {
            try
            {
                CategoryGeneMappingByIdProcModel categoryGeneMappingByIdProcModel = new CategoryGeneMappingByIdProcModel();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_category_gene_mapping_by_id({categoryGeneMappingId})", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        categoryGeneMappingByIdProcModel = DataReaderExtensionMethod.ConvertToObject<CategoryGeneMappingByIdProcModel>(reader);
                    }
                }

                return categoryGeneMappingByIdProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CreateCategoryGeneMapping(CreateCategoryGeneMappingDto createCategoryGeneMappingDto, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();

            int result = 0;


            string query = $"SELECT * FROM create_category_gene_mapping(:pcategory_id, :psub_category_id, :pgene, :pgene_description, :pgene_image, :pcreated_by, :pcreated_date, :pmodified_by, :pmodified_date)";


            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pcategory_id", createCategoryGeneMappingDto.CategoryId);
                cmd.Parameters.AddWithValue(":psub_category_id", createCategoryGeneMappingDto.SubCategoryId);
                cmd.Parameters.AddWithValue(":pgene", createCategoryGeneMappingDto.Gene);
                cmd.Parameters.AddWithValue(":pgene_description", createCategoryGeneMappingDto.GeneDescription);
                cmd.Parameters.AddWithValue(":pgene_image", createCategoryGeneMappingDto.GeneImage);
                cmd.Parameters.AddWithValue(":pcreated_by", createCategoryGeneMappingDto.CreatedBy);
                cmd.Parameters.AddWithValue(":pcreated_date", createCategoryGeneMappingDto.CreatedDate);
                cmd.Parameters.AddWithValue(":pmodified_by", createCategoryGeneMappingDto.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", createCategoryGeneMappingDto.ModifiedDate);
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

        public async Task<int> UpdateCategoryGeneMappingById(UpdateCategoryGeneMappingByIdDto updateCategoryGeneMappingByIdDto, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM update_category_gene_mapping_by_id(:pcategory_gene_mapping_id, :pcategory_id, :psub_category_id, :pgene, :pgene_description, :pgene_image, :pstatus, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pcategory_gene_mapping_id", updateCategoryGeneMappingByIdDto.CategoryGeneMappingId);
                cmd.Parameters.AddWithValue(":pcategory_id", updateCategoryGeneMappingByIdDto.CategoryId);
                cmd.Parameters.AddWithValue(":psub_category_id", updateCategoryGeneMappingByIdDto.SubCategoryId);
                cmd.Parameters.AddWithValue(":pgene", updateCategoryGeneMappingByIdDto.Gene);
                cmd.Parameters.AddWithValue(":pgene_description", updateCategoryGeneMappingByIdDto.GeneDescription);
                cmd.Parameters.AddWithValue(":pgene_image", updateCategoryGeneMappingByIdDto.GeneImage);
                cmd.Parameters.AddWithValue(":pstatus", updateCategoryGeneMappingByIdDto.Status);
                cmd.Parameters.AddWithValue(":pmodified_by", updateCategoryGeneMappingByIdDto.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", updateCategoryGeneMappingByIdDto.ModifiedDate);

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

        public async Task<List<CategoryGeneSnpOpeningDataProcModel>> GetCategoryGeneSnpOpeningData()
        {
            try
            {
                List<CategoryGeneSnpOpeningDataProcModel> categoryGeneSnpOpeningDataProcModel = new List<CategoryGeneSnpOpeningDataProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_category_gene_snp_opening_data()", conn))


                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<CategoryGeneSnpOpeningDataProcModel>(reader);
                        categoryGeneSnpOpeningDataProcModel.Add(result);
                    }
                }

                return categoryGeneSnpOpeningDataProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CategoryGeneSnpMappingListProcModel>> GetCategoryGeneSnpMappingList()
        {
            try
            {
                List<CategoryGeneSnpMappingListProcModel> categoryGeneSnpProcModel = new List<CategoryGeneSnpMappingListProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_category_gene_snp_mapping_list()", conn))

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<CategoryGeneSnpMappingListProcModel>(reader);
                        categoryGeneSnpProcModel.Add(result);
                    }
                }

                return categoryGeneSnpProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CategoryGeneSnpMappingByIdProcModel> GetCategoryGeneSnpMappingById(int categoryGeneSnpMappingId)
        {
            try
            {
                CategoryGeneSnpMappingByIdProcModel categoryGeneSnpMappingByIdProcModel = new CategoryGeneSnpMappingByIdProcModel();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_category_gene_snp_mapping_by_id({categoryGeneSnpMappingId})", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        categoryGeneSnpMappingByIdProcModel = DataReaderExtensionMethod.ConvertToObject<CategoryGeneSnpMappingByIdProcModel>(reader);
                    }
                }

                return categoryGeneSnpMappingByIdProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CreateCategoryGeneSnpMapping(CreateCategoryGeneSnpMappingDto createCategoryGeneSnpMappingDto, ServiceResponse Obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();

            int result = 0;

            string query = $"SELECT * FROM create_category_gene_snp_mapping(:pcategory_gene_snp_mapping_id, :pcategory_gene_mapping_id, :psnp, :pcreated_by, :pcreated_date, :pmodified_by, :pmodified_date)";


            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pcategory_gene_snp_mapping_id", createCategoryGeneSnpMappingDto.CategoryGeneSnpMappingId);
                cmd.Parameters.AddWithValue(":pcategory_gene_mapping_id", createCategoryGeneSnpMappingDto.CategoryGeneMappingId);
                cmd.Parameters.AddWithValue(":psnp", createCategoryGeneSnpMappingDto.Snp);
                cmd.Parameters.AddWithValue(":pcreated_by", createCategoryGeneSnpMappingDto.CreatedBy);
                cmd.Parameters.AddWithValue(":pcreated_date", createCategoryGeneSnpMappingDto.CreatedDate);
                cmd.Parameters.AddWithValue(":pmodified_by", createCategoryGeneSnpMappingDto.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", createCategoryGeneSnpMappingDto.ModifiedDate);
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

        public async Task<int> UpdateCategoryGeneSnpMapping(UpdateCategoryGeneSnpMappingDto updateCategoryGeneSnpMappingDto, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM update_category_gene_snp_mapping_by_id(:pcategory_gene_snp_mapping_id, :pcategory_gene_mapping_id, :psnp, :pstatus, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pcategory_gene_snp_mapping_id", updateCategoryGeneSnpMappingDto.CategoryGeneSnpMappingId);
                cmd.Parameters.AddWithValue(":pcategory_gene_mapping_id", updateCategoryGeneSnpMappingDto.CategoryGeneMappingId);
                cmd.Parameters.AddWithValue(":psnp", updateCategoryGeneSnpMappingDto.Snp);
                cmd.Parameters.AddWithValue(":pstatus", updateCategoryGeneSnpMappingDto.Status);
                cmd.Parameters.AddWithValue(":pmodified_by", updateCategoryGeneSnpMappingDto.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", updateCategoryGeneSnpMappingDto.ModifiedDate);

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

        public async Task<int> DeleteCategoryGeneSnpMapping(DeleteByIdModel deleteCategoryGeneSnp)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM delete_category_gene_snp_mapping(:pcategory_gene_snp_mapping_id, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pcategory_gene_snp_mapping_id", deleteCategoryGeneSnp.Id);
                cmd.Parameters.AddWithValue(":pmodified_by", deleteCategoryGeneSnp.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", deleteCategoryGeneSnp.ModifiedDate);
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

        public async Task<List<CategoryGeneSnpResultOpeningDataProcModel>> GetCategoryGeneSnpResultOpeningData()
        {
            try
            {
                List<CategoryGeneSnpResultOpeningDataProcModel> categoryGeneSnpResultOpeningDataProcModel = new List<CategoryGeneSnpResultOpeningDataProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_category_gene_snp_result_opening_data()", conn))


                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<CategoryGeneSnpResultOpeningDataProcModel>(reader);
                        categoryGeneSnpResultOpeningDataProcModel.Add(result);
                    }
                }

                return categoryGeneSnpResultOpeningDataProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CategoryGeneSnpResultMappingListProcModel>> GetCategoryGeneSnpResultMappingList()
        {
            try
            {
                List<CategoryGeneSnpResultMappingListProcModel> categoryGeneSnpResultProcModel = new List<CategoryGeneSnpResultMappingListProcModel>();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_category_gene_snp_result_mapping_list()", conn))

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var result = DataReaderExtensionMethod.ConvertToObject<CategoryGeneSnpResultMappingListProcModel>(reader);
                        categoryGeneSnpResultProcModel.Add(result);
                    }
                }

                return categoryGeneSnpResultProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<CategoryGeneSnpResultMappingByIdProcModel> GetCategoryGeneSnpResultMappingById(int categoryGeneSnpResultMappingId)
        {
            try
            {
                CategoryGeneSnpResultMappingByIdProcModel categoryGeneSnpResultMappingByIdProcModel = new CategoryGeneSnpResultMappingByIdProcModel();
                await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                await conn.OpenAsync();
                await using (var cmd = new NpgsqlCommand($"SELECT * from public.get_category_gene_snp_result_mapping_list_by_id({categoryGeneSnpResultMappingId})", conn))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        categoryGeneSnpResultMappingByIdProcModel = DataReaderExtensionMethod.ConvertToObject<CategoryGeneSnpResultMappingByIdProcModel>(reader);
                    }
                }

                return categoryGeneSnpResultMappingByIdProcModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> DeleteCategoryGeneSnpResultMapping(DeleteByIdModel deleteCategoryGeneSnpResult)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM delete_category_gene_snp_result_mapping(:pcategory_gene_snps_result_mapping_id, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pcategory_gene_snps_result_mapping_id", deleteCategoryGeneSnpResult.Id);
                cmd.Parameters.AddWithValue(":pmodified_by", deleteCategoryGeneSnpResult.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", deleteCategoryGeneSnpResult.ModifiedDate);
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

        public async Task<int> CreateCategoryGeneSnpResultMapping(CreateCategoryGeneSnpResult createCategoryGeneSnpResult, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();

            int result = 0;

            conn.TypeMapper.MapComposite<CategoryGeneSnpResultRelation>("type_category_gene_snp_results");

            string query = $"SELECT * FROM create_category_gene_snp_result_mapping(:parray_category_gene_snp_results, :pcreated_by, :pcreated_date, :pmodified_by, :pmodified_date)";


            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":parray_category_gene_snp_results", createCategoryGeneSnpResult.CategoryGeneSnpResultRelation);
                cmd.Parameters.AddWithValue(":pcreated_by", createCategoryGeneSnpResult.CreatedBy);
                cmd.Parameters.AddWithValue(":pcreated_date", createCategoryGeneSnpResult.CreatedDate);
                cmd.Parameters.AddWithValue(":pmodified_by", createCategoryGeneSnpResult.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", createCategoryGeneSnpResult.ModifiedDate);
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

        public async Task<int> UpdateCategoryGeneSnpResultMapping(UpdateCategoryGeneSnpResultDto updateCategoryGeneSnpResultDto, ServiceResponse obj)
        {
            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM update_category_gene_snp_result_mapping(:pcategory_gene_snps_result_mapping_id, :pcategory_gene_snps_mapping_id, :pgenotype, :pgenotype_result, :pstudy_name, :pstudy_description, :pstudy_link, :pstatus, :pmodified_by, :pmodified_date)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":pcategory_gene_snps_result_mapping_id", updateCategoryGeneSnpResultDto.CategoryGeneSnpResultMappingId);
                cmd.Parameters.AddWithValue(":pcategory_gene_snps_mapping_id", updateCategoryGeneSnpResultDto.CategoryGeneSnpMappingId);
                cmd.Parameters.AddWithValue(":pgenotype", updateCategoryGeneSnpResultDto.GenoType);
                cmd.Parameters.AddWithValue(":pgenotype_result", updateCategoryGeneSnpResultDto.GenoTypeResult);
                cmd.Parameters.AddWithValue(":pstudy_name", updateCategoryGeneSnpResultDto.StudyName);
                cmd.Parameters.AddWithValue(":pstudy_description", updateCategoryGeneSnpResultDto.StudyDescription);
                cmd.Parameters.AddWithValue(":pstudy_link", updateCategoryGeneSnpResultDto.StudyLink);
                cmd.Parameters.AddWithValue(":pstatus", updateCategoryGeneSnpResultDto.Status);
                cmd.Parameters.AddWithValue(":pmodified_by", updateCategoryGeneSnpResultDto.ModifiedBy);
                cmd.Parameters.AddWithValue(":pmodified_date", updateCategoryGeneSnpResultDto.ModifiedDate);

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

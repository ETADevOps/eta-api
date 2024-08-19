using AutoMapper;
using ETA.API.Models.StoreProcContextModel;
using ETA_API.Helpers;
using ETA_API.Models.Referance;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;
using ETA_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.RegularExpressions;

namespace ETA_API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IStorageService _storageService;
        private readonly string _azureContainer;
        private readonly string _azureContainerFolder;
        public CategoryController(ICategoryRepository categoryRepository, IConfiguration configuration, IMapper mapper, IStorageService storageService)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));

            _azureContainer = _configuration["AzureBlob:container"];
            _azureContainerFolder = _configuration["AzureBlob:containerFolder"];
        }

        [Produces("application/json")]
        [HttpGet("GetCategoryList", Name = "GetCategoryList")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetCategoryList()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                List<CategoryProcModel> categoryProcModel = new List<CategoryProcModel>();
                categoryProcModel = await _categoryRepository.GetCategoryList();

                var categoryDetailsEntity = _mapper.Map<List<CategoryMapDto>>(categoryProcModel);

                serviceResponse.Data = categoryDetailsEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }
            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetCategoryListFile", Name = "GetCategoryListFile")]
        [Authorize]
        public async Task<ActionResult> GetCategoryListFile()
        {
            string fileName = "ReportTypeList";

            try
            {
                List<CategoryProcModel> categoryProcModel = new List<CategoryProcModel>();
                categoryProcModel = await _categoryRepository.GetCategoryList();

                var categoryDetailsEntity = _mapper.Map<List<CategoryMapDto>>(categoryProcModel);

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("ReportName, ReportGenePredisposition, ReportImportantTakeways, Status");

                Func<string, string> CleanString = (input) =>
                {

                    input = Regex.Replace(input, @"[^\u0000-\u007F]", ""); // special characters
                    input = input.Replace("\"", "\"\"");
                    input = input.Replace("\r\n", " ").Replace("\n", " ");
                    input = input.Trim(); // Trim leading and trailing whitespace
                    return input;
                };

                foreach (var item in categoryDetailsEntity)
                {
                    string statusString = item.Status == 1 ? "Active" : "Inactive";
                    stringBuilder.AppendLine($"{"\"" + CleanString(item.ReportName) + "\""},{"\"" + CleanString(item.ReportGenePredisposition) + "\""},{"\"" + CleanString(item.ReportImportantTakeways) + "\""},{"\"" + CleanString(statusString) + "\""}");
                }
                return File(Encoding.UTF8.GetBytes(stringBuilder.ToString()), "text/csv", $"{fileName}.csv");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Produces("application/json")]
        [HttpPost("CreateCategory", Name = "CreateCategory")]
        [Authorize]

        public async Task<ActionResult<ServiceResponse>> CreateCategory(CreateCategoryDto createCategoryDto, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _categoryRepository.CreateCategory(createCategoryDto, obj);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
                return Ok(obj);
            }
            if (result != 0)
            {
                obj.Data = result;
                return Ok(obj);
            }
            else
            {
                obj.StatusCode = 500;
                obj.StatusMessage = "Failed";
                return Ok(obj);
            }
        }

        [Produces("application/json")]
        [HttpDelete("DeleteCategory", Name = "DeleteCategory")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> DeleteCategory(List<DeleteByIdModel> delete, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();

            try
            {
                foreach (var item in delete)
                {
                    int result = await _categoryRepository.DeleteCategory(item);
                }

                obj.Data = "Success";
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
            }

            return Ok(obj);
        }

        [Produces("application/json")]
        [HttpGet("GetCategoryById/{categoryId}", Name = "GetCategoryById")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetCategoryById(int categoryId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                CategoryProcModel categoryByIdProcModel = new CategoryProcModel();
                categoryByIdProcModel = await _categoryRepository.GetCategoryById(categoryId);

                var categoryDetailsEntity = _mapper.Map<CategoryMapDto>(categoryByIdProcModel);


                serviceResponse.Data = categoryDetailsEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpPut("UpdateCategory", Name = "UpdateCategory")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _categoryRepository.UpdateCategory(updateCategoryDto, obj);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
                return Ok(obj);
            }
            if (result != 0)
            {
                obj.Data = result;
                return Ok(obj);
            }
            else
            {
                obj.StatusCode = 500;
                obj.StatusMessage = "Failed";
                return Ok(obj);
            }
        }

        [Produces("application/json")]
        [HttpGet("GetCategoryOpeningData", Name = "GetCategoryOpeningData")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetCategoryOpeningData()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                List<CategoryOpeningDataProcModel> categoryDataProcModel = new List<CategoryOpeningDataProcModel>();
                categoryDataProcModel = await _categoryRepository.GetCategoryOpeningData();

                var categoryOpeningDataEntity = _mapper.Map<List<CategoryOpeningDataDto>>(categoryDataProcModel);

                serviceResponse.Data = categoryOpeningDataEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }
            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpPost("CreateReportCategory", Name = "CreateReportCategory")]
        [Authorize]

        public async Task<ActionResult<ServiceResponse>> CreateReportCategory(CreateReportCategoryDto createReportCategoryDto, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _categoryRepository.CreateReportCategory(createReportCategoryDto, obj);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
                return Ok(obj);
            }
            if (result != 0)
            {
                obj.Data = result;
                return Ok(obj);
            }
            else
            {
                obj.StatusCode = 500;
                obj.StatusMessage = "Failed";
                return Ok(obj);
            }
        }

        [Produces("application/json")]
        [HttpGet("GetReportCategoryList", Name = "GetReportCategoryList")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetReportCategoryList()
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            string fileName = "ReportCategoryList";
            try
            {
                List<ReportCategoryListProcModel> reportCategoryDataProcModel = new List<ReportCategoryListProcModel>();
                reportCategoryDataProcModel = await _categoryRepository.GetReportCategoryList();

                var reportCategoryDataEntity = _mapper.Map<List<ReportCategoryListDto>>(reportCategoryDataProcModel);

                serviceResponse.Data = reportCategoryDataEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }
            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetReportCategoryListFile", Name = "GetReportCategoryListFile")]
        [Authorize]
        public async Task<ActionResult> GetReportCategoryListFile()
        {

            string fileName = "ReportCategoryList";
            try
            {
                List<ReportCategoryListProcModel> reportCategoryDataProcModel = new List<ReportCategoryListProcModel>();
                reportCategoryDataProcModel = await _categoryRepository.GetReportCategoryList();

                var reportCategoryDataEntity = _mapper.Map<List<ReportCategoryListDto>>(reportCategoryDataProcModel);

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("ReportName, CategoryName, CategoryDescription, Status");

                Func<string, string> CleanString = (input) =>
                {

                    input = Regex.Replace(input, @"[^\u0000-\u007F]", ""); // special characters
                    input = input.Replace("\"", "\"\"");
                    input = input.Replace("\r\n", " ").Replace("\n", " ");
                    input = input.Trim(); // Trim leading and trailing whitespace
                    return input;
                };

                foreach (var item in reportCategoryDataEntity)
                {
                    string statusString = item.Status == 1 ? "Active" : "Inactive";
                    stringBuilder.AppendLine($"{"\"" + item.ReportName + "\""},{"\"" + item.CategoryName + "\""},{"\"" + CleanString(item.CategoryIntroduction) + "\""},{"\"" + CleanString(statusString) + "\""}");
                }
                return File(Encoding.UTF8.GetBytes(stringBuilder.ToString()), "text/csv", $"{fileName}.csv");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [Produces("application/json")]
        [HttpGet("GetReportCategoryById/{categoryId}", Name = "GetReportCategoryById")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetReportCategoryById(int categoryId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                ReportCategoryByIdProcModel reportCategoryByIdProcModel = new ReportCategoryByIdProcModel();
                reportCategoryByIdProcModel = await _categoryRepository.GetReportCategoryById(categoryId);

                var reportCategoryDetailsEntity = _mapper.Map<ReportCategoryByIdDto>(reportCategoryByIdProcModel);


                serviceResponse.Data = reportCategoryDetailsEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpDelete("DeleteReportCategory", Name = "DeleteReportCategory")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> DeleteReportCategory(List<DeleteByIdModel> delete, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();

            try
            {
                foreach (var item in delete)
                {
                    int result = await _categoryRepository.DeleteReportCategory(item);
                }

                obj.Data = "Success";
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
            }

            return Ok(obj);
        }

        [Produces("application/json")]
        [HttpPut("UpdateReportCategory", Name = "UpdateReportCategory")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> UpdateReportCategory(UpdateReportCategoryDto updateReportCategoryDto, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _categoryRepository.UpdateReportCategory(updateReportCategoryDto, obj);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
                return Ok(obj);
            }
            if (result != 0)
            {
                obj.Data = result;
                return Ok(obj);
            }
            else
            {
                obj.StatusCode = 500;
                obj.StatusMessage = "Failed";
                return Ok(obj);
            }
        }

        [Produces("application/json")]
        [HttpGet("GetSubCategoryOpeningData", Name = "GetSubCategoryOpeningData")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetSubCategoryOpeningData()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                List<SubCategoryOpeningDataProcModel> subCategoryDataProcModel = new List<SubCategoryOpeningDataProcModel>();
                subCategoryDataProcModel = await _categoryRepository.GetSubCategoryOpeningData();

                var subCategoryOpeningDataEntity = _mapper.Map<List<SubCategoryOpeningDataDto>>(subCategoryDataProcModel);

                serviceResponse.Data = subCategoryOpeningDataEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }
            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpPost("CreateSubCategory", Name = "CreateSubCategory")]
        [Authorize]

        public async Task<ActionResult<ServiceResponse>> CreateSubCategory(CreateSubCategoryDto createSubCategoryDto, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _categoryRepository.CreateSubCategory(createSubCategoryDto, obj);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
                return Ok(obj);
            }
            if (result != 0)
            {
                obj.Data = result;
                return Ok(obj);
            }
            else
            {
                obj.StatusCode = 500;
                obj.StatusMessage = "Failed";
                return Ok(obj);
            }
        }

        [Produces("application/json")]
        [HttpPut("UpdateSubCategory", Name = "UpdateSubCategory")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> UpdateSubCategory(UpdateSubCategoryDto updateSubCategoryDto, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _categoryRepository.UpdateSubCategory(updateSubCategoryDto, obj);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
                return Ok(obj);
            }
            if (result != 0)
            {
                obj.Data = result;
                return Ok(obj);
            }
            else
            {
                obj.StatusCode = 500;
                obj.StatusMessage = "Failed";
                return Ok(obj);
            }
        }

        [Produces("application/json")]
        [HttpGet("GetSubCategoryById/{subCategoryId}", Name = "GetSubCategoryById")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetSubCategoryById(int subCategoryId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                SubCategoryByIdProcModel subCategoryByIdProcModel = new SubCategoryByIdProcModel();
                subCategoryByIdProcModel = await _categoryRepository.GetSubCategoryById(subCategoryId);

                var subCategoryDetailsEntity = _mapper.Map<SubCategoryByIdDto>(subCategoryByIdProcModel);


                serviceResponse.Data = subCategoryDetailsEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetSubCategoryList", Name = "GetSubCategoryList")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetSubCategoryList()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                List<SubCategoryListProcModel> subCategoryDataProcModel = new List<SubCategoryListProcModel>();
                subCategoryDataProcModel = await _categoryRepository.GetSubCategoryList();

                var subCategoryDataEntity = _mapper.Map<List<SubCategoryListDto>>(subCategoryDataProcModel);

                serviceResponse.Data = subCategoryDataEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }
            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetSubCategoryListFile", Name = "GetSubCategoryListFile")]
        [Authorize]
        public async Task<ActionResult> GetSubCategoryListFile()
        {

            string fileName = "SubCategoryList";

            try
            {
                List<SubCategoryListProcModel> subCategoryDataProcModel = new List<SubCategoryListProcModel>();
                subCategoryDataProcModel = await _categoryRepository.GetSubCategoryList();

                var subCategoryDataEntity = _mapper.Map<List<SubCategoryListDto>>(subCategoryDataProcModel);

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("ReportName, CategoryName, SubCategoryName, SubCategoryDescription, Status");

                Func<string, string> CleanString = (input) =>
                {
                    input = Regex.Replace(input, @"[^\u0000-\u007F]", ""); // special characters
                    input = input.Replace("\"", "\"\"");
                    input = input.Replace("\r\n", " ").Replace("\n", " ");
                    input = input.Trim(); // Trim leading and trailing whitespace
                    return input;
                };

                foreach (var item in subCategoryDataEntity)
                {
                    string statusString = item.Status == 1 ? "Active" : "Inactive";
                    stringBuilder.AppendLine($"{"\"" + item.ReportName + "\""},{"\"" + item.CategoryName + "\""},{"\"" + item.SubCategoryName + "\""},{"\"" + CleanString(item.SubCategoryIntroduction) + "\""},{"\"" + CleanString(statusString) + "\""}");
                }
                return File(Encoding.UTF8.GetBytes(stringBuilder.ToString()), "text/csv", $"{fileName}.csv");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Produces("application/json")]
        [HttpDelete("DeleteSubCategory", Name = "DeleteSubCategory")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> DeleteSubCategory(List<DeleteByIdModel> delete, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();

            try
            {
                foreach (var item in delete)
                {
                    int result = await _categoryRepository.DeleteSubCategory(item);
                }

                obj.Data = "Success";
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
            }

            return Ok(obj);
        }

        [Produces("application/json")]
        [HttpGet("GetCategoryGeneOpeningData", Name = "GetCategoryGeneOpeningData")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetCategoryGeneOpeningData()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                List<CategoryGeneOpeningDataProcModel> geneSnpDataProcModel = new List<CategoryGeneOpeningDataProcModel>();
                geneSnpDataProcModel = await _categoryRepository.GetCategoryGeneOpeningData();

                var geneSnpOpeningDataEntity = _mapper.Map<List<CategoryGeneOpeningDataDto>>(geneSnpDataProcModel);

                serviceResponse.Data = geneSnpOpeningDataEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }
            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetSubCategoryByCategoryId/{categoryId}", Name = "GetSubCategoryByCategoryId")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetSubCategoryByCategoryId(int categoryId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                List<SubCategoryByCategoryIdProcModel> subCategoryByCategoryIdProcModel = new List<SubCategoryByCategoryIdProcModel>();
                subCategoryByCategoryIdProcModel = await _categoryRepository.GetSubCategoryByCategoryId(categoryId);

                var subCategoryByCategoryIdDetailsEntity = _mapper.Map<List<SubCategoryByCategoryIdDto>>(subCategoryByCategoryIdProcModel);


                serviceResponse.Data = subCategoryByCategoryIdDetailsEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetCategoryGeneMappingList", Name = "GetCategoryGeneMappingList")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetCategoryGeneMappingList()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                List<CategoryGeneMappingListProcModel> geneSnpDataProcModel = new List<CategoryGeneMappingListProcModel>();
                geneSnpDataProcModel = await _categoryRepository.GetCategoryGeneMappingList();

                var geneSnpDataEntity = _mapper.Map<List<CategoryGeneMappingListDto>>(geneSnpDataProcModel);

                serviceResponse.Data = geneSnpDataEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }
            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetCategoryGeneMappingListFile", Name = "GetCategoryGeneMappingListFile")]
        [Authorize]
        public async Task<ActionResult> GetCategoryGeneMappingListFile()
        {

            string fileName = "GeneList";

            try
            {
                List<CategoryGeneMappingListProcModel> geneSnpDataProcModel = new List<CategoryGeneMappingListProcModel>();
                geneSnpDataProcModel = await _categoryRepository.GetCategoryGeneMappingList();

                var geneSnpDataEntity = _mapper.Map<List<CategoryGeneMappingListDto>>(geneSnpDataProcModel);

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("ReportName, CategoryName, SubCategoryName, Gene, GeneDescription, Status");

                Func<string, string> CleanString = (input) =>
                {

                    input = Regex.Replace(input, @"[^\u0000-\u007F]", ""); // special characters
                    input = input.Replace("\"", "\"\"");
                    input = input.Replace("\r\n", " ").Replace("\n", " ");
                    input = input.Trim(); // Trim leading and trailing whitespace
                    return input;
                };

                foreach (var item in geneSnpDataEntity)
                {
                    string statusString = item.Status == 1 ? "Active" : "Inactive";
                    stringBuilder.AppendLine($"{"\"" + item.ReportName + "\""},{"\"" + item.CategoryName + "\""},{"\"" + item.SubCategoryName + "\""},{"\"" + item.Gene + "\""},{"\"" + CleanString(item.GeneDescription) + "\""},{"\"" + CleanString(statusString) + "\""}");
                }
                return File(Encoding.UTF8.GetBytes(stringBuilder.ToString()), "text/csv", $"{fileName}.csv");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Produces("application/json")]
        [HttpDelete("DeleteCategoryGeneMapping", Name = "DeleteCategoryGeneMapping")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> DeleteCategoryGeneMapping(List<DeleteByIdModel> delete, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();

            try
            {
                foreach (var item in delete)
                {
                    int result = await _categoryRepository.DeleteCategoryGeneMapping(item);
                }

                obj.Data = "Success";
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
            }

            return Ok(obj);
        }

        [Produces("application/json")]
        [HttpGet("GetCategoryGeneMappingById/{categoryGeneMappingId}", Name = "GetCategoryGeneMappingById")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetCategoryGeneMappingById(int categoryGeneMappingId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                CategoryGeneMappingByIdProcModel geneSnpMappingByIdProcModel = new CategoryGeneMappingByIdProcModel();
                geneSnpMappingByIdProcModel = await _categoryRepository.GetCategoryGeneMappingById(categoryGeneMappingId);

                var geneSnpMappingDetailsEntity = _mapper.Map<CatgeoryGeneMappingByIdDto>(geneSnpMappingByIdProcModel);


                serviceResponse.Data = geneSnpMappingDetailsEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpPost("CreateCategoryGeneMapping", Name = "CreateCategoryGeneMapping")]
        [Authorize]

        public async Task<ActionResult<ServiceResponse>> CreateCategoryGeneMapping(CreateCategoryGeneMappingDto createCategoryGeneMappingDto, ApiVersion version)
        {
            if (createCategoryGeneMappingDto.GeneImage != null)
            {
                string downloadUrl, path;
                _storageService.UploadAttachment(createCategoryGeneMappingDto.GeneImage, "image/png", out downloadUrl, out path, "", ".png");
                createCategoryGeneMappingDto.GeneImage = downloadUrl;
            }

            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _categoryRepository.CreateCategoryGeneMapping(createCategoryGeneMappingDto, obj);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
                return Ok(obj);
            }
            if (result != 0)
            {
                obj.Data = result;
                return Ok(obj);
            }
            else
            {
                obj.StatusCode = 500;
                obj.StatusMessage = "Failed";
                return Ok(obj);
            }
        }

        [Produces("application/json")]
        [HttpPut("UpdateCategoryGeneMappingById", Name = "UpdateCategoryGeneMappingById")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> UpdateCategoryGeneMappingById(UpdateCategoryGeneMappingByIdDto updateCategoryGeneMappingByIdDto, ApiVersion version)
        {
            if (!string.IsNullOrEmpty(updateCategoryGeneMappingByIdDto.GeneImage) && updateCategoryGeneMappingByIdDto.GeneImage.Contains("base64"))
            {
                string downloadUrl, path;
                _storageService.UploadAttachment(updateCategoryGeneMappingByIdDto.GeneImage, "image/png", out downloadUrl, out path, "", ".png");
                updateCategoryGeneMappingByIdDto.GeneImage = downloadUrl;
            }

            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _categoryRepository.UpdateCategoryGeneMappingById(updateCategoryGeneMappingByIdDto, obj);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
                return Ok(obj);
            }
            if (result != 0)
            {
                obj.Data = result;
                return Ok(obj);
            }
            else
            {
                obj.StatusCode = 500;
                obj.StatusMessage = "Failed";
                return Ok(obj);
            }
        }

        [Produces("application/json")]
        [HttpGet("GetCategoryGeneSnpOpeningData", Name = "GetCategoryGeneSnpOpeningData")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetCategoryGeneSnpOpeningData()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                List<CategoryGeneSnpOpeningDataProcModel> categoryGeneSnpDataProcModel = new List<CategoryGeneSnpOpeningDataProcModel>();
                categoryGeneSnpDataProcModel = await _categoryRepository.GetCategoryGeneSnpOpeningData();

                var categoryGeneSnpOpeningDataEntity = _mapper.Map<List<CategoryGeneSnpOpeningDataDto>>(categoryGeneSnpDataProcModel);

                serviceResponse.Data = categoryGeneSnpOpeningDataEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }
            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetCategoryGeneSnpMappingList", Name = "GetCategoryGeneSnpMappingList")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetCategoryGeneSnpMappingList()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                List<CategoryGeneSnpMappingListProcModel> categoryGeneSnpDataProcModel = new List<CategoryGeneSnpMappingListProcModel>();
                categoryGeneSnpDataProcModel = await _categoryRepository.GetCategoryGeneSnpMappingList();

                var categoryGeneSnpDataEntity = _mapper.Map<List<CategoryGeneSnpMappingListDto>>(categoryGeneSnpDataProcModel);

                serviceResponse.Data = categoryGeneSnpDataEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }
            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetCategoryGeneSnpMappingListFile", Name = "GetCategoryGeneSnpMappingListFile")]
        [Authorize]
        public async Task<ActionResult> GetCategoryGeneSnpMappingListFile()
        {
            string fileName = "SnpList";

            try
            {
                List<CategoryGeneSnpMappingListProcModel> categoryGeneSnpDataProcModel = new List<CategoryGeneSnpMappingListProcModel>();
                categoryGeneSnpDataProcModel = await _categoryRepository.GetCategoryGeneSnpMappingList();

                var categoryGeneSnpDataEntity = _mapper.Map<List<CategoryGeneSnpMappingListDto>>(categoryGeneSnpDataProcModel);

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("ReportName, CategoryName, SubCategoryName, Gene, Snp, Status");

                Func<string, string> CleanString = (input) =>
                {

                    input = Regex.Replace(input, @"[^\u0000-\u007F]", ""); // special characters
                    input = input.Replace("\"", "\"\"");
                    input = input.Replace("\r\n", " ").Replace("\n", " ");
                    input = input.Trim(); // Trim leading and trailing whitespace
                    return input;
                };

                foreach (var item in categoryGeneSnpDataEntity)
                {
                    string statusString = item.Status == 1 ? "Active" : "Inactive";
                    stringBuilder.AppendLine($"{"\"" + item.ReportName + "\""},{"\"" + item.CategoryName + "\""},{"\"" + item.SubCategoryName + "\""},{"\"" + item.Gene + "\""},{"\"" + item.Snp + "\""},{"\"" + CleanString(statusString) + "\""}");
                }
                return File(Encoding.UTF8.GetBytes(stringBuilder.ToString()), "text/csv", $"{fileName}.csv");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Produces("application/json")]
        [HttpGet("GetCategoryGeneSnpMappingById/{categoryGeneSnpMappingId}", Name = "GetCategoryGeneSnpMappingById")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetCategoryGeneSnpMappingById(int categoryGeneSnpMappingId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                CategoryGeneSnpMappingByIdProcModel categoryGeneSnpMappingByIdProcModel = new CategoryGeneSnpMappingByIdProcModel();
                categoryGeneSnpMappingByIdProcModel = await _categoryRepository.GetCategoryGeneSnpMappingById(categoryGeneSnpMappingId);

                var categoryGeneSnpMappingDetailsEntity = _mapper.Map<CategoryGeneSnpMappingByIdDto>(categoryGeneSnpMappingByIdProcModel);


                serviceResponse.Data = categoryGeneSnpMappingDetailsEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpPost("CreateCategoryGeneSnpMapping", Name = "CreateCategoryGeneSnpMapping")]
        [Authorize]

        public async Task<ActionResult<ServiceResponse>> CreateCategoryGeneSnpMapping(CreateCategoryGeneSnpMappingDto createCategoryGeneSnpMappingDto, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _categoryRepository.CreateCategoryGeneSnpMapping(createCategoryGeneSnpMappingDto, obj);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
                return Ok(obj);
            }
            if (result != 0)
            {
                obj.Data = result;
                return Ok(obj);
            }
            else
            {
                obj.StatusCode = 500;
                obj.StatusMessage = "Failed";
                return Ok(obj);
            }
        }

        [Produces("application/json")]
        [HttpPut("UpdateCategoryGeneSnpMapping", Name = "UpdateCategoryGeneSnpMapping")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> UpdateCategoryGeneSnpMapping(UpdateCategoryGeneSnpMappingDto updateCategoryGeneSnpMappingDto, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _categoryRepository.UpdateCategoryGeneSnpMapping(updateCategoryGeneSnpMappingDto, obj);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
                return Ok(obj);
            }
            if (result != 0)
            {
                obj.Data = result;
                return Ok(obj);
            }
            else
            {
                obj.StatusCode = 500;
                obj.StatusMessage = "Failed";
                return Ok(obj);
            }
        }

        [Produces("application/json")]
        [HttpDelete("DeleteCategoryGeneSnpMapping", Name = "DeleteCategoryGeneSnpMapping")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> DeleteCategoryGeneSnpMapping(List<DeleteByIdModel> delete, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();

            try
            {
                foreach (var item in delete)
                {
                    int result = await _categoryRepository.DeleteCategoryGeneSnpMapping(item);
                }

                obj.Data = "Success";
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
            }

            return Ok(obj);
        }

        [Produces("application/json")]
        [HttpGet("GetCategoryGeneSnpResultOpeningData", Name = "GetCategoryGeneSnpResultOpeningData")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetCategoryGeneSnpResultOpeningData()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                List<CategoryGeneSnpResultOpeningDataProcModel> categoryGeneSnpResultDataProcModel = new List<CategoryGeneSnpResultOpeningDataProcModel>();
                categoryGeneSnpResultDataProcModel = await _categoryRepository.GetCategoryGeneSnpResultOpeningData();

                var categoryGeneSnpResultOpeningDataEntity = _mapper.Map<List<CategoryGeneSnpResultOpeningDataDto>>(categoryGeneSnpResultDataProcModel);

                serviceResponse.Data = categoryGeneSnpResultOpeningDataEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }
            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetCategoryGeneSnpResultMappingList", Name = "GetCategoryGeneSnpResultMappingList")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetCategoryGeneSnpResultMappingList()
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                List<CategoryGeneSnpResultMappingListProcModel> categoryGeneSnpResultDataProcModel = new List<CategoryGeneSnpResultMappingListProcModel>();
                categoryGeneSnpResultDataProcModel = await _categoryRepository.GetCategoryGeneSnpResultMappingList();

                var categoryGeneSnpResultDataEntity = _mapper.Map<List<CategoryGeneSnpResultMappingListDto>>(categoryGeneSnpResultDataProcModel);

                serviceResponse.Data = categoryGeneSnpResultDataEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }
            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetCategoryGeneSnpResultMappingListFile", Name = "GetCategoryGeneSnpResultMappingListFile")]
        [Authorize]
        public async Task<ActionResult> GetCategoryGeneSnpResultMappingListFile()
        {

            string fileName = "SnpResultList";
            try
            {
                List<CategoryGeneSnpResultMappingListProcModel> categoryGeneSnpResultDataProcModel = new List<CategoryGeneSnpResultMappingListProcModel>();
                categoryGeneSnpResultDataProcModel = await _categoryRepository.GetCategoryGeneSnpResultMappingList();

                var categoryGeneSnpResultDataEntity = _mapper.Map<List<CategoryGeneSnpResultMappingListDto>>(categoryGeneSnpResultDataProcModel);

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("ReportName, CategoryName, SubCategoryName, Gene, Snp, GenoType, GenoTypeResult, StudyName, StudyDescription, Studylink, Status");

                Func<string, string> CleanString = (input) =>
                {
                    input = Regex.Replace(input, @"[^\u0000-\u007F]", ""); // special characters
                    input = input.Replace("\"", "\"\"");
                    input = input.Replace("\r\n", " ").Replace("\n", " ");
                    input = input.Trim(); // Trim leading and trailing whitespace
                    return input;
                };

                foreach (var item in categoryGeneSnpResultDataEntity)
                {
                    string statusString = item.Status == 1 ? "Active" : "Inactive";
                    stringBuilder.AppendLine($"{"\"" + item.ReportName + "\""},{"\"" + item.CategoryName + "\""},{"\"" + item.SubCategoryName + "\""},{"\"" + item.Gene + "\""},{"\"" + item.Snp + "\""},{"\"" + item.GenoType + "\""},{"\"" + CleanString(item.GenoTypeResult) + "\""},{"\"" + CleanString(item.StudyName) + "\""},{"\"" + CleanString(item.StudyDescription) + "\""},{"\"" + CleanString(item.Studylink) + "\""},{"\"" + CleanString(statusString) + "\""}");
                }
                return File(Encoding.UTF8.GetBytes(stringBuilder.ToString()), "text/csv", $"{fileName}.csv");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Produces("application/json")]
        [HttpGet("GetCategoryGeneSnpResultMappingById/{categoryGeneSnpResultMappingId}", Name = "GetCategoryGeneSnpResultMappingById")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetCategoryGeneSnpResultMappingById(int categoryGeneSnpResultMappingId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                CategoryGeneSnpResultMappingByIdProcModel categoryGeneSnpResultMappingByIdProcModel = new CategoryGeneSnpResultMappingByIdProcModel();
                categoryGeneSnpResultMappingByIdProcModel = await _categoryRepository.GetCategoryGeneSnpResultMappingById(categoryGeneSnpResultMappingId);

                var categoryGeneSnpMappingResultDetailsEntity = _mapper.Map<CategoryGeneSnpResultMappingByIdDto>(categoryGeneSnpResultMappingByIdProcModel);


                serviceResponse.Data = categoryGeneSnpMappingResultDetailsEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpDelete("DeleteCategoryGeneSnpResultMapping", Name = "DeleteCategoryGeneSnpResultMapping")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> DeleteCategoryGeneSnpResultMapping(List<DeleteByIdModel> delete, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();

            try
            {
                foreach (var item in delete)
                {
                    int result = await _categoryRepository.DeleteCategoryGeneSnpResultMapping(item);
                }

                obj.Data = "Success";
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
            }

            return Ok(obj);
        }

        [Produces("application/json")]
        [HttpPost("CreateCategoryGeneSnpResultMapping", Name = "CreateCategoryGeneSnpResultMapping")]
        [Authorize]

        public async Task<ActionResult<ServiceResponse>> CreateCategoryGeneSnpResultMapping(CreateCategoryGeneSnpResult createCategoryGeneSnpResult, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _categoryRepository.CreateCategoryGeneSnpResultMapping(createCategoryGeneSnpResult, obj);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
                return Ok(obj);
            }
            if (result != 0)
            {
                obj.Data = result;
                return Ok(obj);
            }
            else
            {
                obj.StatusCode = 500;
                obj.StatusMessage = "Failed";
                return Ok(obj);
            }
        }

        [Produces("application/json")]
        [HttpPut("UpdateCategoryGeneSnpResultMapping", Name = "UpdateCategoryGeneSnpResultMapping")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> UpdateCategoryGeneSnpResultMapping(UpdateCategoryGeneSnpResultDto updateCategoryGeneSnpResultDto, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _categoryRepository.UpdateCategoryGeneSnpResultMapping(updateCategoryGeneSnpResultDto, obj);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
                return Ok(obj);
            }
            if (result != 0)
            {
                obj.Data = result;
                return Ok(obj);
            }
            else
            {
                obj.StatusCode = 500;
                obj.StatusMessage = "Failed";
                return Ok(obj);
            }
        }
    }
}

using AutoMapper;
using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;
using ETA.API.Services;
using ETA_API.Helpers;
using ETA_API.Models.Referance;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;
using ETA_API.Services;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.Xml;
using static iTextSharp.text.pdf.AcroFields;


namespace ETA_API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientrepository _patientrepository;
        private readonly IMapper _mapper;
        private readonly ICommonService _commonService;
        private readonly IReportServices _reportService;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        private readonly string _azureContainer;
        private readonly string _azureContainerFolder;

        public PatientsController(IMapper mapper, IPatientrepository patientrepository, ICommonService commonService,
                IReportServices reportService, IStorageService storageService, IConfiguration configuration, IUserRepository userRepository)
        {
            _patientrepository = patientrepository ?? throw new ArgumentNullException(nameof(patientrepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _commonService = commonService ?? throw new ArgumentNullException(nameof(commonService));
            _reportService = reportService ?? throw new ArgumentNullException(nameof(reportService));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

            _azureContainer = _configuration["AzureBlob:container"];
            _azureContainerFolder = _configuration["AzureBlob:containerFolder"];
        }

        [Produces("application/json")]
        [HttpGet("GetPatientsList/{clientid}/{providerId}/{patientId}", Name = "GetPatientsList")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetPatientsList(int clientid, int providerId, int patientId)
        {
            try
            {
                ServiceResponse serviceResponse = new ServiceResponse();

                List<PatientsProcModel> patientList = new List<PatientsProcModel>();

                patientList = await _patientrepository.GetPatientsList(clientid, providerId, patientId);

                foreach (var item in patientList)
                {
                    if (String.IsNullOrEmpty(item.psample_id))
                        item.psample_id = "";
                }

                //var patientDetailsEntity = _mapper.Map<List<PatientsDto>>(patientList);

                serviceResponse.Data = patientList;

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Produces("application/json")]
        [HttpGet("GetPatientsbyId/{patientId}", Name = "GetPatientsbyId")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetPatientsbyId(int patientId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                PatientByIdData patientByIdData = new PatientByIdData();
                patientByIdData = await _patientrepository.GetPatientsbyId(patientId);

                serviceResponse.Data = patientByIdData;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpPost("CreatePatient", Name = "CreatePatient")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> CreatePatient(CreatePatientDto createpatient, ApiVersion version)
        {
            if (createpatient.PatientAttachments.Count > 0)
            {
                for (int i = 0; i < createpatient.PatientAttachments.Count; i++)
                {
                    if (createpatient.PatientAttachments[i].preport_url != null)
                    {
                        string downloadUrl, path;


                        if (createpatient.PatientAttachments[i].preport_type == "image/jpeg")
                        {
                            _storageService.UploadAttachment(createpatient.PatientAttachments[i].preport_url, "image/jpeg", out downloadUrl, out path, "", ".jpeg");
                            createpatient.PatientAttachments[i].preport_url = downloadUrl;
                        }
                        else if (createpatient.PatientAttachments[i].preport_type == "image/png")
                        {
                            _storageService.UploadAttachment(createpatient.PatientAttachments[i].preport_url, "image/png", out downloadUrl, out path, "", ".png");
                            createpatient.PatientAttachments[i].preport_url = downloadUrl;
                        }
                        else if (createpatient.PatientAttachments[i].preport_type == "text/plain")
                        {
                            _storageService.UploadAttachment(createpatient.PatientAttachments[i].preport_url, "text/plain", out downloadUrl, out path, "", ".txt");
                            createpatient.PatientAttachments[i].preport_url = downloadUrl;
                        }
                        else
                        {
                            _storageService.UploadAttachment(createpatient.PatientAttachments[i].preport_url, "application/pdf", out downloadUrl, out path, "", ".pdf");
                            createpatient.PatientAttachments[i].preport_url = downloadUrl;
                        }
                    }
                }
            }

            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _patientrepository.CreatePatient(createpatient, obj);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message.Contains("unq_patients_fn_ln_dob_gender") ? "Patient Already Exists" : ex.Message;
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
        [HttpPut("UpdatePatient", Name = "UpdatePatient")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> UpdatePatient(CreatePatientDto createpatient, ApiVersion version)
        {
            if (createpatient.PatientAttachments.Count > 0)
            {
                for (int i = 0; i < createpatient.PatientAttachments.Count; i++)
                {
                    if (createpatient.PatientAttachments[i].preport_url != null)
                    {
                        string downloadUrl, path;


                        if (createpatient.PatientAttachments[i].preport_type == "image/jpeg")
                        {
                            _storageService.UploadAttachment(createpatient.PatientAttachments[i].preport_url, "image/jpeg", out downloadUrl, out path, "", ".jpeg");
                            createpatient.PatientAttachments[i].preport_url = downloadUrl;
                        }
                        else if (createpatient.PatientAttachments[i].preport_type == "image/png")
                        {
                            _storageService.UploadAttachment(createpatient.PatientAttachments[i].preport_url, "image/png", out downloadUrl, out path, "", ".png");
                            createpatient.PatientAttachments[i].preport_url = downloadUrl;
                        }
                        else if (createpatient.PatientAttachments[i].preport_type == "text/plain")
                        {
                            _storageService.UploadAttachment(createpatient.PatientAttachments[i].preport_url, "text/plain", out downloadUrl, out path, "", ".txt");
                            createpatient.PatientAttachments[i].preport_url = downloadUrl;
                        }
                        else
                        {
                            _storageService.UploadAttachment(createpatient.PatientAttachments[i].preport_url, "application/pdf", out downloadUrl, out path, "", ".pdf");
                            createpatient.PatientAttachments[i].preport_url = downloadUrl;
                        }
                    }
                }
            }

            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _patientrepository.UpdatePatient(createpatient, obj);
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message.Contains("unq_patients_fn_ln_dob_gender") ? "Patient Already Exists" : ex.Message;
                return Ok(obj);
            }
            if (result != 0)
            {
                obj.Data = "Success";
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
        [HttpDelete("DeletePatient", Name = "DeletePatient")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> DeletePatient(List<DeleteByIdModel> delete, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            try
            {
                foreach (var item in delete)
                {
                    int result = await _patientrepository.DeletePatient(item);
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

        [HttpGet("ZIPSearch", Name = "GetZipSearch")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<List<PatientCityState>>> GetZipSearch(string searchText)
        {
            try
            {
                string searchZIPText = string.Empty;
                List<PatientCityState> serachzipList = new List<PatientCityState>();
                serachzipList = await _patientrepository.GetZIPSearch(searchText);
                return serachzipList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Produces("application/json")]
        [HttpGet("GetPatientsOpeningData/{clientId}", Name = "GetPatientsOpeningData")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetPatientsOpeningData(int clientId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                PatientsOpeningDataModel patientsOpeningData = new PatientsOpeningDataModel();
                patientsOpeningData = await _patientrepository.GetPatientsOpeningData(clientId);

                serviceResponse.Data = patientsOpeningData;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpPost("CreatePatientGeneFile/{patientId}/{createdBy}/{createdDate}", Name = "CreatePatientGeneFile")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> CreatePatientGeneFile(int patientId, int createdBy, DateTime? createdDate, IFormFile file)
        {
            ServiceResponse obj = new ServiceResponse();
            string status = "";
            //Validation 
            if (file == null)
            {
                return NotFound(new ServiceResponse { StatusCode = 404, StatusMessage = "file is null" });
            }

            var allowedExtensions = new[] { ".txt" };
            var extension = Path.GetExtension(file.FileName);
            if (!allowedExtensions.Contains(extension))
            {
                return ValidationProblem($"extension must contains {allowedExtensions.ToArray()}");
            }

            try
            {
                if (patientId != 0)
                {
                    status = await _storageService.UploadDNAAzureJob(patientId, createdBy, createdDate, file);

                    if (status.Contains("blob.core"))
                    {
                        obj.Data = status;
                    }
                    else
                    {
                        obj.StatusCode = 500;
                        obj.StatusMessage = status;
                    }
                }
            }
            catch (Exception ex)
            {
                obj.StatusCode = 500;
                obj.StatusMessage = ex.Message;
            }

            if (obj.StatusCode == 500)
            {
                AuditOpeningData auditLogOpeningData = new AuditOpeningData();
                auditLogOpeningData = await _userRepository.GetAuditOpeningData();

                DateTime localTime = DateTime.Now;

                TimeZoneInfo pacificTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

                DateTime pacificTime = TimeZoneInfo.ConvertTime(localTime, TimeZoneInfo.Local, pacificTimeZone);

                CreateAuditLog createAuditLog = new CreateAuditLog
                {
                    AuditUserId = createdBy,
                    UserId = 0,
                    AuditCategoryMasterId = auditLogOpeningData.AuditMaster.Where(x => x.audit_category_name == "Report Management").Select(x => x.audit_category_master_id).First(),
                    AuditDate = pacificTime,
                    Activity = "Sample file upload failed",
                    PatientId = patientId,
                    ProviderId = 0,
                    ReportId = 0,
                    Status = 1
                };

                var result = await _userRepository.CreateAuditLog(createAuditLog, obj);
            }

            return Ok(obj);
        }

        [Produces("application/json")]
        [HttpGet("GetPatientsGeneDumpbyId/{patientId}/{reportMasterId}", Name = "GetPatientsGeneDumpbyId")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetPatientsGeneDumpbyId(int patientId, int reportMasterId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                List<PatientGeneDumpProcModel> patientGeneDumpByIdProcModel = new List<PatientGeneDumpProcModel>();
                patientGeneDumpByIdProcModel = await _patientrepository.GetPatientsGeneDumpbyId(patientId, reportMasterId);

                var patientDetailsEntity = _mapper.Map<List<PatientGeneDumpModel>>(patientGeneDumpByIdProcModel);

                serviceResponse.Data = patientDetailsEntity;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GeneratePatientReport/{patientId}/{reportMasterId}/{generatedDate}/{generatedBy}", Name = "GeneratePatientReport")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GeneratePatientReport(int patientId, int reportMasterId, DateTime? generatedDate, int generatedBy)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            string uuid = Guid.NewGuid().ToString();

            string codeReportUrl = "https://endodna.blob.core.windows.net/" + _azureContainer + "/" + _azureContainerFolder + "/" + uuid + ".pdf#page=";

            try
            {
                string[] mergerPdf = Array.Empty<string>();
                List<string> mergePdfList = new List<string>();
                string templateDesign;

                PatientsReportDataModel patientsReportData = new PatientsReportDataModel();

                patientsReportData = await _patientrepository.GetPatientsReportData(patientId, reportMasterId);

                patientsReportData.ReportBasicInfo.PageLinkUrl = codeReportUrl;

                templateDesign = await _patientrepository.GetClientsReportTemplate(patientId, reportMasterId);

                string headerDownloadUrl = string.Empty;
                string path1 = string.Empty;
                string base641 = await _reportService.GenerateGeneReport(0, patientsReportData, new List<GeneReportDetails>(), reportMasterId, generatedDate, 0, templateDesign);

                _storageService.UploadAttachment(base641, "application/pdf", out headerDownloadUrl, out path1, "", ".pdf");
                if (headerDownloadUrl != null)
                {
                    mergePdfList.Add(headerDownloadUrl);
                }

                var patientReportGenesLst = patientsReportData.PatientGenes.GroupBy(x => x.category).ToList();

                for (int i = 0; i < patientReportGenesLst.Count; i++)
                {
                    List<GeneReportDetails> reportDetails = new List<GeneReportDetails>();

                    var res = patientReportGenesLst[i].ToList();

                    var subCategory = res.GroupBy(x => x.sub_category).Distinct().ToList();
                    var s = subCategory.ToList();

                    List<string> subC = new List<string>();
                    List<string> subCR = new List<string>();

                    foreach (var item in s)
                    {
                        subC.Add(item.Key);

                        foreach (var item1 in item)
                        {
                            string resultColorUrl = await _patientrepository.GetColorUrlByResult(item1.category_id, item1.sub_category_id, item1.result, item1.result_color);

                            if (String.IsNullOrEmpty(resultColorUrl))
                            {
                                resultColorUrl = "https://endodna.blob.core.windows.net/analyzer/Default_Graph.png";
                            }
                            subCR.Add(resultColorUrl);

                            break;
                        }
                    }

                    string subCategoryText = string.Join("", subC);

                    for (int j = 0; j < res.Count; j++)
                    {
                        GeneReportDetails geneReport = new GeneReportDetails
                        {
                            Category = res[j].category.ToUpper(),
                            SubCategory = res[j].sub_category,
                            Gene = res[j].gene,
                            Snp = res[j].snp,
                            Genotype = res[j].genotype,
                            Results = string.IsNullOrEmpty(res[j].genotype_result) ? res[j].genotype_result : res[j].genotype_result.Replace("|", "\n"),
                            ReportIntroduction = res[j].report_introduction,
                            StudyName = string.IsNullOrEmpty(res[j].study_name) ? "Study name: “" + res[j].study_name + "”" : "Study name: “" + res[j].study_name.Replace("|", "\n") + "”",
                            StudyDescription = string.IsNullOrEmpty(res[j].study_description) ? res[j].study_description : res[j].study_description.Replace("|", "\n"),

                            ReferenceLink = string.IsNullOrEmpty(res[j].study_link) ? res[j].study_link : res[j].study_link.Replace("|", "\n"),


                            //ReferenceLink =
                            //    string.IsNullOrEmpty(res[j].study_link)
                            //    ? res[j].study_link
                            //    : (res[j].study_link.Contains("|")
                            //        ? res[j].study_link.Replace("|", "\n")
                            //        : (res[j].study_link.Contains("%0A")
                            //            ? string.Join("\n", res[j].study_link.Split(new string[] { "%0A", "\n" }, StringSplitOptions.RemoveEmptyEntries))
                            //            : res[j].study_link)
                            //      ),


                            SubCategory1 = subC.Count != 0 ? subC[0] : "",
                            SubCategory2 = subC.Count >= 2 ? subC[1] : "",
                            SubCategory3 = subC.Count >= 3 ? subC[2] : "",
                            SubCategory4 = subC.Count >= 4 ? subC[3] : "",
                            SubCategory5 = subC.Count >= 5 ? subC[4] : "",
                            SubCategory6 = subC.Count >= 6 ? subC[5] : "",
                            SubCategoryResultColor1 = subCR.Count != 0 ? subCR[0] : "",
                            SubCategoryResultColor2 = subCR.Count >= 2 ? subCR[1] : "",
                            SubCategoryResultColor3 = subCR.Count >= 3 ? subCR[2] : "",
                            SubCategoryResultColor4 = subCR.Count >= 4 ? subCR[3] : "",
                            SubCategoryResultColor5 = subCR.Count >= 5 ? subCR[4] : "",
                            SubCategoryResultColor6 = subCR.Count >= 6 ? subCR[5] : ""
                        };

                        reportDetails.Add(geneReport);

                    }


                    string downloadUrl = string.Empty;
                    string path = string.Empty;
                    string base64 = await _reportService.GenerateGeneReport(1, patientsReportData, reportDetails, reportMasterId, generatedDate, i, templateDesign);

                    _storageService.UploadAttachment(base64, "application/pdf", out downloadUrl, out path, "", ".pdf");
                    if (downloadUrl != null)
                    {
                        mergePdfList.Add(downloadUrl);

                        //if (i == 1)
                        //    break;
                    }
                }

                mergerPdf = mergePdfList.ToArray();

                if (mergerPdf.Length > 0)
                {
                    string finaldownloadUrl = string.Empty;
                    string finalpath = string.Empty;

                    string finalPdfbase64 = Convert.ToBase64String(await _storageService.MergeMultiplePDF(mergerPdf));

                    _storageService.UploadAttachment(finalPdfbase64, "application/pdf", out finaldownloadUrl, out finalpath, "", ".pdf", uuid);

                    if (!String.IsNullOrEmpty(finaldownloadUrl))
                    {
                        int finalRes = await _patientrepository.UpdatePatientReportUrl(patientId, finaldownloadUrl, reportMasterId, generatedBy, generatedDate);
                    }

                    serviceResponse.Data = finaldownloadUrl;
                }
                else
                {
                    serviceResponse.Data = "No Result";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
                serviceResponse.Data = "";

                AuditOpeningData auditLogOpeningData = new AuditOpeningData();
                auditLogOpeningData = await _userRepository.GetAuditOpeningData();

                DateTime localTime = DateTime.Now; 

                TimeZoneInfo pacificTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

                DateTime pacificTime = TimeZoneInfo.ConvertTime(localTime, TimeZoneInfo.Local, pacificTimeZone);

                CreateAuditLog createAuditLog = new CreateAuditLog
                {
                    AuditUserId = generatedBy,
                    UserId = 0,
                    AuditCategoryMasterId = auditLogOpeningData.AuditMaster.Where(x => x.audit_category_name == "Report Management").Select(x => x.audit_category_master_id).First(),

                    AuditDate = pacificTime,

                    Activity = "Report generation failed",
                    PatientId = patientId,
                    ProviderId = 0,
                    ReportId = reportMasterId,
                    Status = 1

                };

                var result = await _userRepository.CreateAuditLog(createAuditLog, new ServiceResponse());
            }

            return serviceResponse;
        }

        [Produces("application/json")]
        [HttpGet("GetPatientReportDetails/{patientId}", Name = "GetPatientReportDetails")]
        [Authorize]
        public async Task<ActionResult<ServiceResponse>> GetPatientReportDetails(int patientId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                PatientReportDetailsData patientReportData = new PatientReportDetailsData();
                patientReportData = await _patientrepository.GetPatientReportDetails(patientId);

                serviceResponse.Data = patientReportData;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetPatientReportHistoryList/{clientId}", Name = "GetPatientReportHistoryList")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetPatientReportHistoryList(int clientId)
        {
            try
            {
                ServiceResponse serviceResponse = new ServiceResponse();

                List<PatientReportHistoryModel> patientReportHistoryList = new List<PatientReportHistoryModel>();

                patientReportHistoryList = await _patientrepository.GetPatientReportHistoryList(clientId);
                var patientDetailsEntity = _mapper.Map<List<PatientReportHistoryDto>>(patientReportHistoryList);

                serviceResponse.Data = patientDetailsEntity;

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Produces("application/json")]
        [HttpGet("GenerateReportOpeningData/{clientId}", Name = "GenerateReportOpeningData")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GenerateReportOpeningData(int clientId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                ReportOpeningData reportOpeningData = new ReportOpeningData();
                reportOpeningData = await _patientrepository.GenerateReportOpeningData(clientId);

                serviceResponse.Data = reportOpeningData;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetPatientHistory/{patientId}/{reportId}", Name = "GetPatientHistory")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetPatientHistory(int patientId, int reportId)
        {
            try
            {
                ServiceResponse serviceResponse = new ServiceResponse();

                List<PatientHistoryModel> patientHistory = new List<PatientHistoryModel>();

                patientHistory = await _patientrepository.GetPatientHistory(patientId, reportId);
                var patientDetailsEntity = _mapper.Map<List<PatientHistoryDto>>(patientHistory);

                serviceResponse.Data = patientDetailsEntity;

                return Ok(serviceResponse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Produces("application/json")]
        [HttpGet("GetPatientListByClientId/{clientId}", Name = "GetPatientListByClientId")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetPatientListByClientId(int clientId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                PatientListByClientIdData patientOpeningData = new PatientListByClientIdData();
                patientOpeningData = await _patientrepository.GetPatientListByClientId(clientId);

                serviceResponse.Data = patientOpeningData;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetPatientListByProviderId/{providerId}", Name = "GetPatientListByProviderId")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetPatientListByProviderId(int providerId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                List<PatientListByProviderIdProcModel> patientByProviderIdProcModel = new List<PatientListByProviderIdProcModel>();
                patientByProviderIdProcModel = await _patientrepository.GetPatientListByProviderId(providerId);

                var patientDetailsEntity = _mapper.Map<List<PatientListByProviderIdData>>(patientByProviderIdProcModel);

                serviceResponse.Data = patientDetailsEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetCategoryByPatientId/{patientId}", Name = "GetCategoryByPatientId")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetCategoryByPatientId(int patientId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();
            try
            {
                CategoryReportData categoryReportData = new CategoryReportData();
                categoryReportData = await _patientrepository.GetCategoryByPatientId(patientId);

                serviceResponse.Data = categoryReportData;
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500;
                serviceResponse.StatusMessage = ex.Message;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpGet("GetReportColorByClientId/{clientId}", Name = "GetReportColorByClientId")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult<ServiceResponse>> GetReportColorByClientId(int clientId)
        {
            ServiceResponse serviceResponse = new ServiceResponse();

            try
            {
                ReportColoursProcModel reportColorByClientIdProcModel = new ReportColoursProcModel();
                reportColorByClientIdProcModel = await _patientrepository.GetReportColorByClientId(clientId);

                var reportColorDetailsEntity = _mapper.Map<ReportColoursDto>(reportColorByClientIdProcModel);


                serviceResponse.Data = reportColorDetailsEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ok(serviceResponse);
        }

        [Produces("application/json")]
        [HttpPut("UpdateReportColorByClientId", Name = "UpdateReportColorByClientId")]
        [Authorize]
        [GzipCompression]
        public async Task<ActionResult> UpdateReportColorByClientId(UpdateReportColorDto updateReportColor, ApiVersion version)
        {
            ServiceResponse obj = new ServiceResponse();
            int result = 0;
            try
            {
                result = await _patientrepository.UpdateReportColorByClientId(updateReportColor, obj);
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

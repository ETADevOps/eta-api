using ETA.API.Models.StoreProcContextModel;
using ETA_API.Helpers;
using ETA_API.Models.Referance;
using ETA_API.Models.StoreProcModelDto;
using ETA_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ETA_API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CommonController : ControllerBase
    {
        private readonly IReportServices _reportService;
        private readonly IStorageService _storageService;
        private readonly ICommonService _commonService;
        public CommonController(IReportServices reportServices, IStorageService storageService, ICommonService commonService)
        {
            _reportService = reportServices ?? throw new ArgumentNullException(nameof(reportServices));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            _commonService = commonService ?? throw new ArgumentNullException(nameof(commonService));
        }

        [Produces("application/json")]
        [HttpPost("[action]")]
        public async Task<ActionResult<ServiceResponse>> UploadImage(Attachments attachments)
        {
            string downloadUrl = string.Empty;
            string path1 = string.Empty;

            _storageService.UploadImage(attachments.file_url, attachments.file_type, out downloadUrl, out path1, "", attachments.file_extension);

            ServiceResponse serviceResponse = new ServiceResponse
            {
                Data = downloadUrl
            };

            return serviceResponse;
        }
    }
}
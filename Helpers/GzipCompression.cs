using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using System.IO;
using System.Collections.Generic;
using Microsoft.Net.Http.Headers;
using ETA.API.Models.StoreProcContextModel;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace ETA_API.Helpers
{
    public class GzipCompressionAttribute : ActionFilterAttribute
    {
        public override async void OnActionExecuted(ActionExecutedContext context)
        {
            var result = context.Result;
            if (result is ServiceResponse serviceResponse)
            {
                var content = serviceResponse.Data;
                if (content != null)
                {
                    byte[] data = System.Text.Encoding.UTF8.GetBytes(content.ToString());

                    // Compress data
                    byte[] compressedData = CompressionHelper.Compress(data);

                    // Set compressed content
                    context.HttpContext.Response.Headers[HeaderNames.ContentEncoding] = "gzip";
                    context.HttpContext.Response.Headers[HeaderNames.ContentType] = "application/json";
                    await context.HttpContext.Response.Body.WriteAsync(compressedData, 0, compressedData.Length);

                }
            }
        }

        public class CompressionHelper
        {
            public static byte[] Compress(byte[] data)
            {
                using (MemoryStream output = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(output, CompressionMode.Compress))
                    {
                        gzip.Write(data, 0, data.Length);
                    }
                    return output.ToArray();
                }
            }
        }
    }
}

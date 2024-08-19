using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ETA_API.Models.Referance;
using ETA_API.Models.StoreProcModelDto;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Hangfire.MemoryStorage.Database;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using Newtonsoft.Json;
using Npgsql;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ETA_API.Helpers
{
    public class StorageService : IStorageService
    {
        private readonly IConfiguration _configuration;
        private readonly string _azureConnectionString;
        private readonly string _azureContainer;
        private readonly GoogleCredential _googleCredential;
        private readonly GCSConfigOptions _options;
        private readonly string _azureContainerFolder;

        public StorageService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _googleCredential = GoogleCredential.FromFile($@"Reports/GeneReports/virtual-silo-432705-s7-8719b932f840.json");
        }
       
        public async Task<string> Post(string uri, Dictionary<string, string> parameters)
        {
            HttpResponseMessage response = null;
            try
            {
                using (var httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(30) })
                {
                    response = await httpClient.PostAsync(uri, new FormUrlEncodedContent(parameters));
                    if (!response.IsSuccessStatusCode)
                        throw new Exception("post request failed.");

                    var content = response.Content.ReadAsStringAsync().Result;
                    if (string.IsNullOrWhiteSpace(content))
                        throw new Exception("empty response received.");

                    return content;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public void UploadAttachment(string source, string fileType, out string downloadURL, out string path, string location, string extension, string uuid = null)
        {
            try
            {
                var storage = StorageClient.Create(_googleCredential);
                string bucketName = "eta-storage";

                string base64 = source.Substring(source.IndexOf(',') + 1);

                string reportUuid = uuid == null ? Guid.NewGuid().ToString() : uuid;
                byte[] data = Convert.FromBase64String(base64);
                var uploadedFile = storage.UploadObject(bucketName, reportUuid, fileType, new MemoryStream(data));
                downloadURL = $"https://storage.cloud.google.com/{bucketName}/{reportUuid}";
                path = "";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                downloadURL = "";
                path = "";
            }
        }
        private async Task<BlobContainerClient> GetContainerAsync(string containerName)
        {
            var cloudStorageAccount = new BlobContainerClient(_azureConnectionString, containerName);
            await cloudStorageAccount.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob, null, null);
            return cloudStorageAccount;
        }
        public Task<byte[]> MergeMultiplePDF(string[] PDFfileNames)
        {
            try
            {
                // Create document object  
                iTextSharp.text.Document PDFdoc = new iTextSharp.text.Document();
                // Create a object of FileStream which will be disposed at the end  
                using (System.IO.MemoryStream MyFileStream = new System.IO.MemoryStream())
                {
                    // Create a PDFwriter that is listens to the Pdf document  
                    iTextSharp.text.pdf.PdfCopy PDFwriter = new iTextSharp.text.pdf.PdfCopy(PDFdoc, MyFileStream);
                    if (PDFwriter == null)
                    {
                        return null;
                    }
                    // Open the PDFdocument  
                    PDFdoc.Open();
                    foreach (string fileName in PDFfileNames)
                    {
                        // Create a PDFreader for a certain PDFdocument  
                        iTextSharp.text.pdf.PdfReader PDFreader = new iTextSharp.text.pdf.PdfReader(fileName);
                        PDFreader.ConsolidateNamedDestinations();
                        // Add content  
                        for (int i = 1; i <= PDFreader.NumberOfPages; i++)
                        {
                            iTextSharp.text.pdf.PdfImportedPage page = PDFwriter.GetImportedPage(PDFreader, i);
                            PDFwriter.AddPage(page);
                        }
                        iTextSharp.text.pdf.PrAcroForm form = PDFreader.AcroForm;
                        if (form != null)
                        {
                            PDFwriter.CopyAcroForm(PDFreader);
                        }
                        // Close PDFreader  
                        PDFreader.Close();
                    }
                    // Close the PDFdocument and PDFwriter  
                    PDFwriter.Close();
                    PDFdoc.Close();

                    return Task.Run(() => MyFileStream.ToArray());
                }// Disposes the Object of FileStream  

            }
            catch (Exception ex)
            {
                string outval = ex.Message;

                throw ex;
            }
        }
        public async Task<string> UploadDNAAzureJob(int patientId, int createdBy, DateTime? createdDate, IFormFile file)
        {
            string result = "";
            string azureFunctionUrl = _configuration.GetSection("urls").GetSection("AzureFunctionUrl").Value;
            string azureFunctionToken = _configuration.GetSection("urls").GetSection("AzureFunctionToken").Value;

            try
            {
                var content = new MultipartFormDataContent();
                var fileContent = new StreamContent(file.OpenReadStream());
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);

                content.Add(fileContent, "file", file.FileName);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(azureFunctionUrl + "api/DNAFileUpload?code=" + azureFunctionToken + "&PatientId=" + patientId + "&CreatedBy=" + createdBy);
                    client.DefaultRequestHeaders.Accept.Clear();

                    //HTTP POST
                    HttpResponseMessage response = await client.PostAsync(client.BaseAddress, content);
                    if (response.IsSuccessStatusCode)
                    {
                        await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
                        await conn.OpenAsync();
                        string query = $"UPDATE patients SET import_gene_date = :pgenerated_date WHERE  patient_id = " + patientId + "";

                        await using (var cmd = new NpgsqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue(":pgenerated_date", createdDate);
                            try
                            {
                                NpgsqlDataReader dataReader = cmd.ExecuteReader();
                                var data = await response.Content.ReadAsStringAsync();
                                result = data.ToString();

                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }


                    }
                    else
                    {
                        result = response.ReasonPhrase.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
        public void UploadImage(string source, string fileType, out string downloadURL, out string path, string location, string extension)
        {
            try
            {
                var cloudBlobContainer = GetContainerAsync(_azureContainer).Result;
                string base64 = source.Substring(source.IndexOf(',') + 1);

                var link = $"Default_Graph" + extension;

                byte[] data = Convert.FromBase64String(base64);
                BlobClient destBlob = cloudBlobContainer.GetBlobClient(link);
                destBlob.UploadAsync(new MemoryStream(data), new BlobHttpHeaders { ContentType = fileType }).GetAwaiter().GetResult();
                downloadURL = destBlob.Uri.AbsoluteUri;
                path = link;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                downloadURL = "";
                path = "";
            }
        }

        public async Task<string> SendVerificationEmail(string toEmail)
        {
            string otp = await GenerateOTP();

            string res = otp;
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(MailboxAddress.Parse("support@mydnamd.live"));
            mailMessage.To.Add(MailboxAddress.Parse(toEmail));
            mailMessage.Subject = "Verification Code";
            mailMessage.Body = new TextPart("html")
            {
                Text = "<p> Hi,</p>\r\n" +
                "<p>Your OTP for secure login is <b>" + otp + "</b>. Please do not share this code with anyone for your security.</p>\r\n" +
                "<p>This verification is critical to access the application.</p>\r\n" +
                 "<p>Regards,</p>\r\n" +
                 "<p>Support Team </p>\r\n" +
                 "<p>mydnamd.live </p>\r\n"
            };

            using (var smtpClient = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    smtpClient.Connect("outlook.office365.com", 587);
                    smtpClient.Authenticate("support@mydnamd.live", "916*0G6bt$n7D");
                    smtpClient.Send(mailMessage);
                    smtpClient.Disconnect(true);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
               
            }

            return res;

        }

        public async Task<string> GenerateOTP()
        {
            Random random = new Random();
            int otpNumber = random.Next(100000, 999999);
            string otpString = otpNumber.ToString("D6"); // Convert to string with leading zeros if necessary
            return otpString;
        }

    }
}
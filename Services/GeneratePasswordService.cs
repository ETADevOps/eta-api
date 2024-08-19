using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;
using Npgsql;
using System.Security.Cryptography;

namespace ETA_API.Services
{
    public class GeneratePasswordService : IGeneratePasswordService
    {
        private IConfiguration _configuration;
        public GeneratePasswordService(IConfiguration Configuration)
        {
            _configuration = Configuration ?? throw new ArgumentNullException(nameof(Configuration));
        }

        public async Task<string> GeneratePassword(int length, int numberOfNonAlphanumericCharacters)
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            const string nonAlphanumericChars = "!@#$%^&*()_-+=[{]};:<>|./?";
            var randNum = new byte[4];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randNum);
                var randomSeed = BitConverter.ToInt32(randNum, 0);
                var random = new Random(randomSeed);
                var chars = new char[length];
                var allowedCharCount = allowedChars.Length;
                var nonAlphanumericCharCount = nonAlphanumericChars.Length;
                var numNonAlphanumericCharsAdded = 0;
                for (var i = 0; i < length; i++)
                {
                    if (numNonAlphanumericCharsAdded < numberOfNonAlphanumericCharacters && i < length - 1)
                    {
                        chars[i] = nonAlphanumericChars[random.Next(nonAlphanumericCharCount)];
                        numNonAlphanumericCharsAdded++;
                    }
                    else
                    {
                        chars[i] = allowedChars[random.Next(allowedCharCount)];
                    }
                }
                return new string(chars);
            }
        }

       

        public async Task<int> UpdatePassword(int userId, string password, ServiceResponse obj)
        {
            //string password1 = await GeneratePassword(15, 3);

            await using var conn = new NpgsqlConnection(_configuration["ConnectionStrings:DBConnectionString"]);
            await conn.OpenAsync();
            int result = 0;

            string query = $"SELECT * FROM update_password(:puser_id, :ppassword)";

            await using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(":puser_id", userId);
                cmd.Parameters.AddWithValue(":ppassword", password);

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

using Newtonsoft.Json;

namespace ETA_API.Models.Referance
{
    public class TokenRequest
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
    public class DeleteByIdModel
    {
        public int Id { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class CursorMapping
    {
        public string cursorName { get; set; }
        public string cursorModelType { get; set; }

        public CursorMapping(string iCursorName, string cursorModelType)
        {
            this.cursorName = iCursorName;
            this.cursorModelType = cursorModelType;
        }
    }
    public class Attachments
    {
        public string file_name { get; set; }
        public string file_type { get; set; }
        public string file_extension { get; set; }
        public string file_path { get; set; }
        public string file_url { get; set; }
    }
}

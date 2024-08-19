namespace ETA.API.Models.StoreProcContextModel
{
    public class ServiceResponse
    {
        public int StatusCode { get; set; } = 200;
        public string StatusMessage { get; set; } = "Success";
        public object Data { get; set; }
    }
    enum StatusCodes
    {
        Unique_Code_Not_Found = 100
    }
}

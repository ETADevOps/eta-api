namespace ETA_API.Models.StoreProcModelDto
{
    public class ProvidersDto
    {
        public int ProviderId { get; set; }
        public string Providername { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public Int16 Status { get; set; }
    }
    public class CreateProviderDto
    {
        public int ProviderId { get; set; }
        public string Providername { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int ClientId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Int16 Status { get; set; }
    }

    public class ProviderClientMaster
    {
        public int client_id { get; set; }
        public string client_name { get; set; }
    }

    public class ProviderOpeningData
    {
        public List<ProviderClientMaster> ProviderClientMaster { get; set; }
    }

    public class ProviderListByClientId
    {
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }
    }
}

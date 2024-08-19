namespace ETA_API.Models.StoreProcContextModel
{
    public class ProvidersProcModel
    {
        public int pprovider_id { get; set; }
        public string pprovider_name { get; set; }
        public string paddress { get; set; }
        public string pcity { get; set; }
        public string pstate { get; set; }
        public string pzip { get; set; }
        public string pphone { get; set; }
        public string pemail { get; set; }
        public int pclient_id { get; set; }
        public string pclient_name { get; set; }
        public Int16 pstatus { get; set; }
    }

    public class ProviderListByClientIdProcModel
    {
        public int pprovider_id { get; set; }
        public string pprovider_name { get; set; }
    }
}

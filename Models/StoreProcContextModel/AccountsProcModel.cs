namespace ETA_API.Models.StoreProcContextModel
{
    public class AccountsProcModel
    {
        public int paccount_id { get; set; }
        public string paccount_name { get; set; }
        public string paccount_logo { get; set; }
        public string paddress { get; set; }
        public string pcity { get; set; }
        public string pstate { get; set; }
        public string pzip { get; set; }
        public string pphone { get; set; }
        public string pfax { get; set; }
        public string pcontact_person_name { get; set; }
        public string ptimezone { get; set; }
    }

    public class AccountsByIdProcModel
    {
        public string paccount_name { get; set; }
        public string paccount_logo { get; set; }
        public string paddress { get; set; }
        public string pcity { get; set; }
        public string pstate { get; set; }
        public string pzip { get; set; }
        public string pphone { get; set; }
        public string pfax { get; set; }
        public string pcontact_person_name { get; set; }
        public string ptimezone { get; set; }
    }
}

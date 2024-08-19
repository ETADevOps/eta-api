namespace ETA_API.Models.StoreProcModelDto
{
    public class AccountsDto
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountLogo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string ContactPersonName { get; set; }
        public string TimeZone { get; set; }
    }

    public class AccountByIdDto
    {
        public string AccountName { get; set; }
        public string AccountLogo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string ContactPersonName { get; set; }
        public string TimeZone { get; set; }
    }

    public class UpdateAccountDto
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountLogo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonPhone { get; set; }
        public string ContactPersonEmail { get; set; }
        public string TimeZone { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}

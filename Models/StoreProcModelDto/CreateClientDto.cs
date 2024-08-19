using ETA.API.Models.StoreProcModelDto;

namespace ETA_API.Models.StoreProcModelDto
{
    public class CreateClientDto
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientLogo { get; set; }
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
        public List<ClientDomainrelation> ClientDomainrelations { get; set; }
        public List<ClientReportTemplaterelation> ClientReportTemplaterelations { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class ClientReportTemplaterelation
    {
        public int client_id { get; set; }
        public int report_master_id { get; set; }
        public int report_template_mapping_id { get; set; }
        public int client_report_mapping_id { get; set; }
    }

    public class ClientDomainrelation
    {
        public int domain_master_id { get; set; }
        public int domain_client_mapping_id { get; set; }
    }

    public class ClientReportMaster
    {
        public int report_master_id { get; set; }
        public string report_name { get; set; }
    }

    public class ClientReportTemplateMapping
    {
        public int report_template_mapping_id { get; set; }
        public int report_master_id { get; set; }
        public string template_name { get; set; }
        public string template_url { get; set; }
    }

    public class ClientOpeningData
    {
        public List<ClientReportMaster> ClientReportMaster { get; set; }
        public List<ClientReportTemplateMapping> ClientReportTemplateMapping { get; set; }
    }

    public class ClientDto
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientLogo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string ContactPersonName { get; set; }
        public string TimeZone { get; set; }
        public string Domains { get; set; }
    }

    public class ClientMaster
    {
        public int client_id { get; set; }
        public string client_name { get; set; }
        public string client_logo { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public string contact_person_name { get; set; }
        public string contact_person_phone { get; set; }
        public string contact_person_email { get; set; }
        public string timezone { get; set; }
    }

    public class ClientReportTemplateMaster
    {
        public int client_report_mapping_id { get; set; }
        public int client_id { get; set; }
        public int report_master_id { get; set; }
        public int report_template_mapping_id { get; set; }
    }

    public class DomainMaster
    {
        public int domain_client_mapping_id { get; set; }
        public int domain_master_id { get; set; }
        public string domain_name { get; set; }
    }

    public class ClientByIdOpeningData
    {
        public ClientMaster ClientMasters { get; set; }
        public List<DomainMaster> DomainMasters { get; set; }
        public List<ClientReportTemplateMaster> ClientReportTemplateMasters { get; set; }

    }
}

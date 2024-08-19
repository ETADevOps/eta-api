using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class PatientReportHistoryProfile : Profile
    {
        public PatientReportHistoryProfile()
        {
            CreateMap<PatientReportHistoryDto, PatientReportHistoryModel>()
           .ForMember(a => a.ppatient_id, b => b.MapFrom(c => c.PatientId))
           .ForMember(a => a.pfirst_name, b => b.MapFrom(c => c.FirstName))
           .ForMember(a => a.plast_name, b => b.MapFrom(c => c.LastName))
           .ForMember(a => a.pgender, b => b.MapFrom(c => c.Gender))
           .ForMember(a => a.pdate_of_birth, b => b.MapFrom(c => c.DateOfBirth))
           .ForMember(a => a.pprovider_id, b => b.MapFrom(c => c.ProviderId))
           .ForMember(a => a.pprovider_name, b => b.MapFrom(c => c.ProviderName))
           .ForMember(a => a.pclient_name, b => b.MapFrom(c => c.ClientName))
           .ForMember(a => a.psample_id, b => b.MapFrom(c => c.SampleId))
           .ForMember(a => a.pimport_gene_date, b => b.MapFrom(c => c.ImportGeneDate))
           .ForMember(a => a.pgene_file_url, b => b.MapFrom(c => c.PatientGeneFile))
           .ForMember(a => a.preport_file_urls, b => b.MapFrom(c => c.ReportFileUrls))
           .ReverseMap();

        }
    }
}

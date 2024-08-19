using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class PatientsProfile : Profile
    {
        public PatientsProfile()
        {
            CreateMap<PatientsDto, PatientsProcModel>()
           .ForMember(a => a.ppatient_id, b => b.MapFrom(c => c.PatientId))
           .ForMember(a => a.pfirst_name, b => b.MapFrom(c => c.FirstName))
           .ForMember(a => a.plast_name, b => b.MapFrom(c => c.LastName))
           .ForMember(a => a.pgender, b => b.MapFrom(c => c.Gender))
           .ForMember(a => a.pdate_of_birth, b => b.MapFrom(c => c.DateOfBirth))
           .ForMember(a => a.paddress, b => b.MapFrom(c => c.Address))
           .ForMember(a => a.pcity, b => b.MapFrom(c => c.City))
           .ForMember(a => a.pstate, b => b.MapFrom(c => c.State))
           .ForMember(a => a.pzip, b => b.MapFrom(c => c.Zip))
           .ForMember(a => a.pspecimen_type, b => b.MapFrom(c => c.SpecimenType))
           .ForMember(a => a.pcollection_date, b => b.MapFrom(c => c.CollectionDate))
           .ForMember(a => a.pcollection_method, b => b.MapFrom(c => c.CollectionMethod))
           .ForMember(a => a.pimport_gene_date, b => b.MapFrom(c => c.ImportGeneDate))
           .ForMember(a => a.pprovider_id, b => b.MapFrom(c => c.ProviderId))
           .ForMember(a => a.pprovider_name, b => b.MapFrom(c => c.ProviderName))
           .ForMember(a => a.pclient_id, b => b.MapFrom(c => c.ClientId))
           .ForMember(a => a.pclient_name, b => b.MapFrom(c => c.ClientName))
           .ForMember(a => a.psample_id, b => b.MapFrom(c => c.SampleId))
           .ForMember(a => a.pgene_file_url, b => b.MapFrom(c => c.PatientGeneFile))
           .ForMember(a => a.preport_file_urls, b => b.MapFrom(c => c.PatientReportUrls))
           .ForMember(a => a.preport_generated, b => b.MapFrom(c => c.ReportGenerated))
           .ForMember(a => a.pstatus, b => b.MapFrom(c => c.Status))
           .ReverseMap();
        }
    }
}

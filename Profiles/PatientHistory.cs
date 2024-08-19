using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class PatientHistory : Profile
    {
        public PatientHistory()
        {
            CreateMap<PatientHistoryDto, PatientHistoryModel>()
           .ForMember(a => a.preport_master_id, b => b.MapFrom(c => c.ReportMasterId))
           .ForMember(a => a.preport_name, b => b.MapFrom(c => c.ReportName))
           .ForMember(a => a.psample_id, b => b.MapFrom(c => c.SampleId))
           .ForMember(a => a.preport_create_date, b => b.MapFrom(c => c.ReportCreateDate))
           .ForMember(a => a.pimport_gene_date, b => b.MapFrom(c => c.ImportGeneDate))
           .ForMember(a => a.pis_interpretation_exist, b => b.MapFrom(c => c.IsInterpretationExist))
           .ForMember(a => a.pgene_file_url, b => b.MapFrom(c => c.GeneFileUrl))
           .ForMember(a => a.preport_file_url, b => b.MapFrom(c => c.ReportFileUrl))
           .ReverseMap();

        }
    }
}

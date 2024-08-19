//using AutoMapper;
//using ETA_API.Models.StoreProcContextModel;
//using ETA_API.Models.StoreProcModelDto;

//namespace ETA_API.Profiles
//{
//    public class PatientReportUrl : Profile
//    {
//        public PatientReportUrl()
//        {
//            CreateMap<PatientReportDetailsUrl, PatientReportDetailsModel>()
//            .ForMember(a => a.preport_master_id, b => b.MapFrom(c => c.ReportMasterId))
//            .ForMember(a => a.preport_name, b => b.MapFrom(c => c.ReportName))
//            .ForMember(a => a.preport_create_date, b => b.MapFrom(c => c.ReportCreateDate))
//            .ForMember(a => a.preport_card_url, b => b.MapFrom(c => c.ReportCardUrl))
//            .ForMember(a => a.preport_file_url, b => b.MapFrom(c => c.ReportFileUrl))
//            .ReverseMap();
//        }
//    }
//}

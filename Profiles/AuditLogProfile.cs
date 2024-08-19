using AutoMapper;
using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class AuditLogProfile : Profile
    {
        public AuditLogProfile()
        {

            CreateMap<CreateAuditLog, CreateAuditLogProcModel>()
            .ForMember(a => a.puser_id, b => b.MapFrom(c => c.UserId))
            .ForMember(a => a.paudit_category_master_id, b => b.MapFrom(c => c.AuditCategoryMasterId))
            .ForMember(a => a.paudit_date, b => b.MapFrom(c => c.AuditDate))
            .ForMember(a => a.pactivity, b => b.MapFrom(c => c.Activity))
            .ForMember(a => a.ppatient_id, b => b.MapFrom(c => c.PatientId))
            .ForMember(a => a.pprovider_id, b => b.MapFrom(c => c.ProviderId))
            .ForMember(a => a.preport_id, b => b.MapFrom(c => c.ReportId))
            .ForMember(a => a.pstatus, b => b.MapFrom(c => c.Status))

            .ReverseMap();
        }
    }
}

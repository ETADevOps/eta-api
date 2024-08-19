using AutoMapper;
using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class GetAuditLogProfile : Profile
    {
        public GetAuditLogProfile()
        {
            CreateMap<AuditLogDto, AuditLogProcModel>()
            .ForMember(a => a.audit_log_id, b => b.MapFrom(c => c.AuditLogId))
            .ForMember(a => a.user_id, b => b.MapFrom(c => c.UserId))
            .ForMember(a => a.username, b => b.MapFrom(c => c.UserName))
            .ForMember(a => a.category_id, b => b.MapFrom(c => c.CategoryId))
            .ForMember(a => a.category_name, b => b.MapFrom(c => c.CategoryName))
            .ForMember(a => a.audit_date, b => b.MapFrom(c => c.AuditDate))
            .ForMember(a => a.activity, b => b.MapFrom(c => c.Activity))
            .ForMember(a => a.status, b => b.MapFrom(c => c.Status))
            .ForMember(a => a.patient_id, b => b.MapFrom(c => c.PatientId))
            .ForMember(a => a.patient_name, b => b.MapFrom(c => c.PatientName))
            .ForMember(a => a.provider_id, b => b.MapFrom(c => c.ProviderId))
            .ForMember(a => a.provider_name, b => b.MapFrom(c => c.ProviderName))
            .ForMember(a => a.report_id, b => b.MapFrom(c => c.ReportId))
            .ForMember(a => a.report_name, b => b.MapFrom(c => c.ReportName))
            .ForMember(a => a.client_id, b => b.MapFrom(c => c.ClientId))
            .ForMember(a => a.client_name, b => b.MapFrom(c => c.ClientName))
            .ForMember(a => a.from_text, b => b.MapFrom(c => c.FromText))
            .ForMember(a => a.to_text, b => b.MapFrom(c => c.ToText))
            .ReverseMap();
        }
    }
}

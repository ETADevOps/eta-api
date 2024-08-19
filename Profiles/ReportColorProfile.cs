using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class ReportColorProfile : Profile
    {
        public ReportColorProfile()
        {
            CreateMap<ReportColoursDto, ReportColoursProcModel>()
           .ForMember(a => a.preport_bg_color, b => b.MapFrom(c => c.ReportBgColor))
           .ForMember(a => a.preport_heading_color, b => b.MapFrom(c => c.ReportHeadingColor))
           .ForMember(a => a.preport_sub_heading_color, b => b.MapFrom(c => c.ReportSubHeadingColor))
           .ForMember(a => a.preport_bg_font_color, b => b.MapFrom(c => c.ReportBgFontColor))
           .ForMember(a => a.preport_heading_font_color, b => b.MapFrom(c => c.ReportHeadingFontColor))
           .ForMember(a => a.preport_sub_heading_font_color, b => b.MapFrom(c => c.ReportSubHeadingFontColor))

           .ReverseMap();
        }
    }
}

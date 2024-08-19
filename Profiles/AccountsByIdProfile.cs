using AutoMapper;
using ETA_API.Models.StoreProcContextModel;
using ETA_API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class AccountsByIdProfile : Profile
    {
        public AccountsByIdProfile()
        {
            CreateMap<AccountByIdDto, AccountsByIdProcModel>()
            .ForMember(a => a.paccount_name, b => b.MapFrom(c => c.AccountName))
            .ForMember(a => a.paccount_logo, b => b.MapFrom(c => c.AccountLogo))
            .ForMember(a => a.paddress, b => b.MapFrom(c => c.Address))
            .ForMember(a => a.pcity, b => b.MapFrom(c => c.City))
            .ForMember(a => a.pstate, b => b.MapFrom(c => c.State))
            .ForMember(a => a.pzip, b => b.MapFrom(c => c.Zip))
            .ForMember(a => a.pphone, b => b.MapFrom(c => c.Phone))
            .ForMember(a => a.pfax, b => b.MapFrom(c => c.Fax))
            .ForMember(a => a.pcontact_person_name, b => b.MapFrom(c => c.ContactPersonName))
            .ForMember(a => a.ptimezone, b => b.MapFrom(c => c.TimeZone))

            .ReverseMap();
        }
    }
}

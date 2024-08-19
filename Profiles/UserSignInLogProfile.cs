using AutoMapper;
using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class UserSignInLogProfile : Profile
    {
        public UserSignInLogProfile()
        {

            CreateMap<UserSignInLogDto, UserSignInLogProcModel>()
            .ForMember(a => a.psign_in_log_id, b => b.MapFrom(c => c.SignInLogId))
            .ForMember(a => a.psign_in_date, b => b.MapFrom(c => c.SignInDate))
            .ForMember(a => a.p_user_id, b => b.MapFrom(c => c.UserId))
            .ForMember(a => a.puser_name, b => b.MapFrom(c => c.UserName))
            .ForMember(a => a.pemail, b => b.MapFrom(c => c.Email))
            .ForMember(a => a.pip_address, b => b.MapFrom(c => c.IpAddress))
            .ForMember(a => a.plocation, b => b.MapFrom(c => c.Location))
            .ForMember(a => a.paction, b => b.MapFrom(c => c.Action))

            .ReverseMap();
        }
    }
}

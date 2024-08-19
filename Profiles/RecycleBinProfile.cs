using AutoMapper;
using ETA.API.Models.StoreProcContextModel;
using ETA.API.Models.StoreProcModelDto;

namespace ETA_API.Profiles
{
    public class RecycleBinProfile :Profile
    {
        public RecycleBinProfile()
        {
            CreateMap<RecycleBinOpeningData, RecycleBinOpeningDataProcModel>()
        .ForMember(a => a.category_id, b => b.MapFrom(c => c.CategoryId))
        .ForMember(a => a.category_value, b => b.MapFrom(c => c.CategoryValue))
        .ReverseMap();
        }
    }
}

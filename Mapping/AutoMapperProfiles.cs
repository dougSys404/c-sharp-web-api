using AutoMapper;
using NZWalks.Api.Model.Domain;
using NZWalks.Api.Model.DTO;

namespace NZWalks.Api.Mapping
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region, RegionsDto>().ReverseMap();

            CreateMap<AddRregioRequestDto, Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
        }

    }
}

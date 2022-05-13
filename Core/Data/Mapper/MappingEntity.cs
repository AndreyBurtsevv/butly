using AutoMapper;
using Bitly.Core.Data.Entities;
using Bitly.Core.Dto;

namespace Bitly.Core.Data.Mapper
{
    public class MappingEntity : Profile
    {
        public MappingEntity()
        {
            CreateMap<Url, UrlDto>().ReverseMap();
        }
    }
}

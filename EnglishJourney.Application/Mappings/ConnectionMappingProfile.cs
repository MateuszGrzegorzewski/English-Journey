using AutoMapper;
using EnglishJourney.Application.Connection;

namespace EnglishJourney.Application.Mappings
{
    public class ConnectionMappingProfile : Profile
    {
        public ConnectionMappingProfile()
        {
            CreateMap<Domain.Entities.ConnectionTopic, ConnectionTopicDto>()
                .ForMember(dto => dto.Attributes, opt => opt.MapFrom(src => src.Attributes)).ReverseMap();

            CreateMap<Domain.Entities.ConnectionAttribute, ConnectionAttributeDto>().ReverseMap();
        }
    }
}
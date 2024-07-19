using AutoMapper;
using EnglishJourney.Application.Statistic;
using EnglishJourney.Domain.Entities;

namespace EnglishJourney.Application.Mappings
{
    public class StatisticMappingProfile : Profile
    {
        public StatisticMappingProfile()
        {
            CreateMap<UserStatistic, UserStatisticDto>().ReverseMap();
        }
    }
}
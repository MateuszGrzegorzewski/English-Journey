using AutoMapper;
using EnglishJourney.Application.Flashcard;

namespace EnglishJourney.Application.Mappings
{
    public class FlashcardMappingProfile : Profile
    {
        public FlashcardMappingProfile()
        {
            CreateMap<Domain.Entities.FlashcardCategory, FlashcardCategoryDto>()
                .ForMember(dto => dto.FlashcardBoxes, opt => opt.MapFrom(src => src.FlashcardBoxes)).ReverseMap();

            CreateMap<Domain.Entities.FlashcardBox, FlashcardBoxDto>()
              .ForMember(dto => dto.Flashcards, opt => opt.MapFrom(src => src.Flashcards)).ReverseMap();

            CreateMap<Domain.Entities.Flashcard, FlashcardDto>()
                .ForMember(dto => dto.FlashcardBox, opt => opt.MapFrom(src => src.FlashcardBox)).ReverseMap();
        }
    }
}
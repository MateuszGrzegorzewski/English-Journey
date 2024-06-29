using AutoMapper;
using EnglishJourney.Application.Note;

namespace EnglishJourney.Application.Mappings
{
    public class NoteMappingProfile : Profile
    {
        public NoteMappingProfile()
        {
            CreateMap<Domain.Entities.Note, NoteDto>().ReverseMap();
        }
    }
}
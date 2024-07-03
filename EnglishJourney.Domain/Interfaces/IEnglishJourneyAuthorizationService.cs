using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;

namespace EnglishJourney.Domain.Interfaces
{
    public interface IEnglishJourneyAuthorizationService
    {
        bool AuthorizeConnection(ConnectionTopic connectionTopic, ResourceOperation resourceOperation);
    }
}
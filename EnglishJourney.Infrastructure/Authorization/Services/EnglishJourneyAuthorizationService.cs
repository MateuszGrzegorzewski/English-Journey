using EnglishJourney.Application.Users;
using EnglishJourney.Domain.Constants;
using EnglishJourney.Domain.Entities;
using EnglishJourney.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EnglishJourney.Application.Infrastructure.Services
{
    public class EnglishJourneyAuthorizationService(ILogger<EnglishJourneyAuthorizationService> logger,
        IUserContext userContext) : IEnglishJourneyAuthorizationService
    {
        private bool Authorize<T>(T resource, string resourceName, Func<T, string> getResourceId, Func<T, string> getResourceTitle, ResourceOperation resourceOperation)
        {
            var user = userContext.GetCurrentUser();
            if (user == null)
            {
                logger.LogInformation("User is not authenticated");
                return false;
            }

            if (resourceOperation != ResourceOperation.ReadAll)
            {
                logger.LogInformation("['{ResourceName}'] User {UserEmail} is trying to {ResourceOperation} {ResourceTitle}",
                    resourceName, user.Email, resourceOperation, getResourceTitle(resource));
            }
            else
            {
                logger.LogInformation("['{ResourceName}'] User {UserEmail} is trying to {ResourceOperation} {ResourceName}",
                    resourceName, user.Email, resourceOperation, resourceName);
            }

            if (resourceOperation == ResourceOperation.Create || resourceOperation == ResourceOperation.ReadAll)
            {
                logger.LogInformation("['{ResourceName}'] Create / read all operation is successful authorized", resourceName);
                return true;
            }

            if ((resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update || resourceOperation == ResourceOperation.Read) && user.Id == getResourceId(resource))
            {
                logger.LogInformation("['{ResourceName}'] Read / delete / update operation is successful authorized", resourceName);
                return true;
            }

            return false;
        }

        public bool AuthorizeConnection(ConnectionTopic connectionTopic, ResourceOperation resourceOperation)
        {
            return Authorize(connectionTopic, "Connections", ct => ct.UserId, ct => ct.Topic, resourceOperation);
        }

        public bool AuthorizeNotes(Domain.Entities.Note note, ResourceOperation resourceOperation)
        {
            return Authorize(note, "Notes", n => n.UserId, n => n.Title, resourceOperation);
        }

        public bool AuthorizeFlashcard(FlashcardCategory flashcardCategory, ResourceOperation resourceOperation)
        {
            return Authorize(flashcardCategory, "Flashcards", fc => fc.UserId, fc => fc.Name, resourceOperation);
        }
    }
}
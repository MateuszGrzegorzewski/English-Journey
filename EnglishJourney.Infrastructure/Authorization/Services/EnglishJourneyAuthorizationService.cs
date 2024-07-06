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
        public bool AuthorizeConnection(ConnectionTopic connectionTopic, ResourceOperation resourceOperation)
        {
            var user = userContext.GetCurrentUser();
            if (user == null)
            {
                logger.LogInformation("User is not authenticated");
                return false;
            }

            if (resourceOperation != ResourceOperation.ReadAll)
            {
                logger.LogInformation("['Connections'] User {UserEmail} is trying to {ResourceOperation} topic / attribute where topic : {ConnectionTopic}",
                    user.Email, resourceOperation, connectionTopic.Topic);
            }
            else
            {
                logger.LogInformation("['Connections'] User {UserEmail} is trying to {ResourceOperation} topics",
                    user.Email, resourceOperation);
            }

            if (resourceOperation == ResourceOperation.Create || resourceOperation == ResourceOperation.ReadAll)
            {
                logger.LogInformation("['Connections'] Create / read all operation is successful authorized");
                return true;
            }

            if ((resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update || resourceOperation == ResourceOperation.Read) && user.Id == connectionTopic.UserId)
            {
                logger.LogInformation("['Connections'] Read / delete / update operation is successful authorized");
                return true;
            }

            return false;
        }

        public bool AuthorizeNotes(Domain.Entities.Note note, ResourceOperation resourceOperation)
        {
            var user = userContext.GetCurrentUser();
            if (user == null)
            {
                logger.LogInformation("User is not authenticated");
                return false;
            }

            if (resourceOperation != ResourceOperation.ReadAll)
            {
                logger.LogInformation("['Notes'] User {UserEmail} is trying to {ResourceOperation} note : {NoteTitle}",
                    user.Email, resourceOperation, note.Title);
            }
            else
            {
                logger.LogInformation("['Notes'] User {UserEmail} is trying to {ResourceOperation} notes",
                    user.Email, resourceOperation);
            }

            if (resourceOperation == ResourceOperation.Create || resourceOperation == ResourceOperation.ReadAll)
            {
                logger.LogInformation("['Notes'] Create / read all operation is successful authorized");
                return true;
            }

            if ((resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update || resourceOperation == ResourceOperation.Read) && user.Id == note.UserId)
            {
                logger.LogInformation("['Notes'] Read / delete / update operation is successful authorized");
                return true;
            }

            return false;
        }

        public bool AuthorizeFlashcard(FlashcardCategory flashcardCategory, ResourceOperation resourceOperation)
        {
            var user = userContext.GetCurrentUser();
            if (user == null)
            {
                logger.LogInformation("User is not authenticated");
                return false;
            }

            if (resourceOperation != ResourceOperation.ReadAll)
            {
                logger.LogInformation("['Flashcards'] User {UserEmail} is trying to {ResourceOperation} category / flashcard / flashcard box where category : {FlashcardCategory}",
                    user.Email, resourceOperation, flashcardCategory.Name);
            }
            else
            {
                logger.LogInformation("['Flashcards'] User {UserEmail} is trying to {ResourceOperation} categories",
                    user.Email, resourceOperation);
            }

            if (resourceOperation == ResourceOperation.Create || resourceOperation == ResourceOperation.ReadAll)
            {
                logger.LogInformation("['Flashcards'] Create / read all operation is successful authorized");
                return true;
            }

            if ((resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update || resourceOperation == ResourceOperation.Read) && user.Id == flashcardCategory.UserId)
            {
                logger.LogInformation("['Flashcards'] Read / delete / update operation is successful authorized");
                return true;
            }

            return false;
        }
    }
}
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

            logger.LogInformation("['Connections' functionality] User {UserEmail} is trying to {ResourceOperation} topic / attribute",
                               user.Email, resourceOperation);

            if (resourceOperation == ResourceOperation.Create)
            {
                logger.LogInformation("Create operation is successful authorized");
                return true;
            }

            if ((resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update || resourceOperation == ResourceOperation.Read) && user.Id == connectionTopic.UserId)
            {
                logger.LogInformation("Read / delete / update operation is successful authorized");
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

            logger.LogInformation("['Notes' functionality] User {UserEmail} is trying to {ResourceOperation} topic / attribute",
                               user.Email, resourceOperation);

            if (resourceOperation == ResourceOperation.Create)
            {
                logger.LogInformation("Create operation is successful authorized");
                return true;
            }

            if ((resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update || resourceOperation == ResourceOperation.Read) && user.Id == note.UserId)
            {
                logger.LogInformation("Read / delete / update operation is successful authorized");
                return true;
            }

            return false;
        }
    }
}
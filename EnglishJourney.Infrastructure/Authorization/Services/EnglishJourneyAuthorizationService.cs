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

            logger.LogInformation("User {UserEmail} is trying to {ResourceOperation} topic {ConnectionTopic}",
                               user.Email, resourceOperation, connectionTopic.Topic);

            if (resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create)
            {
                logger.LogInformation("Create / read operation is successful authorized");
                return true;
            }

            if ((resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update) && user.Id == connectionTopic.UserId)
            {
                logger.LogInformation("Delete / update operation is successful authorized");
                return true;
            }

            return false;
        }
    }
}
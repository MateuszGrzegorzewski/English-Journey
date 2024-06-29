using EnglishJourney.Domain.Exceptions;
using Xunit;

namespace EnglishJourney.DomainTests.Exceptions
{
    public class NotFoundExceptionTests
    {
        [Fact]
        public void NotFoundException_ShouldSetMessageCorrectly()
        {
            // arrange
            var resourceType = "Topic";
            var resourceIdentifier = "1";
            var expectedMessage = $"{resourceType} with id: {resourceIdentifier} doesn't exist.";

            // act
            var exception = new NotFoundException(resourceType, resourceIdentifier);

            // assert
            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}
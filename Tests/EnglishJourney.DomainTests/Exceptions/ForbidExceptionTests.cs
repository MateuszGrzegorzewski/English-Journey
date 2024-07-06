using EnglishJourney.Domain.Exceptions;
using Xunit;

namespace EnglishJourney.DomainTests.Exceptions
{
    public class ForbidExceptionTests
    {
        [Fact]
        public void NotFoundException_ShouldSetMessageCorrectly()
        {
            // act
            var exception = new ForbidException();
            var expectedMessage = "Exception of type 'EnglishJourney.Domain.Exceptions.ForbidException' was thrown.";

            // assert
            Assert.Equal(expectedMessage, exception.Message);
        }
    }
}
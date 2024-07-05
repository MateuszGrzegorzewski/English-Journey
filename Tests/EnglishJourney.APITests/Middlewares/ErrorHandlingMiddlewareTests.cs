﻿using EnglishJourney.API.Middlewares;
using EnglishJourney.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;
using EnglishJourney.Domain.Entities;

namespace EnglishJourney.APITests.Middlewares
{
    public class ErrorHandlingMiddlewareTests
    {
        [Fact()]
        public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
        {
            // arrange
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var context = new DefaultHttpContext();
            var nextDelegateMock = new Mock<RequestDelegate>();

            // act
            await middleware.InvokeAsync(context, nextDelegateMock.Object);

            // assert
            nextDelegateMock.Verify(next => next.Invoke(context), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode404()
        {
            // arrange
            var context = new DefaultHttpContext();
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var notFoundException = new NotFoundException(nameof(ConnectionTopic), "1");

            // act
            await middleware.InvokeAsync(context, _ => throw notFoundException);

            // assert
            context.Response.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task InvokeAsync_WhenGenericExceptionThrown_ShouldSetStatusCode500()
        {
            // arrange
            var context = new DefaultHttpContext();
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var middleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var exception = new Exception();

            // act
            await middleware.InvokeAsync(context, _ => throw exception);

            // assert
            context.Response.StatusCode.Should().Be(500);
        }
    }
}
using Moq;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Application.UseCases.Handlers;
using TicketingSystem.Application.UseCases.Queries;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Tests.UseCases
{
    public class GetEventsHandlerTests
    {
        private readonly Mock<IEventRepository> _eventRepoMock;
        private readonly GetEventsHandler _handler;

        public GetEventsHandlerTests()
        {
            _eventRepoMock = new Mock<IEventRepository>();
            _handler = new GetEventsHandler(_eventRepoMock.Object);
        }

        [Fact]
        public async Task HandleAsync_WhenEventsExist_ReturnsEventList()
        {
            // Arrange
            var events = new List<Event>
            {
                new Event("Concierto Rock", System.DateTime.Now, "Estadio"),
                new Event("Obra de Teatro", System.DateTime.Now, "Teatro")
            };
            _eventRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(events);

            // Act
            var result = await _handler.HandleAsync(new GetEventsQuery());

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task HandleAsync_WhenNoEventsExist_ReturnsEmptyList()
        {
            // Arrange
            _eventRepoMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Event>());
            // Act
            var result = await _handler.HandleAsync(new GetEventsQuery());
            // Assert
            result.Should().NotBeNull().And.BeEmpty();
        }
    }
}
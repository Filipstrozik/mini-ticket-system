namespace MiniTicketSystem.Test;

using Xunit;
using AutoFixture;
using FluentAssertions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MiniTicketSystem.DTO;
using MiniTicketSystem.Entities;
using MiniTicketSystem.Services;
using Moq;

public class TicketServiceTests : IDisposable
{

    private readonly TicketContext _context;
    private readonly Mock<IMapper> _mockMapper;
    private readonly TicketService _ticketService;
    private readonly Fixture _fixture;


    public TicketServiceTests()
    {
        _fixture = new Fixture();

        var options = new DbContextOptionsBuilder<TicketContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TicketContext(options);

        _mockMapper = new Mock<IMapper>();

        _ticketService = new TicketService(_context, _mockMapper.Object);

        SeedTestStatuses();
    }

    private void SeedTestStatuses()
    {
        var statuses = new[]
        {
            new Status { Id = Guid.NewGuid(), Name = "Open" },
            new Status { Id = Guid.NewGuid(), Name = "In Progress" },
            new Status { Id = Guid.NewGuid(), Name = "Closed" }
        };

        _context.Statuses.AddRange(statuses);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetAllTickets_WithTicketsInDatabase_ReturnsAllTicketsWithStatus()
    {
        // Arrange
        var status1 = _context.Statuses.First(s => s.Name == "Open");
        var status2 = _context.Statuses.First(s => s.Name == "Closed");

        var tickets = new List<Ticket>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Test Ticket 1",
                Description = "Description 1",
                StatusId = status1.Id
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Test Ticket 2",
                Description = "Description 2",
                StatusId = status2.Id
            }
        };

        var ticketDtos = new List<TicketReadDto>
        {
            new() { Id = tickets[0].Id, Title = "Test Ticket 1", Description = "Description 1", StatusName = "Open" },
            new() { Id = tickets[1].Id, Title = "Test Ticket 2", Description = "Description 2", StatusName = "Closed" }
        };

        _context.Tickets.AddRange(tickets);
        await _context.SaveChangesAsync();

        _mockMapper.Setup(m => m.Map<IEnumerable<TicketReadDto>>(It.IsAny<List<Ticket>>()))
                  .Returns(ticketDtos);

        // Act
        var result = await _ticketService.GetAllTickets();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(ticketDtos);

        _mockMapper.Verify(m => m.Map<IEnumerable<TicketReadDto>>(It.IsAny<List<Ticket>>()), Times.Once);
    }

    [Fact]
    public async Task GetAllTickets_WithoutTicketsInDatabase_ReturnsEmptyList()
    {
        // Arrange
        _mockMapper.Setup(m => m.Map<IEnumerable<TicketReadDto>>(It.IsAny<List<Ticket>>()))
                  .Returns(new List<TicketReadDto>());

        // Act
        var result = await _ticketService.GetAllTickets();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _mockMapper.Verify(m => m.Map<IEnumerable<TicketReadDto>>(It.IsAny<List<Ticket>>()), Times.Once);
    }

    [Fact]
    public async Task CreateTicket_WithValidDto_CreatesTicketAndReturnsDto()
    {
        // Arrange
        var status = _context.Statuses.First(s => s.Name == "Open");
        var ticketCreateDto = new TicketCreateDto
        {
            Title = "New Ticket",
            Description = "Ticket Description",
            StatusId = status.Id
        };

        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            Title = ticketCreateDto.Title,
            Description = ticketCreateDto.Description,
            StatusId = ticketCreateDto.StatusId,
            Status = status
        };

        var ticketReadDto = new TicketReadDto
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            StatusName = status.Name
        };

        _mockMapper.Setup(m => m.Map<Ticket>(ticketCreateDto)).Returns(ticket);
        _mockMapper.Setup(m => m.Map<TicketReadDto>(ticket)).Returns(ticketReadDto);

        // Act
        var result = await _ticketService.CreateTicket(ticketCreateDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(ticketReadDto);

        // Verify that the ticket was added to the context
        var addedTicket = await _context.Tickets.FindAsync(ticket.Id);
        addedTicket.Should().NotBeNull();
        addedTicket.Title.Should().Be(ticket.Title);
        addedTicket.Description.Should().Be(ticket.Description);
        addedTicket.StatusId.Should().Be(ticket.StatusId);
    }

    [Fact]
    public async Task CreateTicket_WithInvalidStatusId_ThrowsArgumentException()
    {
        // Arrange
        var ticketCreateDto = new TicketCreateDto
        {
            Title = "New Ticket",
            Description = "Ticket Description",
            StatusId = Guid.NewGuid() // Invalid status ID
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _ticketService.CreateTicket(ticketCreateDto));

        exception.Message.Should().Contain("Invalid status ID");

        // Verify that no ticket was added to the context
        var tickets = await _context.Tickets.ToListAsync();
        tickets.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateTicket_ExistingTicket_UpdatesTicketAndReturnsDto()
    {
        // Arrange
        var status = _context.Statuses.First(s => s.Name == "Open");
        var existingTicket = new Ticket
        {
            Id = Guid.NewGuid(),
            Title = "Existing Ticket",
            Description = "Existing Description",
            StatusId = status.Id
        };

        _context.Tickets.Add(existingTicket);
        await _context.SaveChangesAsync();

        var ticketUpdateDto = new TicketUpdateDto
        {
            Id = existingTicket.Id,
            Title = "Updated Ticket",
            Description = "Updated Description",
            StatusId = status.Id
        };

        var updatedTicket = new Ticket
        {
            Id = existingTicket.Id,
            Title = ticketUpdateDto.Title,
            Description = ticketUpdateDto.Description,
            StatusId = ticketUpdateDto.StatusId,
            Status = status
        };

        var ticketReadDto = new TicketReadDto
        {
            Id = updatedTicket.Id,
            Title = updatedTicket.Title,
            Description = updatedTicket.Description,
            StatusName = status.Name
        };

        // Setup mapper for in-place mapping (void return)
        _mockMapper.Setup(m => m.Map(ticketUpdateDto, It.IsAny<Ticket>()))
                  .Callback<TicketUpdateDto, Ticket>((dto, ticket) =>
                  {
                      ticket.Title = dto.Title;
                      ticket.Description = dto.Description;
                      ticket.StatusId = dto.StatusId;
                  });

        // Setup mapper for final DTO conversion
        _mockMapper.Setup(m => m.Map<TicketReadDto>(It.IsAny<Ticket>()))
                  .Returns(ticketReadDto);

        // Act
        var result = await _ticketService.UpdateTicket(existingTicket.Id, ticketUpdateDto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(ticketReadDto);

        // Verify that the ticket was updated in the context
        var updatedEntity = await _context.Tickets.FindAsync(existingTicket.Id);
        updatedEntity.Should().NotBeNull();
        updatedEntity.Title.Should().Be(updatedTicket.Title);
        updatedEntity.Description.Should().Be(updatedTicket.Description);
    }


    public void Dispose()
    {
        _context.Dispose();
    }

}
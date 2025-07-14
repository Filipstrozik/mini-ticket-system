using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using MiniTicketSystem.DTO;
using MiniTicketSystem.Entities;

namespace MiniTicketSystem.Services;


/// <summary>
/// Service interface for managing tickets.
/// </summary>
public interface ITicketService
{
    /// <summary>
    /// Retrieves all tickets.
    /// </summary>
    /// <returns>A collection of ticket DTOs.</returns>
    public Task<IEnumerable<TicketReadDto>> GetAllTickets();

    /// <summary>
    /// Creates a new ticket.
    /// </summary>
    /// <param name="ticket">The ticket creation DTO.</param>
    /// <returns>The created ticket DTO.</returns>
    public Task<TicketReadDto> CreateTicket(TicketCreateDto ticket);

    /// <summary>
    /// Updates an existing ticket.
    /// </summary>
    /// <param name="id">The ID of the ticket to update.</param>
    /// <param name="ticket">The ticket update DTO.</param>
    /// <returns>The updated ticket DTO.</returns>
    public Task<TicketReadDto> UpdateTicket(Guid id, TicketUpdateDto ticket);


    /// <summary>
    /// Deletes a ticket by its ID.
    /// </summary>
    /// <param name="id">The ID of the ticket to delete.</param>
    /// <returns>A boolean indicating whether the deletion was successful.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the ticket with the specified ID
    public Task<bool> DeleteTicket(Guid id);
}

/// <summary>
/// Service implementation for managing tickets.
/// </summary>
public class TicketService : ITicketService
{
    private readonly TicketContext _context;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="TicketService"/> class.
    /// </summary>
    /// <param name="context">The ticket database context.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public TicketService(TicketContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TicketReadDto>> GetAllTickets()
    {
        var tickets = await _context.Tickets.Include(t => t.Status).ToListAsync();
        return _mapper.Map<IEnumerable<TicketReadDto>>(tickets);
    }

    /// <inheritdoc/>
    public async Task<TicketReadDto> CreateTicket(TicketCreateDto ticketDto)
    {
        var status = await _context.Statuses.FindAsync(ticketDto.StatusId)
                     ?? throw new ArgumentException("Invalid status ID");

        var newTicket = _mapper.Map<Ticket>(ticketDto);
        newTicket.Status = status;

        _context.Tickets.Add(newTicket);
        await _context.SaveChangesAsync();
        return _mapper.Map<TicketReadDto>(newTicket);
    }

    /// <inheritdoc/>
    public async Task<TicketReadDto> UpdateTicket(Guid id, TicketUpdateDto ticketDto)
    {
        var existingTicket = await _context.Tickets.FindAsync(id)
                     ?? throw new KeyNotFoundException("Ticket not found");

        var status = await _context.Statuses.FindAsync(ticketDto.StatusId)
                     ?? throw new ArgumentException("Invalid status ID");

        _mapper.Map(ticketDto, existingTicket);
        existingTicket.Status = status;

        _context.Tickets.Update(existingTicket);
        await _context.SaveChangesAsync();

        return _mapper.Map<TicketReadDto>(existingTicket);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteTicket(Guid id)
    {
        var existingTicket = await _context.Tickets.FindAsync(id)
                     ?? throw new KeyNotFoundException("Ticket not found");

        var result = _context.Tickets.Remove(existingTicket);
        await _context.SaveChangesAsync();
        return result != null;
    }
}
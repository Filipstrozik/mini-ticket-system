
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MiniTicketSystem.DTO;
using MiniTicketSystem.Services;

namespace MiniTicketSystem.Controllers;

/// <summary>
/// API controller for managing tickets.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TicketsController : ControllerBase
{
    private readonly TicketService _ticketService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TicketsController"/> class.
    /// </summary>
    /// <param name="ticketService">The ticket service instance.</param>
    public TicketsController(TicketService ticketService)
    {
        _ticketService = ticketService ?? throw new ArgumentNullException(nameof(ticketService));
    }

    /// <summary>
    /// Retrieves all tickets.
    /// </summary>
    /// <returns>A list of ticket DTOs.</returns>
    [HttpGet]
    public async Task<IActionResult> GetTickets()
    {
        IEnumerable<TicketReadDto> tickets = await _ticketService.GetAllTickets();
        return Ok(tickets);
    }

    /// <summary>
    /// Creates a new ticket.
    /// </summary>
    /// <param name="ticketDto">The ticket creation DTO.</param>
    /// <returns>The created ticket DTO.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromBody] TicketCreateDto ticketDto)
    {
        TicketReadDto createdTicket = await _ticketService.CreateTicket(ticketDto);
        return Created(string.Empty, createdTicket);
    }

    /// <summary>
    /// Updates an existing ticket.
    /// </summary>
    /// <param name="id">The ID of the ticket to update.</param>
    /// <param name="ticketDto">The ticket update DTO.</param>
    /// <returns>The updated ticket DTO.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicket(Guid id, [FromBody] TicketUpdateDto ticketDto)
    {
        TicketReadDto updatedTicket = await _ticketService.UpdateTicket(id, ticketDto);
        return Ok(updatedTicket);
    }

    /// <summary>
    /// Deletes a ticket by its ID.
    /// </summary>
    /// <param name="id">The ID of the ticket to delete.</param>
    /// <returns>A 204 No Content response if successful, or a 404 Not Found

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicket(Guid id)
    {
        var result = await _ticketService.DeleteTicket(id);
        if (result)
        {
            return NoContent();
        }
        return NotFound();

    }
}

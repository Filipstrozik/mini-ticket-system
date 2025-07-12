
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MiniTicketSystem.DTO;
using MiniTicketSystem.Services;

namespace MiniTicketSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TicketsController : ControllerBase
{
    private readonly TicketService _ticketService;

    public TicketsController(TicketService ticketService)
    {
        _ticketService = ticketService ?? throw new ArgumentNullException(nameof(ticketService));
    }

    // required endpoints 

    [HttpGet]
    public async Task<IActionResult> GetTickets()
    {
        var tickets = await _ticketService.GetAllTickets();
        return Ok(tickets);
    }


    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromBody] TicketCreateDto ticketDto)
    {
        var createdTicket = await _ticketService.CreateTicket(ticketDto);
        return Ok(createdTicket);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicket(Guid id, [FromBody] TicketUpdateDto ticketDto)
    {
        var updatedTicket = await _ticketService.UpdateTicket(id, ticketDto);
        return Ok(updatedTicket);
    }

}

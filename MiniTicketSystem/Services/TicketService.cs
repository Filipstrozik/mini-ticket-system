using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniTicketSystem.DTO;
using MiniTicketSystem.Entities;

namespace MiniTicketSystem.Services;

public interface ITicketService
{

    public Task<IEnumerable<TicketReadDto>> GetAllTickets();
    public Task<TicketReadDto> CreateTicket(TicketCreateDto ticket);
    public Task<TicketReadDto?> UpdateTicket(Guid id, TicketUpdateDto ticket);
}

public class TicketService : ITicketService
{

    private readonly TicketContext _context;
    private readonly IMapper _mapper;

    public TicketService(TicketContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<TicketReadDto>> GetAllTickets()
    {
        var tickets = await _context.Tickets.Include(t => t.Status).ToListAsync();
        return _mapper.Map<IEnumerable<TicketReadDto>>(tickets);
    }

    public async Task<TicketReadDto> CreateTicket(TicketCreateDto ticketDto)
    {
        var status = await _context.Statuses.FindAsync(ticketDto.StatusId)
                     ?? throw new ArgumentException("Invalid status ID");


        var newTicket = _mapper.Map<Ticket>(ticketDto);
        newTicket.Status = status;

        // manualy but not recommended
        // var newTicket = new Ticket
        // {
        //     Title = ticket.Title,
        //     Description = ticket.Description,
        //     StatusId = ticket.StatusId,
        //     Status = status,
        // };

        _context.Tickets.Add(newTicket);
        await _context.SaveChangesAsync();
        return _mapper.Map<TicketReadDto>(newTicket);
    }

    public async Task<TicketReadDto?> UpdateTicket(Guid id, TicketUpdateDto ticketDto)
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
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MiniTicketSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatusesController : ControllerBase
{
    private readonly TicketContext _context;

    public StatusesController(TicketContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet]
    public async Task<IActionResult> GetStatuses()
    {
        var statuses = await _context.Statuses.ToListAsync();
        return Ok(statuses);
    }

}
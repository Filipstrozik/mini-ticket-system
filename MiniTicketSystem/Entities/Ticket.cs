using MiniTicketSystem.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniTicketSystem.Entities;

public class Ticket
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Description { get; set; }

    public required Guid StatusId { get; set; }

    public Status Status { get; set; } = null!;
}
using System.ComponentModel.DataAnnotations;

namespace MiniTicketSystem.Entities;

public class Status
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public required string Name { get; set; }
}

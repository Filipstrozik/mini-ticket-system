using System.ComponentModel.DataAnnotations;

namespace MiniTicketSystem.DTO;

public class TicketUpdateDto
{
    [Required]
    public Guid Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Title { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public Guid StatusId { get; set; }
}


public class TicketCreateDto
{
    [Required(AllowEmptyStrings = false)]
    public required string Title { get; set; }

    [Required(AllowEmptyStrings = false)]
    public required string Description { get; set; }

    [Required]
    public Guid StatusId { get; set; }
}


public class TicketReadDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";

    public Guid StatusId { get; set; }
    public string StatusName { get; set; } = "";
}
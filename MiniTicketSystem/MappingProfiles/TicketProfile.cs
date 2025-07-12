using AutoMapper;
using MiniTicketSystem.DTO;
using MiniTicketSystem.Entities;

namespace MiniTicketSystem.MappingProfiles;

public class TicketProfile : Profile
{
    public TicketProfile()
    {
        // DTO -> Entity
        CreateMap<TicketCreateDto, Ticket>();
        CreateMap<TicketUpdateDto, Ticket>();

        // Entity -> DTO
        CreateMap<Ticket, TicketReadDto>()
            .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.Name));
    }
}
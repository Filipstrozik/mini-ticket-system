using Microsoft.EntityFrameworkCore;
using MiniTicketSystem.Entities;

public class TicketContext : DbContext
{
    public TicketContext(DbContextOptions<TicketContext> options) : base(options) { }

    public DbSet<Ticket> Tickets { get; set; }

    public DbSet<Status> Statuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Status)
            .WithMany()
            .HasForeignKey(t => t.StatusId)
            .IsRequired()
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Status>().HasData(
            new Status { Id = new Guid("11111111-1111-1111-1111-111111111111"), Name = "Open" },
            new Status { Id = new Guid("22222222-2222-2222-2222-222222222222"), Name = "In Progress" },
            new Status { Id = new Guid("33333333-3333-3333-3333-333333333333"), Name = "Resolved" }
        );

        base.OnModelCreating(modelBuilder);
    }
}
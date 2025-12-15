using MediatR;
using Microsoft.EntityFrameworkCore;
using Triumph.SMS.Remote.Core.Common.Entities;
using Triumph.SMS.Remote.Core.Students;

namespace Triump.SMS.Remote.Persistence.Data;

public class ApplicationDbContext : DbContext
{
    private readonly IPublisher _publisher;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPublisher publisher)
        : base(options)
    {
        _publisher = publisher;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries()
            .Select(e => e.Entity)
            .OfType<EntityBase<int>>()
            .SelectMany(e =>
            {
                var events = e.DomainEvents.ToList() ?? [];
                e.ClearDomainEvents();
                return events;
            })
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        if (domainEvents.Count > 0)
        {
            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent, cancellationToken);
            }
        }

        return result;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // put sqlite file at the root of the drive C in a folder TriumphSMS
        const string connectionString = "Data Source=C:\\TriumphSMS\\triumph_sms_remote.db";
        optionsBuilder.UseSqlite(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public DbSet<Student> Students => Set<Student>();
}

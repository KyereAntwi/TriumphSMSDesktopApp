using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Triumph.SMS.Remote.Core.ApplicationUsers;
using Triumph.SMS.Remote.Core.Common.Entities;
using Triumph.SMS.Remote.Core.Payments;
using Triumph.SMS.Remote.Core.Students;

namespace Triumph.SMS.Remote.Persistence.Data;

public class ApplicationDbContext : DbContext
{
    private readonly IPublisher _publisher;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPublisher publisher)
        : base(options)
    {
        _publisher = publisher;

        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
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

        if (domainEvents.Count <= 0) return result;
        
        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }

        return result;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // put sqlite file at the root of the drive C in a folder TriumphSMS
        const string connectionString = @"Data Source=C:\TriumphSMS\triumph_sms_remote.db";
        optionsBuilder.UseSqlite(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Global query filter for soft delete - Use .IgnoreQueryFilters() to bypass
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            if(clrType == null) continue;

            if (!typeof(EntityBase<int>).IsAssignableFrom(clrType)) continue;

            var parameter = Expression.Parameter(clrType, "e");
            var property = Expression.Property(parameter, nameof(EntityBase<int>.IsDeleted));
            var isNotDeleted = Expression.Equal(property, Expression.Constant(false));
            var lambda = Expression.Lambda(isNotDeleted, parameter);

            modelBuilder.Entity(clrType).HasQueryFilter(lambda);
        }
    }

    #region Student Entities
    public DbSet<Student> Students => Set<Student>();
    public DbSet<ParentPhone> ParentPhones => Set<ParentPhone>();
    public DbSet<PaymentType> PaymentTypes => Set<PaymentType>();
    public DbSet<PaymentHistory> PaymentHistories => Set<PaymentHistory>();
    #endregion

    #region Staff Entities
    public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
    public DbSet<PrimaryPhone> PrimaryPhones => Set<PrimaryPhone>();
    public DbSet<Activity> Activities => Set<Activity>();
    #endregion
}

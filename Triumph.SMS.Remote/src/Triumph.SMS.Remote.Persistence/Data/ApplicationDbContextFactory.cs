using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Triumph.SMS.Remote.Persistence.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        var dbPath = Path.Combine(AppContext.BaseDirectory, "triumph_sms_remote.db");
        var connectionString = $"Data Source={dbPath}";
        optionsBuilder.UseSqlite(connectionString);

        // Create a no-op publisher for design-time
        var publisher = new NoOpPublisher();

        return new ApplicationDbContext(optionsBuilder.Options, publisher);
    }

    private class NoOpPublisher : IPublisher
    {
        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) 
            where TNotification : INotification
        {
            return Task.CompletedTask;
        }
    }
}

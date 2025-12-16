using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Triumph.SMS.Remote.Persistence.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        const string connectionString = @"Data Source=C:\TriumphSMS\triumph_sms_remote.db";
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

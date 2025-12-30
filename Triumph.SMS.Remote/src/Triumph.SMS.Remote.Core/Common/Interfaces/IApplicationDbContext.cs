using Microsoft.EntityFrameworkCore;
using Triumph.SMS.Remote.Core.ApplicationUsers;
using Triumph.SMS.Remote.Core.Payments;
using Triumph.SMS.Remote.Core.SchoolConfigs;
using Triumph.SMS.Remote.Core.Students;

namespace Triumph.SMS.Remote.Core.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<ApplicationUser> ApplicationUsers { get; }
    public DbSet<Student> Students { get; }
    public DbSet<PaymentHistory> PaymentHistories { get; }
    public DbSet<PaymentType> PaymentTypes { get; }
    public DbSet<SchoolInformation> SchoolInformation { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
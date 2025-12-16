using Microsoft.EntityFrameworkCore;
using Triumph.SMS.Remote.Core.ApplicationUsers;

namespace Triumph.SMS.Remote.Core.Common.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<ApplicationUser> ApplicationUsers { get; }
}
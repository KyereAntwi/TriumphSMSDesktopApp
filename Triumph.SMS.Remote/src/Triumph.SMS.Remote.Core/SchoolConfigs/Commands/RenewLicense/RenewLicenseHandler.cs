using MediatR;
using Microsoft.EntityFrameworkCore;
using Triumph.SMS.Remote.Core.ApplicationUsers.Enums;
using Triumph.SMS.Remote.Core.Common.CQRS;
using Triumph.SMS.Remote.Core.Common.Extensions;
using Triumph.SMS.Remote.Core.Common.Interfaces;

namespace Triumph.SMS.Remote.Core.SchoolConfigs.Commands.RenewLicense;

public record RenewLicenseCommandResult
{
    public IEnumerable<string> Errors { get; set; } = [];
}

public record RenewLicenseCommand(LicenseTpye Type): ICommand<RenewLicenseCommandResult>;

public sealed class RenewLicenseHandler(IApplicationDbContext dbContext) : IRequestHandler<RenewLicenseCommand, RenewLicenseCommandResult>
{
    public async Task<RenewLicenseCommandResult> Handle(RenewLicenseCommand request, CancellationToken cancellationToken)
    {
        return await ((Func<Task<RenewLicenseCommandResult>>)(() => HandleAsync(request, cancellationToken)))
            .HandleWithErrorHandlingAsync(ex => new RenewLicenseCommandResult
            {
                Errors = [$"An error occured. Error: {ex.Message}"]
            });
    }

    private async Task<RenewLicenseCommandResult> HandleAsync(RenewLicenseCommand command, CancellationToken cancellationToken)
    {
        var schoolConfig = await dbContext
            .SchoolInformation
            .Include(s => s.Lisences)
            .AsSplitQuery()
            .AsTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (schoolConfig is null) 
        { 
            return new RenewLicenseCommandResult
            {
                Errors = ["School configuration not found."]
            };
        }

        Lisence newLicense;
        if (command.Type == LicenseTpye.Free)
        {
            newLicense = Lisence.Create(
                lisenceKey: Guid.NewGuid().ToString(),
                schoolConfig.Id,
                issuedOn: DateTime.UtcNow,
                expiresOn: DateTime.UtcNow.AddMonths(24)
            );
        } 
        else if (command.Type == LicenseTpye.Basic)
        {
            newLicense = Lisence.Create(
                lisenceKey: Guid.NewGuid().ToString(),
                schoolConfig.Id,
                issuedOn: DateTime.UtcNow,
                expiresOn: DateTime.UtcNow.AddMonths(1)
            );
        }
        else if (command.Type == LicenseTpye.Premium)
        {
            newLicense = Lisence.Create(
                lisenceKey: Guid.NewGuid().ToString(),
                schoolConfig.Id,
                issuedOn: DateTime.UtcNow,
                expiresOn: DateTime.UtcNow.AddMonths(12)
            );
        }
        else
        {             
            return new RenewLicenseCommandResult
            {
                Errors = ["Invalid license type."]
            };
        }

        schoolConfig.RenewLisence(newLicense);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new RenewLicenseCommandResult();
    }
}
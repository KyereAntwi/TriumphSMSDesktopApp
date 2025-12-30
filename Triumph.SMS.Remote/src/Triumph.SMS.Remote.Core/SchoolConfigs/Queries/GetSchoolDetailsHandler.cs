using MediatR;
using Microsoft.EntityFrameworkCore;
using Triumph.SMS.Remote.Core.Common.CQRS;
using Triumph.SMS.Remote.Core.Common.Extensions;
using Triumph.SMS.Remote.Core.Common.Interfaces;

namespace Triumph.SMS.Remote.Core.SchoolConfigs.Queries;

public record GetSchoolDetailsQueryResult
{
    public string Name { get; set; } = string.Empty;
    public string? Logo { get; set; }
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string Motto { get; set; } = string.Empty;
    public bool HasActiveLicense { get; set; }
    public IEnumerable<string> Errors { get; set; } = [];
}

public record GetSchoolDetailsQuery: IQuery<GetSchoolDetailsQueryResult>;

public sealed class GetSchoolDetailsHandler(IApplicationDbContext dbContext) : IRequestHandler<GetSchoolDetailsQuery, GetSchoolDetailsQueryResult>
{
    public async Task<GetSchoolDetailsQueryResult> Handle(GetSchoolDetailsQuery request, CancellationToken cancellationToken)
    {
        return await ((Func<Task<GetSchoolDetailsQueryResult>>)(() => HandleAsync(request, cancellationToken)))
            .HandleWithErrorHandlingAsync(ex => new GetSchoolDetailsQueryResult
            {
                Errors = [$"An error occured. Error: {ex.Message}"]
            });
    }
    public async Task<GetSchoolDetailsQueryResult> HandleAsync(GetSchoolDetailsQuery query, CancellationToken cancellationToken)
    {
        var schoolConfig = await dbContext
            .SchoolInformation
            .Include(s => s.Lisences)
            .AsSplitQuery()
            .FirstOrDefaultAsync(cancellationToken);

        if (schoolConfig is null)
        {
            return new GetSchoolDetailsQueryResult
            {
                Errors = ["School configuration not found."]
            };
        }

        return new GetSchoolDetailsQueryResult
        {
            Name = schoolConfig.Name,
            Logo = schoolConfig.Logo,
            Address = schoolConfig.Address,
            PhoneNumber = schoolConfig.PhoneNumber,
            Email = schoolConfig.Email,
            Website = schoolConfig.Website,
            Motto = schoolConfig.Motto,
            HasActiveLicense = schoolConfig.HasActiveLisence()
        };
    }
}

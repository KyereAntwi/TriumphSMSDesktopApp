using MediatR;
using Triumph.SMS.Remote.Core.ApplicationUsers.Queries;

namespace Triumph.SMS.Remote.App.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly ISender _sender;
    private readonly GetApplicationUserDetailsResult SelectedUser;
    private bool IsFirstTime = false;
    private IEnumerable<string> Errors = [];

    public Command LoginCommand { get; }
    public Command GetLogedInUserCommand { get; }
    public Command CheckIfFirstTime { get; }


    public LoginViewModel(ISender sender) : base(sender)
    {
        Title = "Login";
        CheckIfFirstTime = new Command(async () => await CheckIfFirstTimeAsync());
    }

    public async Task CheckIfFirstTimeAsync()
    {
        IsBusy = true;

        var result = await _sender.Send(new GetAllApplicationUsersQuery(1, 1));

        Errors = [.. result.Errors];

        if (!result.Users.Any())
        {
            IsFirstTime = true;
        }

        IsBusy = false;
    }
}

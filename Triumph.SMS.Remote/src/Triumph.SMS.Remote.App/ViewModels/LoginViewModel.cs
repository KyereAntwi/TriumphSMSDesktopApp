using Triumph.SMS.Remote.App.Services;

namespace Triumph.SMS.Remote.App.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly ISender _sender;
    private readonly INavigationService _navigation;

    [ObservableProperty]
    private string _userName = default!;
    [ObservableProperty]
    private string _password = default!;
    [ObservableProperty]
    private bool _isBusy = true;
    [ObservableProperty]
    private string _title = default!;
    [ObservableProperty]
    private bool _isFirstTime = false;
    [ObservableProperty]
    private ObservableCollection<string> _errors = [];
    [ObservableProperty]
    private ObservableCollection<Role> _roles = [];
    [ObservableProperty]
    private Role _selectedRole = default!;
    [ObservableProperty]
    private string _buttonText = "Login";

    public LoginViewModel(ISender sender, INavigationService navigation)
    {
        Title = "Login";
        _sender = sender;
        _navigation = navigation;

        using var _ = CheckIfFirstUserAsync();
    }

    private async Task CheckIfFirstUserAsync()
    {
        IsBusy = false;

        var result = await _sender.Send(new GetAllApplicationUsersQuery(1, 1));

        if (result.Errors.Any())
        {
            await _navigation.GotoErrroPage(new ObservableCollection<string>(result.Errors));
            return;
        }

        if(!result.Users.Any())
            IsFirstTime = true;

        IsBusy = true;
    }

    private async Task LoginAsync()
    {
        var result = await _sender.Send(new LoginCommand(UserName, Password));

        if (result.Errors!.Any())
        {
            await _navigation.GotoErrroPage(new ObservableCollection<string>(result.Errors!));
            return;
        }
        else
        {
            Roles.Clear();
            Roles = new ObservableCollection<Role>(result.Roles!);
            ButtonText = "Go to Selected Role Portal";
        }
    }

    [RelayCommand]
    private async Task Login()
    {
        IsBusy = false;

        if (ButtonText.Contains("Login"))
        {
            await LoginAsync();
        }
        else
        {
            await _navigation.GotoSelectedPortal(SelectedRole);
        }

        IsBusy = true;
    }

    [RelayCommand]
    private async Task InitializeSchool()
    {
        await _navigation.GotoInitializeSchoolPage();
    }
}

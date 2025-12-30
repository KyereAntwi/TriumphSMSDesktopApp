using Triumph.SMS.Remote.App.Services;
using Triumph.SMS.Remote.Core.ApplicationUsers.Commands.Register;

namespace Triumph.SMS.Remote.App.ViewModels;

public partial class RegisterSchoolAdminViewModel(ISender sender, INavigationService navigation) : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy = false;
    
    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _firstName = string.Empty;

    [ObservableProperty]
    private string _lastName = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _confirmPassword = string.Empty;

    [ObservableProperty]
    private string _otherNames = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private ObservableCollection<Role> _selectedRoles = [Role.Admin, Role.Accountant];

    [ObservableProperty]
    private string _title = "Register School Admin";

    [RelayCommand]
    private async Task RegisterAdmin()
    {
        if (!ConfirmPassword.Equals(Password))
        {
            await navigation.GotoErrroPage(["Confirm password and password should be the same"]);
        }

        IsBusy = true;

        var command = new RegisterCommand(
            Username: Username,
            Password: Password,
            FirstName: FirstName,
            LastName: LastName,
            Email: string.IsNullOrWhiteSpace(Email) ? null : Email,
            OtherNames: string.IsNullOrWhiteSpace(OtherNames) ? null : OtherNames,
            Roles: SelectedRoles,
            null
        );

        var result = await sender.Send(command);

        if (result.Errors!.Any())
        {
            await navigation.GotoErrroPage(new ObservableCollection<string>(result.Errors!));
            IsBusy = false;
        }

        await navigation.GotoLogInPage();

        IsBusy = false;
    }
}

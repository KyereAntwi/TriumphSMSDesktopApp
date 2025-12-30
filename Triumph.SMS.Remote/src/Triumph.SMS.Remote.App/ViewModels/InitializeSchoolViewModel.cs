using Triumph.SMS.Remote.App.Services;
using Triumph.SMS.Remote.Core.SchoolConfigs.Commands.InitializeSchool;

namespace Triumph.SMS.Remote.App.ViewModels;

public partial class InitializeSchoolViewModel : ObservableObject
{
    private readonly ISender _sender;
    private readonly INavigationService _navigation;

    [ObservableProperty]
    private bool _isBusy = true;
    [ObservableProperty]
    private string _title = default!;
    [ObservableProperty]
    private string _schoolName = default!;
    [ObservableProperty]
    private string _logo = default!;
    [ObservableProperty]
    private string _address = default!;
    [ObservableProperty]
    private string _phoneNumber = default!;
    [ObservableProperty]
    private string _email = default!;
    [ObservableProperty]
    private string _website = default!;
    [ObservableProperty]
    private string _motto = default!;

    public InitializeSchoolViewModel(ISender sender, INavigationService navigation)
    {
        Title = "Initialize School";

        _sender = sender;
        _navigation = navigation;
    }

    [RelayCommand]
    private async Task ProcessInitialization()
    {
        IsBusy = false;

        var command = new InitializeSchoolCommand
        {
            Name = SchoolName,
            Logo = Logo,
            Address = Address,
            Phone = PhoneNumber,
            Email = Email,
            Website = Website,
            Motto = Motto
        };

        var result = await _sender.Send(command);

        if (result.Errors.Any())
        {
           await _navigation.GotoErrroPage(new ObservableCollection<string>(result.Errors));
        }
        else
        {
           await _navigation.GotoLicenseRenewalPage();
        }

        IsBusy = true;
    }
}

using Triumph.SMS.Remote.App.Services;
using Triumph.SMS.Remote.Core.SchoolConfigs.Commands.RenewLicense;

namespace Triumph.SMS.Remote.App.ViewModels;

public partial class LicenseRenewalViewModel(ISender sender, INavigationService navigation) : ObservableObject
{
    [ObservableProperty]
    private string _title = "License Renewal";

    [ObservableProperty]
    private ObservableCollection<LicenseTpye> _licenseTypes =
    [
        LicenseTpye.Basic,
        LicenseTpye.Premium,
        LicenseTpye.Free
    ];

    [ObservableProperty]
    private LicenseTpye _selectedLicenseType = LicenseTpye.Basic;

    [ObservableProperty]
    private bool _isBusy = true;

    [ObservableProperty]
    private string _licenseKey = default!;

    [RelayCommand]
    private async Task RenewLicenseProcess() 
    {
        IsBusy = false;

        if (string.IsNullOrWhiteSpace(LicenseKey))
        {
            await navigation.GotoErrroPage(["License Key is required."]);
        }

        if (string.IsNullOrEmpty(LicenseKey) || !LicenseKey.Equals("secretkeyforlicensing"))
        {
            await navigation.GotoErrroPage(["Provided license key is invalid"]);
        }

        var command = new RenewLicenseCommand(SelectedLicenseType);
        var result = await sender.Send(command);

        if (result.Errors.Any())
        {
            await navigation.GotoErrroPage(new ObservableCollection<string>(result.Errors));
            return;
        }

        IsBusy = true;
        await navigation.GotoRegisterSchoolAdminPage();
    }
}

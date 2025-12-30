
using Triumph.SMS.Remote.App.Services;

namespace Triumph.SMS.Remote.App.ViewModels;

public partial class ErrorPageViewModel(INavigationService navigation) : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    private ObservableCollection<string> _errors = [];

    [ObservableProperty]
    private bool _isLicenseError = false;

    [ObservableProperty]
    private string _buttonText = "Go Back";

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        Errors = (ObservableCollection<string>)query["Errors"];
        OnPropertyChanged(nameof(Errors));

        if (Errors.First().Contains("license", StringComparison.CurrentCultureIgnoreCase))
        {
            IsLicenseError = true;
            OnPropertyChanged(nameof(IsLicenseError));

            ButtonText = "Renew License";
            OnPropertyChanged(nameof(ButtonText));
        }
    }

    [RelayCommand]
    private async Task GoBack()
    {
        if (IsLicenseError)
            await navigation.GotoLicenseRenewalPage();
        else
            await navigation.GoBack();
    }
}

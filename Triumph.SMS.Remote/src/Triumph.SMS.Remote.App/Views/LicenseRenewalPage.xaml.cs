using Triumph.SMS.Remote.App.ViewModels;

namespace Triumph.SMS.Remote.App.Views;

public partial class LicenseRenewalPage : ContentPage
{
	public LicenseRenewalPage(LicenseRenewalViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
    }
}
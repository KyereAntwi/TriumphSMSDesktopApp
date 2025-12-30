using Triumph.SMS.Remote.App.ViewModels;

namespace Triumph.SMS.Remote.App.Views;

public partial class ErrorPage : ContentPage
{
	public ErrorPage(ErrorPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

        Shell.SetBackButtonBehavior(this, new BackButtonBehavior { IsEnabled = false });
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }
}
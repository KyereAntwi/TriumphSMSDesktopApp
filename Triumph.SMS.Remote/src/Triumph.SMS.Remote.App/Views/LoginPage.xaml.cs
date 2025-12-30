using Triumph.SMS.Remote.App.Services;
using Triumph.SMS.Remote.App.ViewModels;

namespace Triumph.SMS.Remote.App.Views;

public partial class LoginPage : ContentPage
{

    public LoginPage(LoginViewModel viewModel, INavigationService navigationService)
	{
		InitializeComponent();
        BindingContext = viewModel;

        if(viewModel.Errors.Any())
        {
            navigationService.GotoErrroPage(viewModel.Errors);
        }
    }
}
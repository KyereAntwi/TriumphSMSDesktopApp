using Triumph.SMS.Remote.App.ViewModels;

namespace Triumph.SMS.Remote.App.Views;

public partial class RegisterSchoolAdminPage : ContentPage
{
	public RegisterSchoolAdminPage(RegisterSchoolAdminViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}
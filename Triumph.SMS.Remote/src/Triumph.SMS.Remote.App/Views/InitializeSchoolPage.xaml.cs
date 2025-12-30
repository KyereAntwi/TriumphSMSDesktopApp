using Triumph.SMS.Remote.App.ViewModels;

namespace Triumph.SMS.Remote.App.Views;

public partial class InitializeSchoolPage : ContentPage
{
    public InitializeSchoolPage(InitializeSchoolViewModel vm)
	{
		BindingContext = vm;
        InitializeComponent();
    }
}
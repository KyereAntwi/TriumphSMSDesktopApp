namespace Triumph.SMS.Remote.App.Services;

public interface INavigationService
{
    Task GotoErrroPage(ObservableCollection<string> errors);
    Task GotoSelectedPortal(Role role);
    Task GoBack();
    Task GotoInitializeSchoolPage();
    Task GotoLicenseRenewalPage();
    Task GotoRegisterSchoolAdminPage();
    Task GotoLogInPage();
}

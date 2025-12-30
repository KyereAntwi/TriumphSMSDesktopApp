

namespace Triumph.SMS.Remote.App.Services;

public class NavigationService : INavigationService
{
    public async Task GoBack() => await Shell.Current.GoToAsync("..");

    public async Task GotoErrroPage(ObservableCollection<string> errors) => 
        await Shell.Current.GoToAsync("ErrorPage", true, new Dictionary<string, object>
        {
            { "Errors", errors }
        });

    public async Task GotoInitializeSchoolPage() => await Shell.Current.GoToAsync("InitializeSchoolPage");

    public async Task GotoLicenseRenewalPage() => await Shell.Current.GoToAsync("LicenseRenewalPage");

    public async Task GotoLogInPage() => await Shell.Current.GoToAsync("//LoginPage");

    public async Task GotoRegisterSchoolAdminPage() => await Shell.Current.GoToAsync("RegisterSchoolAdminPage");

    public async Task GotoSelectedPortal(Role role)
    {
        switch(role)
        {
            case Role.Admin:
                await Shell.Current.GoToAsync("AdminPortalPage");
                break;
            case Role.Teacher:
                await Shell.Current.GoToAsync("TeacherPortalPage");
                break;
            case Role.NonTeaching:
                await Shell.Current.GoToAsync("NonTeachingPortalPage");
                break;
            case Role.Accountant:
                await Shell.Current.GoToAsync("AccountantPortalPage");
                break;
            default:
                await Shell.Current.GoToAsync("//LoginPage");
                break;
        }
    }
}

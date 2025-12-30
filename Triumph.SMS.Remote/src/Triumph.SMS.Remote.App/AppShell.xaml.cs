using Triumph.SMS.Remote.App.Views;

namespace Triumph.SMS.Remote.App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("ErrorPage", typeof(ErrorPage));
            Routing.RegisterRoute("InitializeSchoolPage", typeof(InitializeSchoolPage));
            Routing.RegisterRoute("LicenseRenewalPage", typeof(LicenseRenewalPage));
            Routing.RegisterRoute("RegisterSchoolAdminPage", typeof(RegisterSchoolAdminPage));
        }
    }
}

using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using Triumph.SMS.Remote.Core.Common.Behaviors;
using Triumph.SMS.Remote.Core.Common.Interfaces;
using Triumph.SMS.Remote.Persistence.Data;

namespace Triumph.SMS.Remote.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            //Register syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cWWdCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWH1fdnVWRWJZVUx/W0NWYUo=");

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(MauiProgram).Assembly);
                options.AddOpenBehavior(typeof(RetryDecorator<,>));
            });

            builder.Services.AddDbContext<ApplicationDbContext>();
            builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            return builder.Build();
        }
    }
}

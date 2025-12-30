using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using Syncfusion.Maui.Core.Hosting;
using Triumph.SMS.Remote.App.Services;
using Triumph.SMS.Remote.App.ViewModels;
using Triumph.SMS.Remote.Core;
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

            // Add logging for only desktop environments - logging using serilog and for files
            if (DeviceInfo.Platform == DevicePlatform.WinUI || DeviceInfo.Platform == DevicePlatform.MacCatalyst)
            {
                var logsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Triumph.SMS.Remote", "Logs");
                Directory.CreateDirectory(logsDir);

                var logFilePath = Path.Combine(logsDir, "log-.txt");

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .Enrich.FromLogContext()
                    .WriteTo.File(
                        logFilePath,
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: 30,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                    .CreateLogger();
            }

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(Setup).Assembly);

                options.AddOpenBehavior(typeof(PerformanceDecorator<,>));
                //options.AddOpenBehavior(typeof(ValidationDecorator<,>));
                options.AddOpenBehavior(typeof(RetryDecorator<,>));
            });

            builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>();
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<ErrorPageViewModel>();
            builder.Services.AddSingleton<InitializeSchoolViewModel>();
            builder.Services.AddSingleton<LicenseRenewalViewModel>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<RegisterSchoolAdminViewModel>();

            var app =  builder.Build();

            // apply migrations at startup or ensure db is created
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
            }

            return app;
        }
    }
}

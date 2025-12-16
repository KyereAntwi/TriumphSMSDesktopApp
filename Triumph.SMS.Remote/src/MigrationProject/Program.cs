using Triumph.SMS.Remote.Persistence.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddDbContext<ApplicationDbContext>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
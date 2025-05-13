using SupportTicketApi.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

DBABookLibrary.Utils.Dependency.AddReadDatabase(builder);

var host = builder.Build();

await host.RunAsync();
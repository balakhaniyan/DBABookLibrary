using DBABookLibrary.AppHost.OpenTelemetryCollector;

var builder = DistributedApplication
    .CreateBuilder(args);

var readDatabase = builder
    .AddPostgres("postgres", port: 5433)
    .WithDataVolume()
    .WithPgAdmin(config => { config.WithHostPort(8092); })
    .AddDatabase("DBABookLibraryReadDatabase");

var writeDatabase = builder
    .AddMongoDB("mongo", port: 27017)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithMongoExpress(config => { config.WithHostPort(8093); })
    .AddDatabase("DBABookLibraryWriteDatabase");

builder.AddProject<Projects.DBABookLibrary_MigrationService>("migration-service")
    .WithReference(readDatabase)
    .WaitFor(readDatabase);


var messaging = builder
    .AddRabbitMQ("DBABookLibraryMessenger")
    .WithManagementPlugin()
    .WithDataVolume(isReadOnly: false);

var prometheus = builder.AddContainer("prometheus", "prom/prometheus", "v3.2.1")
    .WithBindMount("../prometheus", "/etc/prometheus", isReadOnly: true)
    .WithArgs("--web.enable-otlp-receiver", "--config.file=/etc/prometheus/prometheus.yml")
    .WithHttpEndpoint(targetPort: 9090, name: "http");

builder.AddOpenTelemetryCollector("otelcollector", "../otelcollector/config.yaml")
    .WithEnvironment("PROMETHEUS_ENDPOINT", $"{prometheus.GetEndpoint("http")}/api/v1/otlp");

var grafana = builder.AddContainer("grafana", "grafana/grafana")
    .WithBindMount("../grafana/config", "/etc/grafana", isReadOnly: true)
    .WithBindMount("../grafana/dashboards", "/var/lib/grafana/dashboards", isReadOnly: true)
    .WithEnvironment("PROMETHEUS_ENDPOINT", prometheus.GetEndpoint("http"))
    .WithHttpEndpoint(targetPort: 3000, name: "http");


var writeRepository = builder.AddProject<Projects.DBABookLibrary_WriteRepository>("write-repository")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(writeDatabase)
    .WaitFor(writeDatabase)
    .WithReference(messaging)
    .WaitFor(messaging);


var readRepository = builder.AddProject<Projects.DBABookLibrary_ReadRepository>("read-repository")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(readDatabase)
    .WaitFor(readDatabase)
    .WithReference(messaging)
    .WaitFor(messaging);

builder.AddProject<Projects.DBABookLibrary_Api>("api")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithEnvironment("GRAFANA_URL", grafana.GetEndpoint("http"))
    .WithReference(readRepository)
    .WaitFor(readRepository)
    .WithReference(writeRepository)
    .WaitFor(writeRepository);

var app = builder
    .Build();

await app.RunAsync();
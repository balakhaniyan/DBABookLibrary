using DBABookLibrary.WriteRepository;
using DBABookLibrary.WriteRepository.Messaging;

var builder = WebApplication.CreateBuilder(args);


builder.AddServiceDefaults();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBookWriteRepository, BookWriteRepository>();
builder.Services.AddScoped<IMessaging, Messaging>();

DBABookLibrary.Utils.Dependency.AddWriteDatabase(builder);
DBABookLibrary.Utils.Dependency.AddMessaging(builder);

var app = builder.Build();

app.MapOpenApi();

app.UseHttpsRedirection();

app.UseSwagger(options => { options.RouteTemplate = "swagger/{documentName}/swagger.json"; });

app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "docs";
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "WriteRepository");
});

app.UsePathBase("/api");

app.MapControllers();

app.MapDefaultEndpoints();

await app.RunAsync();
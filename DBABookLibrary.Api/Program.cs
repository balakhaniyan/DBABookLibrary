using DBABookLibrary.Api;
using DBABookLibrary.Service;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppUrls>(builder.Configuration.GetSection("AppUrls"));

AddServices.Add(builder.Services);

DBABookLibrary.Utils.Dependency.AddReadDatabase(builder);

var app = builder.Build();

app.MapOpenApi();

app.UseHttpsRedirection();

app.UseSwagger(options => { options.RouteTemplate = "swagger/{documentName}/swagger.json"; });

app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "docs";
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "DBABookLibrary");
});

app.UsePathBase("/api");

app.MapControllers();

app.MapDefaultEndpoints();

app.Run();
using DBABookLibrary.IReadRepository;
using DBABookLibrary.IReadRepository.Messaging;
using DBABookLibrary.IReadRepository.Service;
using DBABookLibrary.Model;
using DBABookLibrary.ReadRepository;
using DBABookLibrary.Service;

var builder = WebApplication.CreateBuilder(args);


builder.AddServiceDefaults();
builder.Services.AddProblemDetails();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IBookService, BookService>();
builder.Services.AddTransient<IEventRepository, EventRepository>();
builder.Services.AddTransient<IBookReadRepository, BookReadRepository>();
builder.Services.AddTransient<BookReadContext>();
builder.Services.AddSingleton<AppUrls>();

builder.Services.Configure<AppUrls>(builder.Configuration.GetSection("AppUrls"));


DBABookLibrary.Utils.Dependency.AddReadDatabase(builder);
DBABookLibrary.Utils.Dependency.AddMessaging(builder);

builder.Services.AddHostedService<InitializeMessaging>();

var app = builder.Build();

app.MapOpenApi();

app.UseHttpsRedirection();

app.UseSwagger(options => { options.RouteTemplate = "swagger/{documentName}/swagger.json"; });

app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "docs";
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ReadRepository");
});

app.UsePathBase("/api");

app.MapControllers();

app.MapDefaultEndpoints();

await app.RunAsync();
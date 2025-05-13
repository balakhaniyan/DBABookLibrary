using DBABookLibrary.Service;

namespace DBABookLibrary.Api;

public static class AddServices
{
    public static void Add(IServiceCollection services)
    {
        services.AddTransient<IBookService, BookService>();
        services.AddSingleton<AppUrls>();
    }
}
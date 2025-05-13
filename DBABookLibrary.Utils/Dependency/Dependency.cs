using DBABookLibrary.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace DBABookLibrary.Utils;

public static class Dependency
{
    public static void AddReadDatabase(IHostApplicationBuilder builder)
        =>
            builder
                .AddNpgsqlDbContext<BookReadContext>("DBABookLibraryReadDatabase",
                    configureDbContextOptions: options =>
                    {
                        options.UseNpgsql(b => b.MigrationsAssembly("DBABookLibrary.Migrations"));
                    });

    public static void AddWriteDatabase(IHostApplicationBuilder builder)
        => builder.AddMongoDBClient(connectionName: "DBABookLibraryWriteDatabase");


    public static void AddMessaging(IHostApplicationBuilder builder)
        => builder.AddRabbitMQClient(connectionName: "DBABookLibraryMessenger");
}
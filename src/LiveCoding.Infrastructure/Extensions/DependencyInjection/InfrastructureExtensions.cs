using LiveCoding.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LiveCoding.Infrastructure.Extensions.DependencyInjection
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructureExtensions(this IServiceCollection services)
        {
            // Db Context
            var connection = new Microsoft.Data.Sqlite.SqliteConnection("Data Source=file:memdb1?mode=memory&cache=shared");
            connection.Open();
            services.AddSingleton(connection);

            services.AddDbContext<OrderDbContext>((provider, options) =>
                options.UseSqlite(connection).EnableSensitiveDataLogging());

            return services;
        }
    }
}
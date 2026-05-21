using LiveCoding.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LiveCoding.Infrastructure.DependencyInjection
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services)
        {
            var connection = new Microsoft.Data.Sqlite.SqliteConnection("Data Source=file:memdb1?mode=memory&cache=shared");
            connection.Open();
            services.AddSingleton(connection);

            services.AddDbContext<OrderDbContext>((provider, options) =>
                options.UseSqlite(connection).EnableSensitiveDataLogging());

            return services;
        }
    }
}
using LiveCoding.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LiveCoding.Infrastructure.Extensions.DependencyInjection
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructureExtensions(this IServiceCollection services)
        {
            services.AddDbContext<OrderDbContext>((provider, options) =>
            {
                var connection = new SqliteConnection("Data Source=file:memdb1?mode=memory&cache=shared");
                connection.Open();
                options.UseSqlite(connection);
            });

            return services;
        }
    }
}
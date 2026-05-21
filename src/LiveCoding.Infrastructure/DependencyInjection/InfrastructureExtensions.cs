using LiveCoding.Application.Repositories;
using LiveCoding.Infrastructure.Persistence;
using LiveCoding.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LiveCoding.Infrastructure.DependencyInjection
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services)
        {
            services.AddDbContext<OrderDbContext>(options =>
                options.UseSqlite("DataSource=:memory:").EnableSensitiveDataLogging());
            services.AddScoped<IOrderRepository, OrderRepository>();
            return services;
        }
    }
}
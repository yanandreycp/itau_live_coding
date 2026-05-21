using LiveCoding.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LiveCoding.Infrastructure
{
    public static class DatabaseInitializer
    {
        public static void EnsureDatabaseCreated(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
            db.Database.OpenConnection();
            db.Database.EnsureCreated();
        }
    }
}
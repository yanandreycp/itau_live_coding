using LiveCoding.Application.UseCases.ChangeOrder;
using LiveCoding.Application.UseCases.CreateOrder;
using LiveCoding.Application.UseCases.GetOrder;
using Microsoft.Extensions.DependencyInjection;

namespace LiveCoding.Application.DependencyInjection
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IGetOrderUseCase, GetOrderUseCase>();
            services.AddScoped<ICreateOrderUseCase, CreateOrderUseCase>();
            services.AddScoped<IChangeOrderUseCase, ChangeOrderUseCase>();
            return services;
        }
    }
}
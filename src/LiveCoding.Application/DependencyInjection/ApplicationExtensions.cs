using LiveCoding.Application.Interfaces;
using LiveCoding.Application.Services;
using LiveCoding.Application.UseCases.ChangeProductQuantity;
using LiveCoding.Application.UseCases.CreateOrder;
using LiveCoding.Application.UseCases.GetOrder;
using LiveCoding.Application.UseCases.RemoveOrderProduct;
using LiveCoding.Infrastructure.Interfaces;
using LiveCoding.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace LiveCoding.Application.DependencyInjection
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<IGetOrderUseCase, GetOrderUseCase>();
            services.AddScoped<ICreateOrderUseCase, CreateOrderUseCase>();
            services.AddScoped<IChangeProductQuantityUseCase, ChangeProductQuantityUseCase>();
            services.AddScoped<IRemoveOrderProductUseCase, RemoveOrderProductUseCase>();
            return services;
        }
    }
}
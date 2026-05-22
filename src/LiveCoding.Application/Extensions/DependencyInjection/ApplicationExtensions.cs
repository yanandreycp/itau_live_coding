using LiveCoding.Application.Interfaces;
using LiveCoding.Application.Services;
using LiveCoding.Application.UseCases.ChangeProductQuantity;
using LiveCoding.Application.UseCases.CreateOrder;
using LiveCoding.Application.UseCases.GetOrder;
using LiveCoding.Application.UseCases.RemoveOrderProduct;
using LiveCoding.Infrastructure.Interfaces;
using LiveCoding.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace LiveCoding.Application.Extensions.DependencyInjection
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationExtensions(this IServiceCollection services)
        {
            // Use Cases
            services.AddScoped<IGetOrderUseCase, GetOrderUseCase>();
            services.AddScoped<ICreateOrderUseCase, CreateOrderUseCase>();
            services.AddScoped<IChangeProductQuantityUseCase, ChangeProductQuantityUseCase>();
            services.AddScoped<IRemoveOrderProductUseCase, RemoveOrderProductUseCase>();

            // Validations
            services.AddScoped<IGetOrderValidation, GetOrderValidation>();
            services.AddScoped<ICreateOrderValidation, CreateOrderValidation>();
            services.AddScoped<IChangeProductQuantityValidation, ChangeProductQuantityValidation>();
            services.AddScoped<IRemoveOrderProductValidation, RemoveOrderProductValidation>();

            // Services
            services.AddScoped<IOrderService, OrderService>();

            // Repositories
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
using ECommerce.Application.Commands;
using ECommerce.Application.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateProductHandler>();
            services.AddScoped<GetAllProductsHandler>();
            services.AddScoped<GetProductByIdHandler>();
            services.AddScoped<UpdateProductHandler>();
            services.AddScoped<DeleteProductHandler>();

            services.AddScoped<CreateCategoryHandler>();
            services.AddScoped<GetAllCategoriesHandler>();
            services.AddScoped<GetCategoryByIdHandler>();
            services.AddScoped<UpdateCategoryHandler>();
            services.AddScoped<DeleteCategoryHandler>();

            return services;
        }
    }
}
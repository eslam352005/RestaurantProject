using Microsoft.Extensions.DependencyInjection;
using Restaurant.Application.Interfaces;
using Restaurant.Application.Services;

namespace Restaurant.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            

            services.AddScoped<IBranchService,BranchService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IMenuItemService, MenuitemService>();
            services.AddScoped<ITableService, TableService>();
            services.AddScoped<IStaffService, Staffservice>();
            services.AddScoped<IOrderservice, OrderService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddAutoMapper(cfg => { }, typeof(ApplicationDependencyInjection).Assembly);

            return services;
        }
    }
}

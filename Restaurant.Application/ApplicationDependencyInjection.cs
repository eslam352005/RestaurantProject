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
            services.AddAutoMapper(cfg => { }, typeof(ApplicationDependencyInjection).Assembly);

            return services;
        }
    }
}

namespace Nebula.Modules.Products;

public static class ModuleProducts
{
    public static IServiceCollection AddModuleProductsServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        return services;
    }
}

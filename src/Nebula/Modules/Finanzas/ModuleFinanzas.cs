namespace Nebula.Modules.Finanzas
{
    public static class ModuleFinanzas
    {
        public static IServiceCollection AddModuleFinanzasServices(this IServiceCollection services)
        {
            services.AddScoped<IFinancialAccountService, FinancialAccountService>();
            return services;
        }
    }
}

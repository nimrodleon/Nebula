namespace Nebula.Modules.Finanzas
{
    public static class ModuleFinanzas
    {
        public static IServiceCollection AddModuleFinanzasServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountsReceivableService, AccountsReceivableService>();
            return services;
        }
    }
}

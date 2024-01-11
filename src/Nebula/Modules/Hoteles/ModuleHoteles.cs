namespace Nebula.Modules.Hoteles;

public static class ModuleHoteles
{
    public static IServiceCollection AddModuleHotelesServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoriaHabitacionService, CategoriaHabitacionService>();
        services.AddScoped<IHabitacionHotelService, HabitacionHotelService>();
        services.AddScoped<IPisoHotelService, PisoHotelService>();
        return services;
    }
}

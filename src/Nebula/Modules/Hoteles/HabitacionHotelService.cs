using MongoDB.Bson;
using MongoDB.Driver;
using Nebula.Common;
using Nebula.Modules.Hoteles.Dto;
using Nebula.Modules.Hoteles.Models;

namespace Nebula.Modules.Hoteles;

public interface IHabitacionHotelService : ICrudOperationService<HabitacionHotel>
{
    Task<List<HabitacionDisponible>> GetHabitacionesDisponiblesAsync(string companyId, string query = "", int page = 1, int pageSize = 12);
    Task<long> GetTotalHabitacionesDisponiblesAsync(string companyId, string query = "");
}

public class HabitacionHotelService : CrudOperationService<HabitacionHotel>, IHabitacionHotelService
{
    private readonly ICategoriaHabitacionService _categoriaHabitacionService;
    private readonly IPisoHotelService _pisoHotelService;

    public HabitacionHotelService(MongoDatabaseService mongoDatabase,
        ICategoriaHabitacionService categoriaHabitacionService,
        IPisoHotelService pisoHotelService) : base(mongoDatabase)
    {
        _categoriaHabitacionService = categoriaHabitacionService;
        _pisoHotelService = pisoHotelService;
    }

    public async Task<List<HabitacionDisponible>> GetHabitacionesDisponiblesAsync(string companyId, string query = "", int page = 1, int pageSize = 12)
    {
        var skip = (page - 1) * pageSize;

        var builder = Builders<HabitacionHotel>.Filter;
        var filter = builder.Eq(x => x.CompanyId, companyId);

        if (!string.IsNullOrWhiteSpace(query))
        {
            filter = filter & builder.Regex("Nombre", new BsonRegularExpression(query.ToUpper(), "i"));
        }

        var habitaciones = await _collection.Find(filter).Sort(new SortDefinitionBuilder<HabitacionHotel>()
            .Descending("$natural")).Skip(skip).Limit(pageSize).ToListAsync();

        List<string> pisosIds = new List<string>();
        habitaciones.ForEach(item => pisosIds.Add(item.PisoHotelId));
        var pisos = await _pisoHotelService.GetPisosByArrIds(pisosIds);

        List<string> categoriasIds = new List<string>();
        habitaciones.ForEach(item => categoriasIds.Add(item.CategoriaHabitacionId));
        var categorias = await _categoriaHabitacionService.GetCategoriasByArrIds(categoriasIds);

        var habitacionesDisponibles = new List<HabitacionDisponible>();
        habitaciones.ForEach(item =>
        {
            var piso = pisos.FirstOrDefault(x => x.Id == item.PisoHotelId);
            var categoria = categorias.FirstOrDefault(x => x.Id == item.CategoriaHabitacionId);
            habitacionesDisponibles.Add(new HabitacionDisponible()
            {
                HabitacionId = item.Id,
                Nombre = item.Nombre,
                Piso = $"{item.PisoHotelId}:{piso?.Nombre}",
                Categoria = $"{item.CategoriaHabitacionId}:{categoria?.Nombre}",
                Precio = item.Precio
            });
        });

        return habitacionesDisponibles;
    }

    public async Task<long> GetTotalHabitacionesDisponiblesAsync(string companyId, string query = "")
    {
        var builder = Builders<HabitacionHotel>.Filter;
        var filter = builder.Eq(x => x.CompanyId, companyId);

        if (!string.IsNullOrWhiteSpace(query))
        {
            filter = filter & builder.Regex("Nombre", new BsonRegularExpression(query.ToUpper(), "i"));
        }

        return await _collection.Find(filter).CountDocumentsAsync();
    }
}

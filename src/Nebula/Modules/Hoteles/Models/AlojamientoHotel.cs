using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Nebula.Common.Models;

namespace Nebula.Modules.Hoteles.Models;

[BsonIgnoreExtraElements]
public class AlojamientoHotel : IGenericModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string HabitacionHotelId { get; set; } = string.Empty;
    public string ContactId { get; set; } = string.Empty;
    public string FechaEntrada { get; set; } = string.Empty;
    public string HoraEntrada { get; set; } = string.Empty;
    public string FechaSalida { get; set; } = string.Empty;
    public string HoraSalida { get; set; } = string.Empty;
    public decimal Descuento { get; set; } = decimal.Zero;
    public decimal CobroExtra { get; set; } = decimal.Zero;
    public decimal Adelanto { get; set; } = decimal.Zero;
    public decimal TotalPagar { get; set; } = decimal.Zero;
    public string FormaPago { get; set; } = string.Empty;
    public string Remark { get; set; } = string.Empty;
}

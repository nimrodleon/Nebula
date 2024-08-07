using Microsoft.Data.Sqlite;
using Nebula.Modules.Contacts.Dto;

namespace Nebula.Modules.Contacts;

public interface IContribuyenteService
{
    ContribuyenteDto? GetByRuc(string ruc);
    ContribuyenteDto? GetByDni(string dni);
}

public class ContribuyenteService : IContribuyenteService
{
    private string? dataSource = string.Empty;
    private readonly IConfiguration _configuration;

    public ContribuyenteService(IConfiguration configuration)
    {
        _configuration = configuration;
        dataSource = _configuration.GetValue<string>("BDContribuyentes");
    }

    public ContribuyenteDto? GetByDni(string dni)
    {
        using var connection = new SqliteConnection(dataSource);
        connection.Open();
        string query = "SELECT ruc, dni, nombre, estado, condicion_domicilio, ubigeo, tipo_via, nombre_via, codigo_zona, tipo_zona, numero, interior, lote, departamento, manzana, kilometro FROM contribuyentes WHERE dni = @dni";
        using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@dni", dni);
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            var contribuyente = new ContribuyenteDto
            {
                ruc = reader.GetString(0),
                dni = reader.GetString(1),
                nombre = reader.GetString(2),
                estado = reader.GetString(3),
                condicion_domicilio = reader.GetString(4),
                ubigeo = reader.GetString(5),
                tipo_via = reader.GetString(6),
                nombre_via = reader.GetString(7),
                codigo_zona = reader.GetString(8),
                tipo_zona = reader.GetString(9),
                numero = reader.GetString(10),
                interior = reader.GetString(11),
                lote = reader.GetString(12),
                departamento = reader.GetString(13),
                manzana = reader.GetString(14),
                kilometro = reader.GetString(15)
            };
            return contribuyente;
        }
        return null;
    }

    public ContribuyenteDto? GetByRuc(string ruc)
    {
        using var connection = new SqliteConnection(dataSource);
        connection.Open();
        string query = "SELECT ruc, dni, nombre, estado, condicion_domicilio, ubigeo, tipo_via, nombre_via, codigo_zona, tipo_zona, numero, interior, lote, departamento, manzana, kilometro FROM contribuyentes WHERE ruc = @ruc";
        using var command = new SqliteCommand(query, connection);
        command.Parameters.AddWithValue("@ruc", ruc);
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            var contribuyente = new ContribuyenteDto
            {
                ruc = reader.GetString(0),
                dni = reader.GetString(1),
                nombre = reader.GetString(2),
                estado = reader.GetString(3),
                condicion_domicilio = reader.GetString(4),
                ubigeo = reader.GetString(5),
                tipo_via = reader.GetString(6),
                nombre_via = reader.GetString(7),
                codigo_zona = reader.GetString(8),
                tipo_zona = reader.GetString(9),
                numero = reader.GetString(10),
                interior = reader.GetString(11),
                lote = reader.GetString(12),
                departamento = reader.GetString(13),
                manzana = reader.GetString(14),
                kilometro = reader.GetString(15)
            };
            return contribuyente;
        }
        return null;
    }
}

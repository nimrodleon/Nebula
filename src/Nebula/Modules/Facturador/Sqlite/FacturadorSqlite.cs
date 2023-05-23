using Microsoft.Data.Sqlite;

namespace Nebula.Modules.Facturador.Sqlite;

public class FacturadorSqlite
{
    private string? dataSource = string.Empty;

    public FacturadorSqlite(string? dataSource)
    {
        this.dataSource = dataSource;
    }

    public int BorrarDocumento(string nomArch)
    {
        using var connection = new SqliteConnection(dataSource);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM DOCUMENTO WHERE NOM_ARCH = $nomArch";
        command.Parameters.AddWithValue("$nomArch", nomArch);
        return command.ExecuteNonQuery();
    }

    public List<DocumentoFacturador> GetDocumentos()
    {
        var documentos = new List<DocumentoFacturador>();
        using var connection = new SqliteConnection(dataSource);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM DOCUMENTO";
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var doc = new DocumentoFacturador();
            doc.NUM_RUC = reader.GetString(reader.GetOrdinal("NUM_RUC"));
            doc.TIP_DOCU = reader.GetString(reader.GetOrdinal("TIP_DOCU"));
            doc.NUM_DOCU = reader.GetString(reader.GetOrdinal("NUM_DOCU"));
            if (!reader.IsDBNull(reader.GetOrdinal("FEC_CARG")))
                doc.FEC_CARG = reader.GetDateTime(reader.GetOrdinal("FEC_CARG"));
            if (!reader.IsDBNull(reader.GetOrdinal("FEC_GENE")))
                doc.FEC_GENE = reader.GetDateTime(reader.GetOrdinal("FEC_GENE"));
            if (!reader.IsDBNull(reader.GetOrdinal("FEC_ENVI")))
                doc.FEC_ENVI = reader.GetDateTime(reader.GetOrdinal("FEC_ENVI"));
            if (!reader.IsDBNull(reader.GetOrdinal("DES_OBSE")))
                doc.DES_OBSE = reader.GetString(reader.GetOrdinal("DES_OBSE"));
            if (!reader.IsDBNull(reader.GetOrdinal("NOM_ARCH")))
                doc.NOM_ARCH = reader.GetString(reader.GetOrdinal("NOM_ARCH"));
            if (!reader.IsDBNull(reader.GetOrdinal("IND_SITU")))
                doc.IND_SITU = reader.GetString(reader.GetOrdinal("IND_SITU"));
            if (!reader.IsDBNull(reader.GetOrdinal("TIP_ARCH")))
                doc.TIP_ARCH = reader.GetString(reader.GetOrdinal("TIP_ARCH"));
            if (!reader.IsDBNull(reader.GetOrdinal("FIRM_DIGITAL")))
                doc.FIRM_DIGITAL = reader.GetString(reader.GetOrdinal("FIRM_DIGITAL"));
            documentos.Add(doc);
        }
        return documentos;
    }
}

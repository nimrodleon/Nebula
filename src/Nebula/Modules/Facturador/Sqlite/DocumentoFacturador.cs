namespace Nebula.Modules.Facturador.Sqlite;

public class DocumentoFacturador
{
    public string NUM_RUC { get; set; } = string.Empty;
    public string TIP_DOCU { get; set; } = string.Empty;
    public string NUM_DOCU { get; set; } = string.Empty;
    public DateTime FEC_CARG { get; set; }
    public DateTime FEC_GENE { get; set; }
    public DateTime FEC_ENVI { get; set; }
    public string DES_OBSE { get; set; } = string.Empty;
    public string NOM_ARCH { get; set; } = string.Empty;
    public string IND_SITU { get; set; } = string.Empty;
    public string TIP_ARCH { get; set; } = string.Empty;
    public string FIRM_DIGITAL { get; set; } = string.Empty;
}

using Nebula.Modules.Lycet.Models.Company;

namespace Nebula.Modules.Lycet.Models.Sale;

public class LycetEmbededDespatch
{
    public LycetAddress Llegada { get; set; } = new LycetAddress();
    public LycetAddress Partida { get; set; } = new LycetAddress();
    public LycetClient Transportista { get; set; } = new LycetClient();

    /// <summary>
    /// N° de licencia de conducir.
    /// </summary>
    public string NroLicencia { get; set; } = string.Empty;
    public string TranspPlaca { get; set; } = string.Empty;
    public string TranspCodeAuth { get; set; } = string.Empty;
    public string TranspMarca { get; set; } = string.Empty;
    public string ModTraslado { get; set; } = string.Empty;
    public decimal PesoBruto { get; set; }
    public string UndPesoBruto { get; set; } = string.Empty;
}

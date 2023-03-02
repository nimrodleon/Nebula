namespace Nebula.Plugins.Facturador.Dto;

public class BandejaFacturador
{
    public string Validacion { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;

    /// <summary>
    /// Lista de comprobantes del facturador.
    /// </summary>
    public List<ItemBandejaFacturador> ListaBandejaFacturador { get; set; } = new List<ItemBandejaFacturador>();

    /// <summary>
    /// Obtener un Item de la ListaBandejaFacturador.
    /// </summary>
    /// <param name="nomArch">20520485750-07-BC01-00000008</param>
    /// <returns>ItemBandejaFacturador|null</returns>
    public ItemBandejaFacturador? GetItemBandejaFacturador(string nomArch)
    {
        return ListaBandejaFacturador.FirstOrDefault(x => x.NomArch == nomArch);
    }
}

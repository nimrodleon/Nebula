namespace Nebula.Plugins.Facturador.Dto;

public class BandejaFacturador
{
    public string validacion { get; set; } = string.Empty;
    public string mensaje { get; set; } = string.Empty;

    /// <summary>
    /// Lista de comprobantes del facturador.
    /// </summary>
    public List<ItemBandejaFacturador> listaBandejaFacturador { get; set; } = new List<ItemBandejaFacturador>();

    /// <summary>
    /// Obtener un Item de la ListaBandejaFacturador.
    /// </summary>
    /// <param name="nomArch">20520485750-07-BC01-00000008</param>
    /// <returns>ItemBandejaFacturador|null</returns>
    public ItemBandejaFacturador? GetItemBandejaFacturador(string nomArch)
    {
        return listaBandejaFacturador.FirstOrDefault(x => x.nom_arch == nomArch);
    }
}

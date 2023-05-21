namespace Nebula.Modules.Facturador.Models;

public class AdicionalCabecera
{
    #region DETRACCIONES!
    //public string ctaBancoNacionDetraccion { get; set; } = string.Empty;
    //public string codBienDetraccion { get; set; } = string.Empty;
    //public string porDetraccion { get; set; } = string.Empty;
    //public string mtoDetraccion { get; set; } = string.Empty;
    //public string codMedioPago { get; set; } = string.Empty;
    #endregion
    #region DIRECCIÓN_DEL_CLIENTE!
    public string codPaisCliente { get; set; } = "-";
    public string codUbigeoCliente { get; set; } = string.Empty;
    public string desDireccionCliente { get; set; } = string.Empty;
    #endregion
    #region DIRECCIÓN_DISTINTA_DEL_CLIENTE!
    public string codPaisEntrega { get; set; } = "-";
    //public string codUbigeoEntrega { get; set; } = string.Empty;
    //public string desDireccionEntrega { get; set; } = string.Empty;
    #endregion
}

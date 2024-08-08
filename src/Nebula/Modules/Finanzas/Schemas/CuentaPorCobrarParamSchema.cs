using System.ComponentModel.DataAnnotations;

namespace Nebula.Modules.Finanzas.Schemas;

public abstract class CuentaPorCobrarParamSchema
{
    [Required]
    public string Status { get; set; } = string.Empty;
    [Required]
    public string Year { get; set; } = string.Empty;
}

public class CuentaPorCobrarClienteAnualParamSchema : CuentaPorCobrarParamSchema
{
    [Required]
    public string ContactId { get; set; } = string.Empty;
}

public class CuentaPorCobrarMensualParamSchema : CuentaPorCobrarParamSchema
{
    [Required]
    public string Month { get; set; } = string.Empty;
}

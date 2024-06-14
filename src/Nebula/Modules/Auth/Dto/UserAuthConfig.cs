using Nebula.Modules.Account.Dto;
using Nebula.Modules.Account.Models;

namespace Nebula.Modules.Auth.Dto;

public class UserAuthConfig
{
    public UserAuth UserAuth { get; set; } = new UserAuth();
    public List<CompanySummaryDto> Companies { get; set; } = new List<CompanySummaryDto>();
    public string CompanyName { get; set; } = string.Empty;
    public bool IsEnableModComprobante { get; set; } = false;
    public bool IsEnableModCuentaPorCobrar { get; set; } = false;
    public bool IsEnableModReparaciones { get; set; } = false;
    public bool IsEnableModCaja { get; set; } = false;
}

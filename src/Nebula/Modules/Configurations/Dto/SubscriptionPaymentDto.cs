namespace Nebula.Modules.Configurations.Dto;

public class LicenseDto
{
    public bool Ok { get; set; } = false;
    public string OriginalText { get; set; } = "-";
}

public class SubscriptionPaymentDto
{
    public string Uuid { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string UuidAccess { get; set; } = string.Empty;
    public bool Payment { get; set; } = false;
    public string Desde { get; set; } = string.Empty;
    public string Hasta { get; set; } = string.Empty;
}

public class ResponseSubscriptionPaymentDto
{
    public bool Ok { get; set; } = false;
    public SubscriptionPaymentDto Data { get; set; } = new SubscriptionPaymentDto();
}

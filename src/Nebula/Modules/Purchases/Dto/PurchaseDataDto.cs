namespace Nebula.Modules.Purchases.Dto;

public class PurchaseDataDto
{
    #region ORIGIN_HTTP_REQUEST!
    public PurchaseHeaderDto HeaderDto { get; set; } = new PurchaseHeaderDto();
    public List<PurchaseDetailDto> DetailDtos { get; set; } = new List<PurchaseDetailDto>();
    #endregion
}

using Nebula.Modules.Inventory.Helpers;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Stock.Helpers;

public class AjusteInventarioCalculator
{
    private readonly List<ProductStock> _stocks;
    private readonly List<AjusteInventarioDetail> _ajusteInventarioDetails;

    public AjusteInventarioCalculator(List<ProductStock> stocks, List<AjusteInventarioDetail> ajusteInventarioDetails)
    {
        _stocks = stocks;
        _ajusteInventarioDetails = ajusteInventarioDetails;
    }

    public List<AjusteInventarioDetail> CalcularCantidadExistente(string companyId)
    {
        _ajusteInventarioDetails.ForEach(item =>
        {
            item.Id = string.Empty;
            var products = _stocks.Where(x => x.CompanyId == companyId && x.ProductId == item.ProductId).ToList();
            var entrada = products.Where(x => x.TransactionType == TransactionType.ENTRADA).Sum(x => x.Quantity);
            var salida = products.Where(x => x.TransactionType == TransactionType.SALIDA).Sum(x => x.Quantity);
            item.CantExistente = entrada - salida;
        });
        return _ajusteInventarioDetails;
    }
}

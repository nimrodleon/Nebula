using Nebula.Modules.Inventory.Helpers;
using Nebula.Modules.Inventory.Models;

namespace Nebula.Modules.Inventory.Stock.Helpers;

public class TransferenciaCalculator
{
    private List<ProductStock> _stocks;
    private List<TransferenciaDetail> _transferenciaDetails;

    public TransferenciaCalculator(List<ProductStock> stocks, List<TransferenciaDetail> transferenciaDetails)
    {
        _stocks = stocks;
        _transferenciaDetails = transferenciaDetails;
    }

    public List<TransferenciaDetail> CalcularCantidadesRestante(string companyId)
    {
        _transferenciaDetails.ForEach(item =>
        {
            item.Id = string.Empty;
            var products = _stocks.Where(x => x.CompanyId == companyId &&  x.ProductId == item.ProductId).ToList();
            var entrada = products.Where(x => x.TransactionType == TransactionType.ENTRADA).Sum(x => x.Quantity);
            var salida = products.Where(x => x.TransactionType == TransactionType.SALIDA).Sum(x => x.Quantity);
            item.CantExistente = entrada - salida;
            item.CantRestante = item.CantExistente - item.CantTransferido;
        });
        return _transferenciaDetails;
    }
}

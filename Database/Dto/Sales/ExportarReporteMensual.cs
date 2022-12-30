using ClosedXML.Excel;
using Nebula.Database.Models.Sales;

namespace Nebula.Database.Dto.Sales;

public class ExportarReporteMensual
{
    private readonly List<InvoiceSale> invoiceSales;

    public ExportarReporteMensual(List<InvoiceSale> invoiceSales)
    {
        this.invoiceSales = invoiceSales;
    }

    public string GuardarCambios()
    {
        string fileName = Guid.NewGuid().ToString();
        string filePath = Path.Combine(Path.GetTempPath(), $"{fileName}.xlsx");
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("COMPROBANTES");

            workbook.SaveAs(filePath);
        }
        return filePath;
    }
}

using ClosedXML.Excel;
using Nebula.Database.Models.Common;
using Nebula.Database.Models.Sales;

namespace Nebula.Database.Dto.Sales;

public class ExportarReporteMensual
{
    private readonly List<InvoiceSerie> invoiceSeries;
    private readonly List<InvoiceSale> invoiceSales;

    public ExportarReporteMensual(List<InvoiceSerie> invoiceSeries, List<InvoiceSale> invoiceSales)
    {
        this.invoiceSeries = invoiceSeries;
        this.invoiceSales = invoiceSales;
    }

    private List<InvoiceSale> GetBoletas(string invoiceSerieId)
    {
        return invoiceSales.Where(x => x.DocType == "BOLETA"
        && x.InvoiceSerieId == invoiceSerieId).OrderBy(x => x.Number).ToList();
    }

    private List<InvoiceSale> GetFacturas(string invoiceSerieId)
    {
        return invoiceSales.Where(x => x.DocType == "FACTURA"
        && x.InvoiceSerieId == invoiceSerieId).OrderBy(x => x.Number).ToList();
    }

    public string GuardarCambios()
    {
        string fileName = Guid.NewGuid().ToString();
        string filePath = Path.Combine(Path.GetTempPath(), $"{fileName}.xlsx");
        using (var workbook = new XLWorkbook())
        {
            invoiceSeries.ForEach(serieComprobante =>
            {
                var worksheet = workbook.Worksheets.Add(serieComprobante.Name);
                // generar cabecera documento.
                worksheet.Cell("A1").Value = "FECHA";
                worksheet.Cell("B1").Value = "N° BOLETA";
                worksheet.Cell("C1").Value = "IMPORTE TOTAL";
                worksheet.Cell("D1").Value = "ESTADO SUNAT";
                // E1 - empty.
                worksheet.Cell("F1").Value = "FECHA";
                worksheet.Cell("G1").Value = "N° FACTURA";
                worksheet.Cell("H1").Value = "IMPORTE TOTAL";
                worksheet.Cell("I1").Value = "ESTADO SUNAT";
                // formato cabecera documento.
                var cabeceraBoletas = worksheet.Range("A1:D1");
                cabeceraBoletas.Style.Font.Bold = true;
                cabeceraBoletas.Style.Font.FontColor = XLColor.White;
                cabeceraBoletas.Style.Fill.BackgroundColor = XLColor.Black;
                var cabeceraFacturas = worksheet.Range("F1:I1");
                cabeceraFacturas.Style.Font.Bold = true;
                cabeceraFacturas.Style.Font.FontColor = XLColor.White;
                cabeceraFacturas.Style.Fill.BackgroundColor = XLColor.Black;
                // generar registro de boletas.            
                int contador = 2;
                List<InvoiceSale> boletas = GetBoletas(serieComprobante.Id);
                foreach (var item in boletas)
                {
                    worksheet.Cell(contador, 1).Value = item.FecEmision;
                    worksheet.Cell(contador, 2).Value = $"{item.Serie}-{item.Number}";
                    worksheet.Cell(contador, 3).Value = item.SumImpVenta;
                    worksheet.Cell(contador, 4).Value = item.SituacionFacturador;
                    contador++;
                }
                // generar registro de facturas.
                contador = 2;
                List<InvoiceSale> facturas = GetFacturas(serieComprobante.Id);
                foreach (var item in facturas)
                {
                    worksheet.Cell(contador, 6).Value = item.FecEmision;
                    worksheet.Cell(contador, 7).Value = $"{item.Serie}-{item.Number}";
                    worksheet.Cell(contador, 8).Value = item.SumImpVenta;
                    worksheet.Cell(contador, 9).Value = item.SituacionFacturador;
                    contador++;
                }
                // ajustar ancho de las columnas para que se muestren todo el contenido.
                worksheet.Columns(1, 4).AdjustToContents();
                worksheet.Columns(6, 9).AdjustToContents();
            });
            workbook.SaveAs(filePath);
        }
        return filePath;
    }
}

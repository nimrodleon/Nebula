using ClosedXML.Excel;
using Nebula.Database.Models.Common;
using Nebula.Database.Models.Sales;

namespace Nebula.Database.Dto.Sales;

public class ExportarReporteMensual
{
    private readonly List<InvoiceSerie> invoiceSeries;
    private readonly List<InvoiceSale> invoiceSales;
    private readonly List<CreditNote> creditNotes;

    public ExportarReporteMensual(List<InvoiceSerie> invoiceSeries,
        List<InvoiceSale> invoiceSales, List<CreditNote> creditNotes)
    {
        this.invoiceSeries = invoiceSeries;
        this.invoiceSales = invoiceSales;
        this.creditNotes = creditNotes;
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

    private List<CreditNote> GetCreditNotes(string invoiceSerieId, string serie)
    {
        return creditNotes.Where(x => x.InvoiceSerieId.Equals(invoiceSerieId)
        && x.Serie.Equals(serie)).OrderBy(x => x.Number).ToList();
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
                #region BOLETA!
                // generar cabecera documento.
                worksheet.Cell("A1").Value = "FECHA";
                worksheet.Cell("B1").Value = "N° BOLETA";
                worksheet.Cell("C1").Value = "IMPORTE TOTAL";
                worksheet.Cell("D1").Value = "ESTADO SUNAT";
                worksheet.Cell("E1").Value = "ANULADO";
                // formato cabecera documento.
                var cabeceraBoletas = worksheet.Range("A1:E1");
                cabeceraBoletas.Style.Font.Bold = true;
                cabeceraBoletas.Style.Font.FontColor = XLColor.White;
                cabeceraBoletas.Style.Fill.BackgroundColor = XLColor.DarkBlue;
                // generar registro de boletas.            
                int contador = 2;
                List<InvoiceSale> boletas = GetBoletas(serieComprobante.Id);
                foreach (var item in boletas)
                {
                    worksheet.Cell(contador, 1).Value = item.FecEmision;
                    worksheet.Cell(contador, 2).Value = $"{item.Serie}-{item.Number}";
                    worksheet.Cell(contador, 3).Value = item.SumImpVenta;
                    worksheet.Cell(contador, 4).Value = item.SituacionFacturador;
                    worksheet.Cell(contador, 5).Value = item.Anulada ? "SI" : "NO";
                    contador++;
                }
                // ajustar ancho de las columnas para que se muestren todo el contenido.
                worksheet.Columns(1, 5).AdjustToContents();
                #endregion
                #region NOTA_CRÉDITO_BOLETA!
                worksheet.Cell("G1").Value = "FECHA";
                worksheet.Cell("H1").Value = "N° NOTA CRÉDITO";
                worksheet.Cell("I1").Value = "IMPORTE TOTAL";
                worksheet.Cell("J1").Value = "ESTADO SUNAT";
                worksheet.Cell("K1").Value = "DOC. AFECTADO";
                var cabeceraNotaBoletas = worksheet.Range("G1:K1");
                cabeceraNotaBoletas.Style.Font.Bold = true;
                cabeceraNotaBoletas.Style.Font.FontColor = XLColor.White;
                cabeceraNotaBoletas.Style.Fill.BackgroundColor = XLColor.DarkRed;
                List<CreditNote> notasCreditoBoleta = GetCreditNotes(serieComprobante.Id, serieComprobante.CreditNoteBoleta);
                contador = 2;
                foreach (var item in notasCreditoBoleta)
                {
                    worksheet.Cell(contador, 7).Value = item.FecEmision;
                    worksheet.Cell(contador, 8).Value = $"{item.Serie}-{item.Number}";
                    worksheet.Cell(contador, 9).Value = item.SumImpVenta;
                    worksheet.Cell(contador, 10).Value = item.SituacionFacturador;
                    worksheet.Cell(contador, 11).Value = item.NumDocAfectado;
                    contador++;
                }
                // ajustar ancho de las columnas para que se muestren todo el contenido.
                worksheet.Columns(7, 11).AdjustToContents();
                #endregion
                #region FACTURA!
                worksheet.Cell("M1").Value = "FECHA";
                worksheet.Cell("N1").Value = "N° FACTURA";
                worksheet.Cell("O1").Value = "IMPORTE TOTAL";
                worksheet.Cell("P1").Value = "ESTADO SUNAT";
                worksheet.Cell("Q1").Value = "ANULADO";
                // formato cabecera documento.
                var cabeceraFacturas = worksheet.Range("M1:Q1");
                cabeceraFacturas.Style.Font.Bold = true;
                cabeceraFacturas.Style.Font.FontColor = XLColor.White;
                cabeceraFacturas.Style.Fill.BackgroundColor = XLColor.DarkBlue;
                // generar registro de facturas.
                contador = 2;
                List<InvoiceSale> facturas = GetFacturas(serieComprobante.Id);
                foreach (var item in facturas)
                {
                    worksheet.Cell(contador, 13).Value = item.FecEmision;
                    worksheet.Cell(contador, 14).Value = $"{item.Serie}-{item.Number}";
                    worksheet.Cell(contador, 15).Value = item.SumImpVenta;
                    worksheet.Cell(contador, 16).Value = item.SituacionFacturador;
                    worksheet.Cell(contador, 17).Value = item.Anulada ? "SI" : "NO";
                    contador++;
                }
                // ajustar ancho de las columnas para que se muestren todo el contenido.
                worksheet.Columns(13, 17).AdjustToContents();
                #endregion                
                #region NOTA_CRÉDITO_FACTURA!
                worksheet.Cell("S1").Value = "FECHA";
                worksheet.Cell("T1").Value = "N° NOTA CRÉDITO";
                worksheet.Cell("U1").Value = "IMPORTE TOTAL";
                worksheet.Cell("V1").Value = "ESTADO SUNAT";
                worksheet.Cell("W1").Value = "DOC. AFECTADO";
                var cabeceraNotaFacturas = worksheet.Range("S1:W1");
                cabeceraNotaFacturas.Style.Font.Bold = true;
                cabeceraNotaFacturas.Style.Font.FontColor = XLColor.White;
                cabeceraNotaFacturas.Style.Fill.BackgroundColor = XLColor.DarkRed;
                List<CreditNote> notasCreditoFactura = GetCreditNotes(serieComprobante.Id, serieComprobante.CreditNoteFactura);
                contador = 2;
                foreach (var item in notasCreditoFactura)
                {
                    worksheet.Cell(contador, 19).Value = item.FecEmision;
                    worksheet.Cell(contador, 20).Value = $"{item.Serie}-{item.Number}";
                    worksheet.Cell(contador, 21).Value = item.SumImpVenta;
                    worksheet.Cell(contador, 22).Value = item.SituacionFacturador;
                    worksheet.Cell(contador, 23).Value = item.NumDocAfectado;
                    contador++;
                }
                // ajustar ancho de las columnas para que se muestren todo el contenido.
                worksheet.Columns(19, 23).AdjustToContents();
                #endregion
            });
            workbook.SaveAs(filePath);
        }
        return filePath;
    }
}

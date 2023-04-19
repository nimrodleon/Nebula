using System.Globalization;
using ClosedXML.Excel;
using Nebula.Database.Models.Common;
using Nebula.Database.Models.Sales;

namespace Nebula.Database.Dto.Sales;

/// <summary>
/// Crear excel formato 14.1 - ventas.
/// </summary>
public class ExcelRegistroVentasF141
{
    private readonly List<InvoiceSerie> _invoiceSeries;
    private readonly List<InvoiceSale> _invoiceSales;
    private readonly List<CreditNote> _creditNotes;
    private readonly List<TributoSale> _tributoSales;

    public ExcelRegistroVentasF141(List<InvoiceSerie> invoiceSeries,
        List<InvoiceSale> invoiceSales, List<CreditNote> creditNotes, List<TributoSale> tributoSales)
    {
        _invoiceSeries = invoiceSeries;
        _invoiceSales = invoiceSales;
        _creditNotes = creditNotes;
        _tributoSales = tributoSales;
    }

    public string CrearArchivo()
    {
        string fileName = Guid.NewGuid().ToString();
        string filePath = Path.Combine(Path.GetTempPath(), $"{fileName}.xlsx");
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Registro de Ventas - F14.1");

        worksheet.Style.Font.FontSize = 8;
        worksheet.Style.Font.FontName = "Arial Narrow";
        // worksheet.Style.Fill.BackgroundColor = XLColor.White;

        #region Cabecera

        for (int i = 1; i <= 35; i++)
        {
            worksheet.Cell(1, i).Value = i.ToString();
            worksheet.Cell(1, i).Style.Alignment.WrapText = true;
            worksheet.Cell(1, i).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(1, i).Style.Border.TopBorderColor = XLColor.White;
            worksheet.Cell(1, i).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(1, i).Style.Border.RightBorderColor = XLColor.White;
        }

        worksheet.Cell("A1").Style.Font.Bold = true;
        worksheet.Cell("A1").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("A1").Style.Fill.BackgroundColor = XLColor.Red;
        worksheet.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        var cabeceraFila1 = worksheet.Range("B1:AI1");
        cabeceraFila1.Style.Font.Bold = true;
        cabeceraFila1.Style.Font.FontColor = XLColor.White;
        cabeceraFila1.Style.Fill.BackgroundColor = XLColor.Black;
        cabeceraFila1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        // Periodo
        var rangoPeriodo = worksheet.Range("A2:A3");
        rangoPeriodo.Merge();
        rangoPeriodo.Value = "PERIODO";
        rangoPeriodo.Style.Font.Bold = true;
        rangoPeriodo.Style.Font.FontColor = XLColor.White;
        rangoPeriodo.Style.Fill.BackgroundColor = XLColor.Red;
        rangoPeriodo.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        rangoPeriodo.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        rangoPeriodo.Style.Alignment.WrapText = true;
        rangoPeriodo.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        rangoPeriodo.Style.Border.TopBorderColor = XLColor.White;
        rangoPeriodo.Style.Border.RightBorder = XLBorderStyleValues.Thin;
        rangoPeriodo.Style.Border.RightBorderColor = XLColor.White;
        // Código único de la operación.
        var rangoCuoOperación = worksheet.Range("B2:B3");
        rangoCuoOperación.Merge();
        rangoCuoOperación.Value = "CÓDIGO ÚNICO\n DE LA OPERACIÓN \n(CUO)";
        rangoCuoOperación.Style.Font.Bold = true;
        rangoCuoOperación.Style.Font.FontColor = XLColor.White;
        rangoCuoOperación.Style.Fill.BackgroundColor = XLColor.Black;
        rangoCuoOperación.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        rangoCuoOperación.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        rangoCuoOperación.Style.Alignment.WrapText = true;
        rangoCuoOperación.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        rangoCuoOperación.Style.Border.TopBorderColor = XLColor.White;
        rangoCuoOperación.Style.Border.RightBorder = XLBorderStyleValues.Thin;
        rangoCuoOperación.Style.Border.RightBorderColor = XLColor.White;
        // Número correlativo del Cuo.
        var rangoNumCorrelativoCuo = worksheet.Range("C2:C3");
        rangoNumCorrelativoCuo.Merge();
        rangoNumCorrelativoCuo.Value = "NUMERO\nCORRELATIVO\nDEL (CUO)";
        rangoNumCorrelativoCuo.Style.Font.Bold = true;
        rangoNumCorrelativoCuo.Style.Font.FontColor = XLColor.White;
        rangoNumCorrelativoCuo.Style.Fill.BackgroundColor = XLColor.Black;
        rangoNumCorrelativoCuo.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        rangoNumCorrelativoCuo.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        rangoNumCorrelativoCuo.Style.Alignment.WrapText = true;
        rangoNumCorrelativoCuo.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        rangoNumCorrelativoCuo.Style.Border.TopBorderColor = XLColor.White;
        rangoNumCorrelativoCuo.Style.Border.RightBorder = XLBorderStyleValues.Thin;
        rangoNumCorrelativoCuo.Style.Border.RightBorderColor = XLColor.White;

        #region Comprobantes de Pago o Documentos

        var rangoTitleComprobantesDePago = worksheet.Range("D2:I2");
        rangoTitleComprobantesDePago.Merge();
        rangoTitleComprobantesDePago.Value = "COMPROBANTE DE PAGO O DOCUMENTO";
        rangoTitleComprobantesDePago.Style.Font.Bold = true;
        rangoTitleComprobantesDePago.Style.Font.FontColor = XLColor.White;
        rangoTitleComprobantesDePago.Style.Fill.BackgroundColor = XLColor.DarkRed;
        rangoTitleComprobantesDePago.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        rangoTitleComprobantesDePago.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        rangoTitleComprobantesDePago.Style.Border.TopBorderColor = XLColor.White;
        rangoTitleComprobantesDePago.Style.Border.RightBorder = XLBorderStyleValues.Thin;
        rangoTitleComprobantesDePago.Style.Border.RightBorderColor = XLColor.White;
        // fecha de emisión.
        worksheet.Cell("D3").Value = "FECHA DE\nEMISIÓN";
        worksheet.Cell("D3").Style.Font.Bold = true;
        worksheet.Cell("D3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("D3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("D3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("D3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("D3").Style.Alignment.WrapText = true;
        worksheet.Cell("D3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("D3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("D3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("D3").Style.Border.RightBorderColor = XLColor.White;
        // fechas de vencimiento.
        worksheet.Cell("E3").Value = "FECHA DE\nVENCIMIENTO";
        worksheet.Cell("E3").Style.Font.Bold = true;
        worksheet.Cell("E3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("E3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("E3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("E3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("E3").Style.Alignment.WrapText = true;
        worksheet.Cell("E3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("E3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("E3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("E3").Style.Border.RightBorderColor = XLColor.White;
        // tipo.
        worksheet.Cell("F3").Value = "TIPO";
        worksheet.Cell("F3").Style.Font.Bold = true;
        worksheet.Cell("F3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("F3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("F3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("F3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("F3").Style.Alignment.WrapText = true;
        worksheet.Cell("F3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("F3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("F3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("F3").Style.Border.RightBorderColor = XLColor.White;
        // serie comprobante.
        worksheet.Cell("G3").Value = "SERIE";
        worksheet.Cell("G3").Style.Font.Bold = true;
        worksheet.Cell("G3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("G3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("G3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("G3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("G3").Style.Alignment.WrapText = true;
        worksheet.Cell("G3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("G3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("G3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("G3").Style.Border.RightBorderColor = XLColor.White;
        // número comprobante.
        worksheet.Cell("H3").Value = "NÚMERO";
        worksheet.Cell("H3").Style.Font.Bold = true;
        worksheet.Cell("H3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("H3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("H3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("H3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("H3").Style.Alignment.WrapText = true;
        worksheet.Cell("H3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("H3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("H3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("H3").Style.Border.RightBorderColor = XLColor.White;
        // numero final de los comprobantes.
        worksheet.Cell("I3").Value = "NÚMERO FINAL";
        worksheet.Cell("I3").Style.Font.Bold = true;
        worksheet.Cell("I3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("I3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("I3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("I3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("I3").Style.Alignment.WrapText = true;
        worksheet.Cell("I3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("I3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("I3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("I3").Style.Border.RightBorderColor = XLColor.White;

        #endregion

        #region Datos del Cliente

        var rangoTitleCliente = worksheet.Range("J2:L2");
        rangoTitleCliente.Merge();
        rangoTitleCliente.Value = "CLIENTE";
        rangoTitleCliente.Style.Font.Bold = true;
        rangoTitleCliente.Style.Font.FontColor = XLColor.White;
        rangoTitleCliente.Style.Fill.BackgroundColor = XLColor.DarkRed;
        rangoTitleCliente.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        rangoTitleCliente.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        rangoTitleCliente.Style.Border.TopBorderColor = XLColor.White;
        rangoTitleCliente.Style.Border.RightBorder = XLBorderStyleValues.Thin;
        rangoTitleCliente.Style.Border.RightBorderColor = XLColor.White;

        // Tipo de Documento de Identidad del cliente
        worksheet.Cell("J3").Value = "TIPO";
        worksheet.Cell("J3").Style.Font.Bold = true;
        worksheet.Cell("J3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("J3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("J3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("J3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("J3").Style.Alignment.WrapText = true;
        worksheet.Cell("J3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("J3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("J3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("J3").Style.Border.RightBorderColor = XLColor.White;

        // Número de Documento de Identidad del cliente
        worksheet.Cell("K3").Value = "NÚMERO";
        worksheet.Cell("K3").Style.Font.Bold = true;
        worksheet.Cell("K3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("K3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("K3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("K3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("K3").Style.Alignment.WrapText = true;
        worksheet.Cell("K3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("K3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("K3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("K3").Style.Border.RightBorderColor = XLColor.White;

        // Apellidos y nombres, denominación o razón social  del cliente.
        worksheet.Cell("L3").Value = "APELLIDOS Y NOMBRES O RAZÓN SOCIAL DEL CLIENTE";
        worksheet.Cell("L3").Style.Font.Bold = true;
        worksheet.Cell("L3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("L3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("L3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("L3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("L3").Style.Alignment.WrapText = true;
        worksheet.Cell("L3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("L3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("L3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("L3").Style.Border.RightBorderColor = XLColor.White;

        #endregion

        #region Importes del Comprobante de pago o documento

        var rangoTitleImportes = worksheet.Range("M2:Y2");
        rangoTitleImportes.Merge();
        rangoTitleImportes.Value = "IMPORTES DEL COMPROBANTE DE PAGO O DOCUMENTO";
        rangoTitleImportes.Style.Font.Bold = true;
        rangoTitleImportes.Style.Font.FontColor = XLColor.White;
        rangoTitleImportes.Style.Fill.BackgroundColor = XLColor.DarkRed;
        rangoTitleImportes.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        rangoTitleImportes.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        rangoTitleImportes.Style.Border.TopBorderColor = XLColor.White;
        rangoTitleImportes.Style.Border.RightBorder = XLBorderStyleValues.Thin;
        rangoTitleImportes.Style.Border.RightBorderColor = XLColor.White;

        // Valor facturado de la exportación
        worksheet.Cell("M3").Value = "EXPORTACIÓN";
        worksheet.Cell("M3").Style.Font.Bold = true;
        worksheet.Cell("M3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("M3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("M3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("M3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("M3").Style.Alignment.WrapText = true;
        worksheet.Cell("M3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("M3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("M3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("M3").Style.Border.RightBorderColor = XLColor.White;

        // Base imponible de la operación gravada (4)
        worksheet.Cell("N3").Value = "BASE IMPONIBLE";
        worksheet.Cell("N3").Style.Font.Bold = true;
        worksheet.Cell("N3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("N3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("N3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("N3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("N3").Style.Alignment.WrapText = true;
        worksheet.Cell("N3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("N3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("N3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("N3").Style.Border.RightBorderColor = XLColor.White;

        // Descuento de la Base Imponible
        worksheet.Cell("O3").Value = "DESCUENTO\nBASE\nIMPONIBLE";
        worksheet.Cell("O3").Style.Font.Bold = true;
        worksheet.Cell("O3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("O3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("O3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("O3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("O3").Style.Alignment.WrapText = true;
        worksheet.Cell("O3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("O3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("O3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("O3").Style.Border.RightBorderColor = XLColor.White;

        // Impuesto General a las Ventas y/o Impuesto de Promoción Municipal
        worksheet.Cell("P3").Value = "IGV";
        worksheet.Cell("P3").Style.Font.Bold = true;
        worksheet.Cell("P3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("P3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("P3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("P3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("P3").Style.Alignment.WrapText = true;
        worksheet.Cell("P3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("P3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("P3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("P3").Style.Border.RightBorderColor = XLColor.White;

        // Descuento del Impuesto General a las Ventas y/o Impuesto de Promoción Municipal
        worksheet.Cell("Q3").Value = "DESCUENTO\nIGV";
        worksheet.Cell("Q3").Style.Font.Bold = true;
        worksheet.Cell("Q3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("Q3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("Q3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("Q3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("Q3").Style.Alignment.WrapText = true;
        worksheet.Cell("Q3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("Q3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("Q3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("Q3").Style.Border.RightBorderColor = XLColor.White;

        // Importe total de la operación exonerada
        worksheet.Cell("R3").Value = "EXONERADO";
        worksheet.Cell("R3").Style.Font.Bold = true;
        worksheet.Cell("R3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("R3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("R3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("R3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("R3").Style.Alignment.WrapText = true;
        worksheet.Cell("R3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("R3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("R3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("R3").Style.Border.RightBorderColor = XLColor.White;

        // Importe total de la operación inafecta
        worksheet.Cell("S3").Value = "INAFECTO";
        worksheet.Cell("S3").Style.Font.Bold = true;
        worksheet.Cell("S3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("S3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("S3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("S3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("S3").Style.Alignment.WrapText = true;
        worksheet.Cell("S3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("S3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("S3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("S3").Style.Border.RightBorderColor = XLColor.White;

        // Impuesto Selectivo al Consumo, de ser el caso.
        worksheet.Cell("T3").Value = "I.S.C";
        worksheet.Cell("T3").Style.Font.Bold = true;
        worksheet.Cell("T3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("T3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("T3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("T3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("T3").Style.Alignment.WrapText = true;
        worksheet.Cell("T3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("T3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("T3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("T3").Style.Border.RightBorderColor = XLColor.White;

        // Base imponible de la operación gravada con el Impuesto a las Ventas del Arroz Pilado
        worksheet.Cell("U3").Value = "BASE\nIMPONIBLE\nARROZ\nPILADO";
        worksheet.Cell("U3").Style.Font.Bold = true;
        worksheet.Cell("U3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("U3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("U3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("U3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("U3").Style.Alignment.WrapText = true;
        worksheet.Cell("U3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("U3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("U3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("U3").Style.Border.RightBorderColor = XLColor.White;

        // Impuesto a las Ventas del Arroz Pilado
        worksheet.Cell("V3").Value = "IGV\nARROZ\nPILADO";
        worksheet.Cell("V3").Style.Font.Bold = true;
        worksheet.Cell("V3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("V3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("V3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("V3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("V3").Style.Alignment.WrapText = true;
        worksheet.Cell("V3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("V3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("V3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("V3").Style.Border.RightBorderColor = XLColor.White;

        // Impuesto al Consumo de las Bolsas de Plástico.
        worksheet.Cell("W3").Value = "ICBPER";
        worksheet.Cell("W3").Style.Font.Bold = true;
        worksheet.Cell("W3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("W3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("W3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("W3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("W3").Style.Alignment.WrapText = true;
        worksheet.Cell("W3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("W3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("W3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("W3").Style.Border.RightBorderColor = XLColor.White;

        // Otros conceptos, tributos y cargos que no forman parte de la base imponible
        worksheet.Cell("X3").Value = "OTROS\nCONCEPTOS";
        worksheet.Cell("X3").Style.Font.Bold = true;
        worksheet.Cell("X3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("X3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("X3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("X3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("X3").Style.Alignment.WrapText = true;
        worksheet.Cell("X3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("X3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("X3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("X3").Style.Border.RightBorderColor = XLColor.White;

        // Importe total del comprobante de pago
        worksheet.Cell("Y3").Value = "IMPORTE TOTAL";
        worksheet.Cell("Y3").Style.Font.Bold = true;
        worksheet.Cell("Y3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("Y3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("Y3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("Y3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("Y3").Style.Alignment.WrapText = true;
        worksheet.Cell("Y3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("Y3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("Y3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("Y3").Style.Border.RightBorderColor = XLColor.White;

        #endregion

        #region Moneda

        var rangoTitleMoneda = worksheet.Range("Z2:AA2");
        rangoTitleMoneda.Merge();
        rangoTitleMoneda.Value = "MONEDA";
        rangoTitleMoneda.Style.Font.Bold = true;
        rangoTitleMoneda.Style.Font.FontColor = XLColor.White;
        rangoTitleMoneda.Style.Fill.BackgroundColor = XLColor.DarkRed;
        rangoTitleMoneda.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        rangoTitleMoneda.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        rangoTitleMoneda.Style.Border.TopBorderColor = XLColor.White;
        rangoTitleMoneda.Style.Border.RightBorder = XLBorderStyleValues.Thin;
        rangoTitleMoneda.Style.Border.RightBorderColor = XLColor.White;

        // Código  de la Moneda (Tabla 4)
        worksheet.Cell("Z3").Value = "CÓDIGO  MONEDA";
        worksheet.Cell("Z3").Style.Font.Bold = true;
        worksheet.Cell("Z3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("Z3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("Z3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("Z3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("Z3").Style.Alignment.WrapText = true;
        worksheet.Cell("Z3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("Z3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("Z3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("Z3").Style.Border.RightBorderColor = XLColor.White;

        // Tipo de cambio (5)
        worksheet.Cell("AA3").Value = "TIPO DE\nCAMBIO";
        worksheet.Cell("AA3").Style.Font.Bold = true;
        worksheet.Cell("AA3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("AA3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("AA3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("AA3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("AA3").Style.Alignment.WrapText = true;
        worksheet.Cell("AA3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("AA3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("AA3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("AA3").Style.Border.RightBorderColor = XLColor.White;

        #endregion

        #region Documento de Referencia

        var rangoTitleDocReferencia = worksheet.Range("AB2:AE2");
        rangoTitleDocReferencia.Merge();
        rangoTitleDocReferencia.Value = "DOCUMENTO DE REFERENCIA";
        rangoTitleDocReferencia.Style.Font.Bold = true;
        rangoTitleDocReferencia.Style.Font.FontColor = XLColor.White;
        rangoTitleDocReferencia.Style.Fill.BackgroundColor = XLColor.DarkRed;
        rangoTitleDocReferencia.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        rangoTitleDocReferencia.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        rangoTitleDocReferencia.Style.Border.TopBorderColor = XLColor.White;
        rangoTitleDocReferencia.Style.Border.RightBorder = XLBorderStyleValues.Thin;
        rangoTitleDocReferencia.Style.Border.RightBorderColor = XLColor.White;

        // Fecha de emisión del comprobante de pago o documento original que se modifica (6) o documento referencial al documento que sustenta el crédito fiscal
        worksheet.Cell("AB3").Value = "FECHA DE EMISIÓN";
        worksheet.Cell("AB3").Style.Font.Bold = true;
        worksheet.Cell("AB3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("AB3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("AB3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("AB3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("AB3").Style.Alignment.WrapText = true;
        worksheet.Cell("AB3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("AB3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("AB3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("AB3").Style.Border.RightBorderColor = XLColor.White;

        // Tipo del comprobante de pago que se modifica (6)
        worksheet.Cell("AC3").Value = "TIPO";
        worksheet.Cell("AC3").Style.Font.Bold = true;
        worksheet.Cell("AC3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("AC3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("AC3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("AC3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("AC3").Style.Alignment.WrapText = true;
        worksheet.Cell("AC3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("AC3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("AC3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("AC3").Style.Border.RightBorderColor = XLColor.White;

        // Número de serie del comprobante de pago que se modifica (6) o Código de la Dependencia Aduanera
        worksheet.Cell("AD3").Value = "SERIE";
        worksheet.Cell("AD3").Style.Font.Bold = true;
        worksheet.Cell("AD3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("AD3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("AD3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("AD3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("AD3").Style.Alignment.WrapText = true;
        worksheet.Cell("AD3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("AD3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("AD3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("AD3").Style.Border.RightBorderColor = XLColor.White;

        // Número del comprobante de pago que se modifica (6) o Número de la DUA, de corresponder
        worksheet.Cell("AE3").Value = "NÚMERO";
        worksheet.Cell("AE3").Style.Font.Bold = true;
        worksheet.Cell("AE3").Style.Font.FontColor = XLColor.White;
        worksheet.Cell("AE3").Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell("AE3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell("AE3").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell("AE3").Style.Alignment.WrapText = true;
        worksheet.Cell("AE3").Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("AE3").Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell("AE3").Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell("AE3").Style.Border.RightBorderColor = XLColor.White;

        #endregion

        // Identificación del Contrato o del proyecto en el caso de los Operadores de las sociedades irregulares, consorcios, joint ventures u otras formas de contratos de colaboración empresarial, que no lleven contabilidad independiente.
        var rangoContratoSociedades = worksheet.Range("AF2:AF3");
        rangoContratoSociedades.Merge();
        rangoContratoSociedades.Value = "IDENTIFICACIÓN\nDEL CONTRATO DE\nLAS SOCIEDADES";
        rangoContratoSociedades.Style.Font.Bold = true;
        rangoContratoSociedades.Style.Font.FontColor = XLColor.White;
        rangoContratoSociedades.Style.Fill.BackgroundColor = XLColor.DarkRed;
        rangoContratoSociedades.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        rangoContratoSociedades.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        rangoContratoSociedades.Style.Alignment.WrapText = true;
        rangoContratoSociedades.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        rangoContratoSociedades.Style.Border.TopBorderColor = XLColor.White;
        rangoContratoSociedades.Style.Border.RightBorder = XLBorderStyleValues.Thin;
        rangoContratoSociedades.Style.Border.RightBorderColor = XLColor.White;

        // Error tipo 1: inconsistencia en el tipo de cambio
        var rangoErrorTipo1 = worksheet.Range("AG2:AG3");
        rangoErrorTipo1.Merge();
        rangoErrorTipo1.Value = "ERROR TIPO 1";
        rangoErrorTipo1.Style.Font.Bold = true;
        rangoErrorTipo1.Style.Font.FontColor = XLColor.White;
        rangoErrorTipo1.Style.Fill.BackgroundColor = XLColor.DarkRed;
        rangoErrorTipo1.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        rangoErrorTipo1.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        rangoErrorTipo1.Style.Alignment.WrapText = true;
        rangoErrorTipo1.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        rangoErrorTipo1.Style.Border.TopBorderColor = XLColor.White;
        rangoErrorTipo1.Style.Border.RightBorder = XLBorderStyleValues.Thin;
        rangoErrorTipo1.Style.Border.RightBorderColor = XLColor.White;

        // Indicador de Comprobantes de pago cancelados con medios de pago
        var rangoIndicadorMediosPago = worksheet.Range("AH2:AH3");
        rangoIndicadorMediosPago.Merge();
        rangoIndicadorMediosPago.Value = "COMPROBANTES\nCANCELADOS CON\nMEDIOS DE PAGO";
        rangoIndicadorMediosPago.Style.Font.Bold = true;
        rangoIndicadorMediosPago.Style.Font.FontColor = XLColor.White;
        rangoIndicadorMediosPago.Style.Fill.BackgroundColor = XLColor.DarkRed;
        rangoIndicadorMediosPago.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        rangoIndicadorMediosPago.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        rangoIndicadorMediosPago.Style.Alignment.WrapText = true;
        rangoIndicadorMediosPago.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        rangoIndicadorMediosPago.Style.Border.TopBorderColor = XLColor.White;
        rangoIndicadorMediosPago.Style.Border.RightBorder = XLBorderStyleValues.Thin;
        rangoIndicadorMediosPago.Style.Border.RightBorderColor = XLColor.White;

        // Estado que identifica la oportunidad de la anotación o indicación si ésta corresponde a alguna de las situaciones previstas en el inciso e) del artículo 8° de la Resolución de Superintendencia N.° 286-2009/SUNAT
        var rangoEstado = worksheet.Range("AI2:AI3");
        rangoEstado.Merge();
        rangoEstado.Value = "ESTADO";
        rangoEstado.Style.Font.Bold = true;
        rangoEstado.Style.Font.FontColor = XLColor.White;
        rangoEstado.Style.Fill.BackgroundColor = XLColor.Black;
        rangoEstado.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        rangoEstado.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        rangoEstado.Style.Alignment.WrapText = true;
        rangoEstado.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        rangoEstado.Style.Border.TopBorderColor = XLColor.White;
        rangoEstado.Style.Border.RightBorder = XLBorderStyleValues.Thin;
        rangoEstado.Style.Border.RightBorderColor = XLColor.White;

        #endregion

        #region Contenido

        int contador = 4;
        // generar registro de facturas.
        _invoiceSeries.ForEach(serieComprobante =>
        {
            var facturas = GetFacturas(serieComprobante.Id);
            foreach (var item in facturas)
            {
                // Fecha de emisión del Comprobante de Pago
                DateTime fechaEmisión =
                    DateTime.ParseExact(item.FecEmision, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                worksheet.Cell(contador, 4).Style.NumberFormat.Format = "@";
                worksheet.Cell(contador, 4).Value = fechaEmisión.ToString("dd/MM/yyyy");
                // Tipo de Comprobante de Pago o Documento
                worksheet.Cell(contador, 6).Style.NumberFormat.Format = "@";
                worksheet.Cell(contador, 6).Value = "01";
                // Número serie del comprobante de pago o documento o número de serie de la maquina registradora
                worksheet.Cell(contador, 7).Value = item.Serie;
                // Número del comprobante de pago o documento.
                worksheet.Cell(contador, 8).Style.NumberFormat.Format = "@";
                worksheet.Cell(contador, 8).Value = item.Number;
                // Tipo de Documento de Identidad del cliente
                worksheet.Cell(contador, 10).Style.NumberFormat.Format = "@";
                worksheet.Cell(contador, 10).Value = item.TipDocUsuario.Split(":")[0];
                // Número de Documento de Identidad del cliente
                worksheet.Cell(contador, 11).Style.NumberFormat.Format = "@";
                worksheet.Cell(contador, 11).Value = item.NumDocUsuario;
                // Apellidos y nombres, denominación o razón social  del cliente.
                if (item.RznSocialUsuario.Length >= 100)
                    worksheet.Cell(contador, 12).Value = item.RznSocialUsuario.Substring(0, 99);
                else
                    worksheet.Cell(contador, 12).Value = item.RznSocialUsuario;
                // Base imponible de la operación gravada (4)
                var tributos = GetTributos(item.Id);
                var igvItem = tributos.Single(x => x.IdeTributo.Equals("1000"));
                worksheet.Cell(contador, 14).Style.NumberFormat.Format = "###########0.00";
                worksheet.Cell(contador, 14).Value = igvItem.MtoBaseImponible;
                // Impuesto General a las Ventas y/o Impuesto de Promoción Municipal
                worksheet.Cell(contador, 16).Style.NumberFormat.Format = "###########0.00";
                worksheet.Cell(contador, 16).Value = igvItem.MtoTributo;
                // Impuesto al Consumo de las Bolsas de Plástico.
                var bolsaItem = tributos.SingleOrDefault(x => x.IdeTributo.Equals("7152"));
                worksheet.Cell(contador, 23).Style.NumberFormat.Format = "###########0.00";
                if (bolsaItem == null)
                    worksheet.Cell(contador, 23).Value = 0;
                else
                    worksheet.Cell(contador, 23).Value = bolsaItem.MtoTributo;
                // Importe total del comprobante de pago
                worksheet.Cell(contador, 25).Style.NumberFormat.Format = "###########0.00";
                worksheet.Cell(contador, 25).Value = item.SumImpVenta;
                // Código  de la Moneda (Tabla 4)
                worksheet.Cell(contador, 26).Value = item.TipMoneda;
                // Tipo de cambio (5)
                worksheet.Cell(contador, 27).Style.NumberFormat.Format = "#.000";
                worksheet.Cell(contador, 27).Value = 1;
                // Estado que identifica la oportunidad de la anotación o indicación si ésta corresponde a alguna de las situaciones previstas en el inciso e) del artículo 8° de la Resolución de Superintendencia N.° 286-2009/SUNAT
                worksheet.Cell(contador, 35).Value = 1;
                contador++;
            }
        });

        // generar registro de boletas.
        _invoiceSeries.ForEach(serieComprobante =>
        {
            contador += 2;
            var boletas = GetBoletas(serieComprobante.Id);
            foreach (var item in boletas)
            {
                // Fecha de emisión del Comprobante de Pago
                DateTime fechaEmisión =
                    DateTime.ParseExact(item.FecEmision, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                worksheet.Cell(contador, 4).Style.NumberFormat.Format = "@";
                worksheet.Cell(contador, 4).Value = fechaEmisión.ToString("dd/MM/yyyy");
                // Tipo de Comprobante de Pago o Documento
                worksheet.Cell(contador, 6).Style.NumberFormat.Format = "@";
                worksheet.Cell(contador, 6).Value = "03";
                // Número serie del comprobante de pago o documento o número de serie de la maquina registradora
                worksheet.Cell(contador, 7).Value = item.Serie;
                // Número del comprobante de pago o documento.
                worksheet.Cell(contador, 8).Style.NumberFormat.Format = "@";
                worksheet.Cell(contador, 8).Value = item.Number;
                // Tipo de Documento de Identidad del cliente
                worksheet.Cell(contador, 10).Style.NumberFormat.Format = "@";
                worksheet.Cell(contador, 10).Value = item.TipDocUsuario.Split(":")[0];
                // Número de Documento de Identidad del cliente
                worksheet.Cell(contador, 11).Style.NumberFormat.Format = "@";
                worksheet.Cell(contador, 11).Value = item.NumDocUsuario;
                // Apellidos y nombres, denominación o razón social  del cliente.
                if (item.RznSocialUsuario.Length >= 100)
                    worksheet.Cell(contador, 12).Value = item.RznSocialUsuario.Substring(0, 99);
                else
                    worksheet.Cell(contador, 12).Value = item.RznSocialUsuario;
                // Base imponible de la operación gravada (4)
                var tributos = GetTributos(item.Id);
                var igvItem = tributos.Single(x => x.IdeTributo.Equals("1000"));
                worksheet.Cell(contador, 14).Style.NumberFormat.Format = "###########0.00";
                worksheet.Cell(contador, 14).Value = igvItem.MtoBaseImponible;
                // Impuesto General a las Ventas y/o Impuesto de Promoción Municipal
                worksheet.Cell(contador, 16).Style.NumberFormat.Format = "###########0.00";
                worksheet.Cell(contador, 16).Value = igvItem.MtoTributo;
                // Impuesto al Consumo de las Bolsas de Plástico.
                var bolsaItem = tributos.SingleOrDefault(x => x.IdeTributo.Equals("7152"));
                worksheet.Cell(contador, 23).Style.NumberFormat.Format = "###########0.00";
                if (bolsaItem == null)
                    worksheet.Cell(contador, 23).Value = 0;
                else
                    worksheet.Cell(contador, 23).Value = bolsaItem.MtoTributo;
                // Importe total del comprobante de pago
                worksheet.Cell(contador, 25).Style.NumberFormat.Format = "###########0.00";
                worksheet.Cell(contador, 25).Value = item.SumImpVenta;
                // Código  de la Moneda (Tabla 4)
                worksheet.Cell(contador, 26).Value = item.TipMoneda;
                // Tipo de cambio (5)
                worksheet.Cell(contador, 27).Style.NumberFormat.Format = "#.000";
                worksheet.Cell(contador, 27).Value = 1;
                // Estado que identifica la oportunidad de la anotación o indicación si ésta corresponde a alguna de las situaciones previstas en el inciso e) del artículo 8° de la Resolución de Superintendencia N.° 286-2009/SUNAT
                worksheet.Cell(contador, 35).Value = 1;
                contador++;
            }
        });

        #endregion

        #region Ancho y Alto de filas y columnas

        worksheet.Row(3).Height = 50;
        worksheet.Column("A").Width = 12;
        worksheet.Column("B").Width = 12;
        worksheet.Column("C").Width = 12;
        worksheet.Column("D").Width = 12;
        worksheet.Column("E").Width = 12;
        worksheet.Column("F").Width = 5;
        worksheet.Column("G").Width = 12;
        worksheet.Column("H").Width = 12;
        worksheet.Column("I").Width = 12;
        worksheet.Column("J").Width = 5;
        worksheet.Column("K").Width = 12;
        worksheet.Column("L").Width = 50;
        worksheet.Column("M").Width = 12;
        worksheet.Column("N").Width = 12;
        worksheet.Column("O").Width = 12;
        worksheet.Column("P").Width = 12;
        worksheet.Column("Q").Width = 12;
        worksheet.Column("R").Width = 12;
        worksheet.Column("S").Width = 12;
        worksheet.Column("T").Width = 12;
        worksheet.Column("U").Width = 12;
        worksheet.Column("V").Width = 12;
        worksheet.Column("W").Width = 12;
        worksheet.Column("X").Width = 12;
        worksheet.Column("Y").Width = 12;
        worksheet.Column("Z").Width = 12;
        worksheet.Column("AA").Width = 12;
        worksheet.Column("AB").Width = 12;
        worksheet.Column("AC").Width = 5;
        worksheet.Column("AD").Width = 12;
        worksheet.Column("AE").Width = 12;
        worksheet.Column("AF").Width = 15;
        worksheet.Column("AG").Width = 12;
        worksheet.Column("AH").Width = 15;
        worksheet.Column("AI").Width = 12;

        #endregion

        workbook.SaveAs(filePath);
        return filePath;
    }

    private List<InvoiceSale> GetFacturas(string invoiceSerieId)
    {
        return _invoiceSales.Where(x => x.DocType == "FACTURA" && x.InvoiceSerieId == invoiceSerieId)
            .OrderBy(x => x.Number).ToList();
    }

    private List<InvoiceSale> GetBoletas(string invoiceSerieId)
    {
        return _invoiceSales.Where(x => x.DocType == "BOLETA" && x.InvoiceSerieId == invoiceSerieId)
            .OrderBy(x => x.Number).ToList();
    }

    private List<TributoSale> GetTributos(string id) =>
        _tributoSales.Where(x => x.InvoiceSale.Equals(id)).ToList();
}

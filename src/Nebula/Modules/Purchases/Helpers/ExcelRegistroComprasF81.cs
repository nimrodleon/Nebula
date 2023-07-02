using ClosedXML.Excel;
using Nebula.Modules.Purchases.Models;
using System.Globalization;

namespace Nebula.Modules.Purchases.Helpers;

public class ExcelRegistroComprasF81
{
    private readonly List<PurchaseInvoice> _purchases = new List<PurchaseInvoice>();

    public ExcelRegistroComprasF81(List<PurchaseInvoice> purchases)
    {
        _purchases = purchases;
    }

    public string CrearArchivo()
    {
        string fileName = Guid.NewGuid().ToString();
        string filePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"{fileName}.xlsx");
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Registro de Compras - F8.1");

        worksheet.Style.Font.FontSize = 8;
        worksheet.Style.Font.FontName = "Arial Narrow";

        #region Cabecera

        for (int i = 1; i <= 42; i++)
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
        var cabeceraFila1 = worksheet.Range("B1:AP1");
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
        ColumnTitleDarkRed(ref rangoCuoOperación, "CÓDIGO ÚNICO\n DE LA OPERACIÓN \n(CUO)", true);

        // Número correlativo del Cuo.
        var rangoNumCorrelativoCuo = worksheet.Range("C2:C3");
        ColumnTitleDarkRed(ref rangoNumCorrelativoCuo, "NUMERO\nCORRELATIVO\nDEL (CUO)", true);

        #endregion

        #region Comprobantes de Pago o Documentos

        var rangoTitleComprobantesDePago = worksheet.Range("D2:J2");
        GroupTitleDarkRed(ref rangoTitleComprobantesDePago, "COMPROBANTE DE PAGO O DOCUMENTO");
        // fecha de emisión.
        ColumnTitleDarkRed(ref worksheet, "D3", "FECHA DE\nEMISIÓN");
        // fechas de vencimiento.
        ColumnTitleDarkRed(ref worksheet, "E3", "FECHA DE\nVENCIMIENTO");
        // tipo.
        ColumnTitleDarkRed(ref worksheet, "F3", "TIPO\nTABLA 10");
        // serie comprobante.
        ColumnTitleDarkRed(ref worksheet, "G3", "SERIE");
        // serie comprobante.
        ColumnTitleDarkRed(ref worksheet, "H3", "AÑO\nDUA O\nDSI");
        // número comprobante.
        ColumnTitleDarkRed(ref worksheet, "I3", "NÚMERO");
        // numero final de los comprobantes.
        ColumnTitleDarkRed(ref worksheet, "J3", "NÚMERO FINAL");

        #endregion

        #region Datos del Proveedor

        var rangoTitleProveedor = worksheet.Range("K2:M2");
        GroupTitleDarkRed(ref rangoTitleProveedor, "PROVEEDOR");
        // Tipo de Documento de Identidad del cliente
        ColumnTitleDarkRed(ref worksheet, "K3", "TIPO\nTABLA 2");
        // Número de Documento de Identidad del cliente
        ColumnTitleDarkRed(ref worksheet, "L3", "NÚMERO");
        // Apellidos y nombres, denominación o razón social  del cliente.
        ColumnTitleDarkRed(ref worksheet, "M3", "APELLIDOS Y NOMBRES O RAZÓN SOCIAL DEL PROVEEDOR");

        #endregion

        #region Importes del Comprobante de pago o documento

        var rangoTitleImportes = worksheet.Range("N2:X2");
        GroupTitleDarkRed(ref rangoTitleImportes, "IMPORTES DEL COMPROBANTE DE PAGO O DOCUMENTO");
        // Base imponible de las adquisiciones gravadas que dan derecho a crédito fiscal.
        ColumnTitleDarkRed(ref worksheet, "N3", "BASE IMPONIBLE\nOP G Y EXP");
        // Monto del Impuesto General a las Ventas y/o Impuesto de Promoción Municipal.
        ColumnTitleDarkRed(ref worksheet, "O3", "IGV");
        // Base imponible de las adquisiciones gravadas que dan derecho a crédito fiscal.
        ColumnTitleDarkRed(ref worksheet, "P3", "BASE IMPONIBLE\nOP G Y EXP - OP\nNG");
        // Monto del Impuesto General a las Ventas y/o Impuesto de Promoción Municipal.
        ColumnTitleDarkRed(ref worksheet, "Q3", "IGV");
        // Base imponible de las adquisiciones gravadas que no dan derecho a crédito fiscal.
        ColumnTitleDarkRed(ref worksheet, "R3", "BASE IMPONIBLE\nOP NG");
        // Monto del Impuesto General a las Ventas y/o Impuesto de Promoción Municipal.
        ColumnTitleDarkRed(ref worksheet, "S3", "IGV");
        // Valor de las adquisiciones no gravadas.
        ColumnTitleDarkRed(ref worksheet, "T3", "NO\nGRAVADO");
        // Monto del Impuesto Selectivo al Consumo en los casos en que el sujeto pueda utilizarlo como deducción.
        ColumnTitleDarkRed(ref worksheet, "U3", "I.S.C");
        // Impuesto al Consumo de las Bolsas de Plástico.
        ColumnTitleDarkRed(ref worksheet, "V3", "ICBPER");
        // Otros conceptos, tributos y cargos que no formen parte de la base imponible.
        ColumnTitleDarkRed(ref worksheet, "W3", "OTROS\nCARGOS");
        // Importe total de las adquisiciones registradas según comprobante de pago.
        ColumnTitleDarkRed(ref worksheet, "X3", "IMPORTE\nTOTAL");

        #endregion

        #region Moneda

        var rangoTitleMoneda = worksheet.Range("Y2:Z2");
        GroupTitleDarkRed(ref rangoTitleMoneda, "MONEDA");
        // Código de la Moneda (Tabla 4).
        ColumnTitleDarkRed(ref worksheet, "Y3", "CÓDIGO\nMONEDA\nTABLA 4");
        // Tipo de cambio (3).
        ColumnTitleDarkRed(ref worksheet, "Z3", "TIPO\nDE\nCAMBIO");

        #endregion

        #region Documento de Referencia

        var rangoTitleDocReferencia = worksheet.Range("AA2:AE2");
        GroupTitleDarkRed(ref rangoTitleDocReferencia, "DOCUMENTO DE REFERENCIA");
        // Fecha de emisión del comprobante de pago que se modifica(4).
        ColumnTitleDarkRed(ref worksheet, "AA3", "FECHA DE\nEMISIÓN");
        // Tipo de comprobante de pago que se modifica (4).
        ColumnTitleDarkRed(ref worksheet, "AB3", "TIPO\nTABLA 10");
        // Número de serie del comprobante de pago que se modifica(4).
        ColumnTitleDarkRed(ref worksheet, "AC3", "SERIE");
        // Código de la dependencia Aduanera de la Declaración Única de Aduanas(DUA) o de la Declaración Simplificada de Importación(DSI).
        ColumnTitleDarkRed(ref worksheet, "AD3", "CÓDIGO\nTABLA 11");
        // Número del comprobante de pago que se modifica (4). 
        ColumnTitleDarkRed(ref worksheet, "AE3", "NÚMERO");

        #endregion

        #region Detracción

        var rangoTitleDetraccion = worksheet.Range("AF2:AG2");
        GroupTitleDarkRed(ref rangoTitleDetraccion, "DETRACCIÓN");
        // Fecha de emisión de la Constancia de Depósito de Detracción(5)
        ColumnTitleDarkRed(ref worksheet, "AF3", "FECHA DE\nEMISIÓN");
        // Número de la Constancia de Depósito de Detracción (5)
        ColumnTitleDarkRed(ref worksheet, "AG3", "NÚMERO\nDE LA\nCONSTANCIA");

        #endregion

        // Marca del comprobante de pago sujeto a retención
        var rangoRetencion = worksheet.Range("AH2:AH3");
        ColumnTitleDarkRed(ref rangoRetencion, "RETENCIÓN");

        // Clasificación de los bienes y servicios adquiridos (Tabla 30)
        var rangoClasificacion = worksheet.Range("AI2:AI3");
        ColumnTitleDarkRed(ref rangoClasificacion, "CLASIFICACIÓN\nDE BIENES Y\nSERVICIOS\nTABLA 30");

        // Identificación del Contrato o del proyecto en el caso de los Operadores de las sociedades irregulares,
        // consorcios, joint ventures u otras formas de contratos de colaboración empresarial, que no lleven contabilidad independiente.
        var rangoContrato = worksheet.Range("AJ2:AJ3");
        ColumnTitleDarkRed(ref rangoContrato, "IDENTIFICACIÓN\nDEL CONTRATO\nDE LAS\nSOCIEDADES");

        // Error tipo 1: inconsistencia en el tipo de cambio.
        var rangoError1 = worksheet.Range("AK2:AK3");
        ColumnTitleDarkRed(ref rangoError1, "ERROR\nTIPO 1");

        // Error tipo 2: inconsistencia por proveedores no habidos.
        var rangoError2 = worksheet.Range("AL2:AL3");
        ColumnTitleDarkRed(ref rangoError2, "ERROR\nTIPO 2");

        // Error tipo 3: inconsistencia por proveedores que renunciaron a la exoneración del Apéndice I del IGV.
        var rangoError3 = worksheet.Range("AM2:AM3");
        ColumnTitleDarkRed(ref rangoError3, "ERROR\nTIPO 3");

        // Error tipo 4: inconsistencia por DNIs que fueron utilizados en las Liquidaciones de Compra y que ya cuentan con RUC.
        var rangoError4 = worksheet.Range("AN2:AN3");
        ColumnTitleDarkRed(ref rangoError4, "ERROR\nTIPO 4");

        // Indicador de Comprobantes de pago cancelados con medios de pago.
        var rangoIndicadorComprobantes = worksheet.Range("AO2:AO3");
        ColumnTitleDarkRed(ref rangoIndicadorComprobantes, "COMPROBANTES\nCANCELADOS\nCON MEDIOS DE\nPAGO");

        // Estado que identifica la oportunidad de la anotación o indicación si ésta corresponde a un ajuste.
        var rangoEstado = worksheet.Range("AP2:AP3");
        ColumnTitleDarkRed(ref rangoEstado, "ESTADO", true);

        int contador = 4;
        // generar registro de facturas.
        _purchases.ForEach(item =>
        {
            // 4.- Fecha de emisión del comprobante de pago o documento.
            DateTime fechaEmision = DateTime.ParseExact(item.FecEmision, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            worksheet.Cell(contador, 4).Style.NumberFormat.Format = "@";
            worksheet.Cell(contador, 4).Value = fechaEmision.ToString("dd/MM/yyyy");
            // 6.- Tipo de Comprobante de Pago o Documento.
            string tipoDocu = string.Empty;
            if (item.DocType.Equals("FACTURA")) tipoDocu = "01";
            if (item.DocType.Equals("BOLETA")) tipoDocu = "03";
            worksheet.Cell(contador, 6).Style.NumberFormat.Format = "@";
            worksheet.Cell(contador, 6).Value = tipoDocu;
            // 7.- Serie del comprobante de pago o documento.
            worksheet.Cell(contador, 7).Value = item.Serie;
            // 9.- Número del comprobante de pago o documento.
            worksheet.Cell(contador, 9).Style.NumberFormat.Format = "@";
            worksheet.Cell(contador, 9).Value = item.Number;
            // 11.- Tipo de Documento de Identidad del proveedor.
            worksheet.Cell(contador, 11).Style.NumberFormat.Format = "@";
            worksheet.Cell(contador, 11).Value = item.TipDocProveedor.Split(":")[0].Trim();
            // 12.- Número de RUC del proveedor o número de documento de Identidad.
            worksheet.Cell(contador, 12).Style.NumberFormat.Format = "@";
            worksheet.Cell(contador, 12).Value = item.NumDocProveedor;
            // 13.- Apellidos y nombres, denominación o razón social  del proveedor.
            if (item.RznSocialProveedor.Length >= 100)
                worksheet.Cell(contador, 13).Value = item.RznSocialProveedor.Substring(0, 99);
            else
                worksheet.Cell(contador, 13).Value = item.RznSocialProveedor;
            // 14.- Base imponible de las adquisiciones gravadas que dan derecho a crédito fiscal.
            // 15.- Monto del Impuesto General a las Ventas y/o Impuesto de Promoción Municipal.
            // 22.- Impuesto al Consumo de las Bolsas de Plástico.

            // 24.- Importe total de las adquisiciones registradas según comprobante de pago.
            worksheet.Cell(contador, 24).Style.NumberFormat.Format = "###########0.00";
            worksheet.Cell(contador, 24).Value = item.SumImpCompra;

            // 25.- Código de la Moneda(Tabla 4).
            worksheet.Cell(contador, 25).Value = item.TipMoneda;
            // 26.- Tipo de cambio (3).
            worksheet.Cell(contador, 26).Style.NumberFormat.Format = "#.000";
            worksheet.Cell(contador, 26).Value = item.TipoCambio;
            // 35.- Clasificación de los bienes y servicios adquiridos(Tabla 30).
            worksheet.Cell(contador, 35).Style.NumberFormat.Format = "@";
            worksheet.Cell(contador, 35).Value = "1";
            // 42.- Estado que identifica la oportunidad de la anotación o indicación si ésta corresponde a un ajuste.
            worksheet.Cell(contador, 42).Value = "1";

            contador++;
        });

        #region Ancho y Alto de filas y columnas

        worksheet.Row(3).Height = 50;
        worksheet.Column("A").Width = 12;
        worksheet.Column("B").Width = 12;
        worksheet.Column("C").Width = 12;
        worksheet.Column("D").Width = 12;
        worksheet.Column("E").Width = 12;
        worksheet.Column("F").Width = 12;
        worksheet.Column("G").Width = 12;
        worksheet.Column("H").Width = 12;
        worksheet.Column("I").Width = 12;
        worksheet.Column("J").Width = 12;
        worksheet.Column("K").Width = 12;
        worksheet.Column("L").Width = 12;
        worksheet.Column("M").Width = 70;
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
        worksheet.Column("AC").Width = 12;
        worksheet.Column("AD").Width = 12;
        worksheet.Column("AE").Width = 12;
        worksheet.Column("AF").Width = 15;
        worksheet.Column("AG").Width = 12;
        worksheet.Column("AH").Width = 15;
        worksheet.Column("AI").Width = 12;
        worksheet.Column("AJ").Width = 12;
        worksheet.Column("AK").Width = 12;
        worksheet.Column("AL").Width = 12;
        worksheet.Column("AM").Width = 12;
        worksheet.Column("AN").Width = 12;
        worksheet.Column("AO").Width = 12;
        worksheet.Column("AP").Width = 12;

        #endregion

        workbook.SaveAs(filePath);
        return filePath;
    }

    private void GroupTitleDarkRed(ref IXLRange range, string title)
    {
        range.Merge();
        range.Value = title;
        range.Style.Font.Bold = true;
        range.Style.Font.FontColor = XLColor.White;
        range.Style.Fill.BackgroundColor = XLColor.DarkRed;
        range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        range.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        range.Style.Border.TopBorderColor = XLColor.White;
        range.Style.Border.RightBorder = XLBorderStyleValues.Thin;
        range.Style.Border.RightBorderColor = XLColor.White;
    }

    private void ColumnTitleDarkRed(ref IXLWorksheet worksheet, string cell, string title)
    {
        worksheet.Cell(cell).Value = title;
        worksheet.Cell(cell).Style.Font.Bold = true;
        worksheet.Cell(cell).Style.Font.FontColor = XLColor.White;
        worksheet.Cell(cell).Style.Fill.BackgroundColor = XLColor.DarkRed;
        worksheet.Cell(cell).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell(cell).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Cell(cell).Style.Alignment.WrapText = true;
        worksheet.Cell(cell).Style.Border.TopBorder = XLBorderStyleValues.Thin;
        worksheet.Cell(cell).Style.Border.TopBorderColor = XLColor.White;
        worksheet.Cell(cell).Style.Border.RightBorder = XLBorderStyleValues.Thin;
        worksheet.Cell(cell).Style.Border.RightBorderColor = XLColor.White;
    }

    private void ColumnTitleDarkRed(ref IXLRange range, string title, bool black = false)
    {
        range.Merge();
        range.Value = title;
        range.Style.Font.Bold = true;
        range.Style.Font.FontColor = XLColor.White;
        range.Style.Fill.BackgroundColor = black ? XLColor.Black : XLColor.DarkRed;
        range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        range.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        range.Style.Alignment.WrapText = true;
        range.Style.Border.TopBorder = XLBorderStyleValues.Thin;
        range.Style.Border.TopBorderColor = XLColor.White;
        range.Style.Border.RightBorder = XLBorderStyleValues.Thin;
        range.Style.Border.RightBorderColor = XLColor.White;
    }
}

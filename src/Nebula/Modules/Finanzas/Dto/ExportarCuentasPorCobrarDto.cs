using ClosedXML.Excel;
using Nebula.Modules.Finanzas.Models;

namespace Nebula.Modules.Finanzas.Dto;

public class ExportarCuentasPorCobrarDto
{
    private readonly List<FinancialAccount> _cuentasPorCobrar;

    public ExportarCuentasPorCobrarDto(List<FinancialAccount> cuentasPorCobrar)
    {
        _cuentasPorCobrar = cuentasPorCobrar;
    }

    public string GenerarArchivoExcel()
    {
        string fileName = Guid.NewGuid().ToString();
        string filePath = Path.Combine(Path.GetTempPath(), $"{fileName}.xlsx");
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("DEUDAS");
        // generar cabecera documento.
        worksheet.Cell("A1").Value = "FECHA";
        worksheet.Cell("B1").Value = "COMPROBANTE";
        worksheet.Cell("C1").Value = "OBSERVACIÓN";
        worksheet.Cell("D1").Value = "CARGO";
        worksheet.Cell("E1").Value = "SALDO";
        // formato cabecera documento.
        var formatoCabecera = worksheet.Range("A1:E1");
        formatoCabecera.Style.Font.Bold = true;
        formatoCabecera.Style.Font.FontColor = XLColor.White;
        formatoCabecera.Style.Fill.BackgroundColor = XLColor.DarkBlue;
        // detalle de cuentas por cobrar.
        int contador = 2;
        foreach (var item in _cuentasPorCobrar)
        {
            worksheet.Cell(contador, 1).Value = item.CreatedAt;
            worksheet.Cell(contador, 2).Value = item.Document;
            worksheet.Cell(contador, 3).Value = item.Remark;
            worksheet.Cell(contador, 4).Value = item.Cargo;
            worksheet.Cell(contador, 5).Value = item.Saldo;
            contador++;
        }

        // monto total deuda.
        worksheet.Cell(contador, 5).Value = _cuentasPorCobrar.Sum(x => x.Cargo);
        // ajustar ancho de las columnas.
        worksheet.Columns(1, 5).AdjustToContents();
        workbook.SaveAs(filePath);
        return filePath;
    }
}

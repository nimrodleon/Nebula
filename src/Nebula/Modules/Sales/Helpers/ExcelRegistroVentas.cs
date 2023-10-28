using ClosedXML.Excel;
using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales.Helpers;

class ExcelDatos
{
    public string Fecha { get; set; } = string.Empty;
    public string Serie { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public decimal MontoTotal { get; set; } = decimal.Zero;
}

class ExcelNotas : ExcelDatos
{
    public string NumDocAfectado { get; set; } = string.Empty;
}

public class ExcelRegistroVentas
{
    private readonly List<InvoiceSale> _invoices;
    private readonly List<CreditNote> _creditNotes;

    public ExcelRegistroVentas(List<InvoiceSale> invoices, List<CreditNote> creditNotes)
    {
        _invoices = invoices;
        _creditNotes = creditNotes;
    }

    public XLWorkbook GenerarArchivo()
    {
        var workbook = new XLWorkbook();
        var worksheetDatos = workbook.Worksheets.Add("Datos");

        // Añadimos los datos de boletas.
        worksheetDatos.Cell(1, 1).Value = "Fecha";
        worksheetDatos.Cell(1, 2).Value = "Serie";
        worksheetDatos.Cell(1, 3).Value = "Nro. Boleta";
        worksheetDatos.Cell(1, 4).Value = "Monto";
        int ultimaFilaBol = CargarDatos(ref worksheetDatos, ObtenerDatos("03", false), 2, 1);
        worksheetDatos.Cell(ultimaFilaBol + 1, 1).Value = "Boletas Anuladas";
        CargarDatos(ref worksheetDatos, ObtenerDatos("03", true), ultimaFilaBol + 2, 1);

        // Añadimos los datos de facturas.
        worksheetDatos.Cell(1, 6).Value = "Fecha";
        worksheetDatos.Cell(1, 7).Value = "Serie";
        worksheetDatos.Cell(1, 8).Value = "Nro. Factura";
        worksheetDatos.Cell(1, 9).Value = "Monto";
        int ultimaFilaFact = CargarDatos(ref worksheetDatos, ObtenerDatos("01", false), 2, 6);
        worksheetDatos.Cell(ultimaFilaFact + 1, 6).Value = "Facturas Anuladas";
        CargarDatos(ref worksheetDatos, ObtenerDatos("01", true), ultimaFilaFact + 2, 6);

        // Generar Notas de Crédito.
        var worksheetNotas = workbook.Worksheets.Add("Notas");

        // Añadimos los datos de boletas.
        worksheetNotas.Cell(1, 1).Value = "Fecha";
        worksheetNotas.Cell(1, 2).Value = "Serie";
        worksheetNotas.Cell(1, 3).Value = "NC. Boleta";
        worksheetNotas.Cell(1, 4).Value = "Monto";
        worksheetNotas.Cell(1, 5).Value = "Doc. Afectado";
        CargarNotas(ref worksheetNotas, ObtenerNotas("03"), 2, 1);

        // Añadimos los datos de facturas.
        worksheetNotas.Cell(1, 7).Value = "Fecha";
        worksheetNotas.Cell(1, 8).Value = "Serie";
        worksheetNotas.Cell(1, 9).Value = "NC. Factura";
        worksheetNotas.Cell(1, 10).Value = "Monto";
        worksheetNotas.Cell(1, 11).Value = "Doc. Afectado";
        CargarNotas(ref worksheetNotas, ObtenerNotas("01"), 2, 7);

        return workbook;
    }

    private int CargarDatos(ref IXLWorksheet worksheet, List<ExcelDatos> datos, int row, int column)
    {
        int currentRow = row;
        foreach (var dato in datos)
        {
            worksheet.Cell(currentRow, column).Value = dato.Fecha;
            worksheet.Cell(currentRow, column + 1).Value = dato.Serie;
            worksheet.Cell(currentRow, column + 2).Value = dato.Numero;
            worksheet.Cell(currentRow, column + 3).Value = dato.MontoTotal;
            currentRow++;
        }
        return currentRow;
    }

    private int CargarNotas(ref IXLWorksheet worksheet, List<ExcelNotas> notas, int row, int column)
    {
        int currentRow = row;
        foreach (var nota in notas)
        {
            worksheet.Cell(currentRow, column).Value = nota.Fecha;
            worksheet.Cell(currentRow, column + 1).Value = nota.Serie;
            worksheet.Cell(currentRow, column + 2).Value = nota.Numero;
            worksheet.Cell(currentRow, column + 3).Value = nota.MontoTotal;
            worksheet.Cell(currentRow, column + 4).Value = nota.NumDocAfectado;
            currentRow++;
        }
        return currentRow;
    }

    private List<ExcelDatos> ObtenerDatos(string tipoDoc, bool anulada = false)
    {
        var datos = new List<ExcelDatos>();
        var comprobantes = _invoices.Where(x => x.TipoDoc == tipoDoc && x.Anulada == anulada).ToList();
        comprobantes.ForEach(item =>
        {
            datos.Add(new ExcelDatos()
            {
                Fecha = item.FechaEmision,
                Serie = item.Serie,
                Numero = item.Correlativo,
                MontoTotal = item.MtoImpVenta
            });
        });
        return datos;
    }

    private List<ExcelNotas> ObtenerNotas(string tipoDoc)
    {
        var datos = new List<ExcelNotas>();
        var notas = _creditNotes.Where(x => x.TipDocAfectado == tipoDoc).ToList();
        notas.ForEach(item =>
        {
            datos.Add(new ExcelNotas()
            {
                Fecha = item.FechaEmision,
                Serie = item.Serie,
                Numero = item.Correlativo,
                MontoTotal = item.MtoImpVenta,
                NumDocAfectado = item.NumDocfectado,
            });
        });
        return datos;
    }

}

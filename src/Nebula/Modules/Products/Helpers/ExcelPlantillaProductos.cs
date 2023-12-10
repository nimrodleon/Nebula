using ClosedXML.Excel;

namespace Nebula.Modules.Products.Helpers;

public class ExcelPlantillaProductos
{
    public XLWorkbook GenerarArchivo()
    {
        var workbook = new XLWorkbook();
        var worksheetProductos = workbook.Worksheets.Add("Productos");

        // Añadir datos de cabecera.
        worksheetProductos.Cell(1, 1).Value = "Descripción";
        worksheetProductos.Cell(1, 2).Value = "Precio";
        worksheetProductos.Cell(1, 3).Value = "Tipo Item";
        worksheetProductos.Cell(1, 4).Value = "Unidad Medida";

        return workbook;
    }
}

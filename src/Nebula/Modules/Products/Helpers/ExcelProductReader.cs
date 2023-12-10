using ClosedXML.Excel;
using Nebula.Modules.Products.Models;
using Nebula.Modules.Sales.Helpers;

namespace Nebula.Modules.Products.Helpers;

public class ExcelProductReader
{
    private readonly IFormFile _datos;
    private readonly string _companyId;
    private readonly string _category;

    public ExcelProductReader(IFormFile datos, string companyId, string category)
    {
        _datos = datos;
        _companyId = companyId;
        _category = category;
    }

    public List<Product> ReadProducts()
    {
        var products = new List<Product>();

        using (var stream = _datos.OpenReadStream())
        {
            using (var workbook = new XLWorkbook(stream))
            {
                var worksheet = workbook.Worksheet(1);

                for (int row = 2; row <= worksheet.LastRowUsed().RowNumber(); row++)
                {
                    var descripcion = worksheet.Cell(row, 1).Value.ToString();
                    var precio = decimal.Parse(worksheet.Cell(row, 2).Value.ToString());
                    var tipoItem = worksheet.Cell(row, 3).Value.ToString();
                    var unidadMedida = worksheet.Cell(row, 4).Value.ToString();

                    // Genera el nuevo objeto Product
                    var product = new Product
                    {
                        CompanyId = _companyId.Trim(),
                        Description = descripcion.ToUpper().Trim(),
                        Category = _category.Trim(),
                        IgvSunat = TipoIGV.Gravado,
                        PrecioVentaUnitario = precio,
                        Type = tipoItem.ToUpper().Trim(),
                        UndMedida = unidadMedida.ToUpper().Trim(),
                    };

                    products.Add(product);
                }
            }
        }

        return products;
    }
}

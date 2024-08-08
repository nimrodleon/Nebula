using Nebula.Common.Dto;
using Nebula.Modules.Products.Models;

namespace Nebula.Modules.Products.Dto;

public class ProductSelect : Product, ISelect2
{
    public string Text { get; set; } = string.Empty;
}

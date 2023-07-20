using System.Reflection;

namespace Nebula.Modules.Auth.Helpers;

public static class Permission
{
    /// <summary>
    /// Obtener todos los permisos de la clase.
    /// </summary>
    public static string[] GetAllPermissions()
    {
        var type = typeof(Permission);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

        var permissions = new List<string>();
        foreach (var field in fields)
        {
            var value = field.GetValue(null);
            if (value != null) permissions.Add(value.ToString());
        }

        return permissions.ToArray();
    }

    public const string ProductRead = "Product.Read";
    public const string ProductCreate = "Product.Create";
    public const string ProductEdit = "Product.Edit";
    public const string ProductDelete = "Product.Delete";
    public const string PosRead = "Pos.Read";
    public const string PosCreate = "Pos.Create";
    public const string PosEdit = "Pos.Edit";
    public const string PosDelete = "Pos.Delete";
    public const string ContactRead = "Contact.Read";
    public const string ContactCreate = "Contact.Create";
    public const string ContactEdit = "Contact.Edit";
    public const string ContactDelete = "Contact.Delete";
    public const string TallerRead = "Taller.Read";
    public const string TallerCreate = "Taller.Create";
    public const string TallerEdit = "Taller.Edit";
    public const string TallerDelete = "Taller.Delete";
    public const string MaterialTallerRead = "MaterialTaller.Read";
    public const string MaterialTallerCreate = "MaterialTaller.Create";
    public const string MaterialTallerEdit = "MaterialTaller.Edit";
    public const string MaterialTallerDelete = "MaterialTaller.Delete";
    public const string SalesRead = "Sales.Read";
    public const string SalesCreate = "Sales.Create";
    public const string SalesEdit = "Sales.Edit";
    public const string SalesDelete = "Sales.Delete";
    public const string ReceivableRead = "Receivable.Read";
    public const string ReceivableCreate = "Receivable.Create";
    public const string ReceivableEdit = "Receivable.Edit";
    public const string ReceivableDelete = "Receivable.Delete";
    public const string PurchasesRead = "Purchases.Read";
    public const string PurchasesCreate = "Purchases.Create";
    public const string PurchasesEdit = "Purchases.Edit";
    public const string PurchasesDelete = "Purchases.Delete";
    public const string InventoryRead = "Inventory.Read";
    public const string InventoryCreate = "Inventory.Create";
    public const string InventoryEdit = "Inventory.Edit";
    public const string InventoryDelete = "Inventory.Delete";
    public const string ConfigurationRead = "Configuration.Read";
    public const string ConfigurationCreate = "Configuration.Create";
    public const string ConfigurationEdit = "Configuration.Edit";
    public const string ConfigurationDelete = "Configuration.Delete";
}

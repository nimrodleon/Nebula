using Microsoft.EntityFrameworkCore;
using Nebula.Modules.Account.Models;
using Nebula.Modules.Auth.Models;
using Nebula.Modules.Cashier.Models;
using Nebula.Modules.Contacts.Models;
using Nebula.Modules.Finanzas.Models;
using Nebula.Modules.Inventory.Models;
using Nebula.Modules.Products.Models;
using Nebula.Modules.Sales.Models;
using Nebula.Modules.Taller.Models;

namespace Nebula.Common;

public class ApplicationDbContext : DbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<InvoiceSerie> InvoiceSeries { get; set; }
    public DbSet<PagoSuscripcion> PagosSuscripcion { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }

    public DbSet<Collaborator> Collaborators { get; set; }
    public DbSet<User> Users { get; set; }

    public DbSet<CajaDiaria> CajasDiaria { get; set; }
    public DbSet<CashierDetail> CashierDetails { get; set; }

    public DbSet<Contact> Contacts { get; set; }

    public DbSet<FinancialAccount> FinancialAccounts { get; set; }

    public DbSet<AjusteInventario> AjusteInventarios { get; set; }
    public DbSet<AjusteInventarioDetail> AjusteInventarioDetails { get; set; }
    public DbSet<InventoryNotas> InventoryNotas { get; set; }
    public DbSet<InventoryNotasDetail> InventoryNotasDetails { get; set; }
    public DbSet<Location> Location { get; set; }
    public DbSet<LocationDetail> LocationDetails { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<MaterialDetail> MaterialDetails { get; set; }
    public DbSet<ProductStock> ProductStock { get; set; }
    public DbSet<Transferencia> Transferencias { get; set; }
    public DbSet<TransferenciaDetail> TransferenciaDetails { get; set; }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    public DbSet<CreditNote> CreditNotes { get; set; }
    public DbSet<CreditNoteDetail> CreditNoteDetails { get; set; }
    public DbSet<Cuota> Cuotas { get; set; }
    public DbSet<InvoiceSale> InvoiceSales { get; set; }
    public DbSet<InvoiceSaleDetail> InvoiceSaleDetails { get; set; }

    public DbSet<TallerRepairOrder> TallerRepairOrders { get; set; }
    public DbSet<TallerItemRepairOrder> TallerItemRepairOrders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}

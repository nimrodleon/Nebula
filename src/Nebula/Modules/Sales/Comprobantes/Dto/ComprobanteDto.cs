using Nebula.Modules.Account.Models;
using Nebula.Modules.Sales.Helpers;
using Nebula.Modules.Sales.Models;

namespace Nebula.Modules.Sales.Comprobantes.Dto;

public class ComprobanteDto
{
    #region ORIGIN_HTTP_REQUEST!
    public CabeceraComprobanteDto Cabecera { get; set; } = new CabeceraComprobanteDto();
    public List<ItemComprobanteDto> Detalle { get; set; } = new List<ItemComprobanteDto>();
    public PaymentTerms FormaPago { get; set; } = new PaymentTerms();
    public List<Cuota> Cuotas { get; set; } = new List<Cuota>();
    #endregion
    private decimal _mtoOperGravadas;
    private decimal _mtoOperInafectas;
    private decimal _mtoOperExoneradas;
    private decimal _mtoIGV;
    private decimal _totalImpuestos;
    private decimal _valorVenta;
    private decimal _subTotal;
    private decimal _mtoImpVenta;

    private void CalcularTotales(Company company)
    {
        _mtoOperGravadas = 0;
        _mtoOperInafectas = 0;
        _mtoOperExoneradas = 0;
        _mtoIGV = 0;
        _totalImpuestos = 0;
        _valorVenta = 0;
        _subTotal = 0;
        _mtoImpVenta = 0;

        Detalle.ForEach(item =>
        {
            if (item.IgvSunat == "10") _mtoOperGravadas += item.GetMtoBaseIgv(company);
            if (item.IgvSunat == "20") _mtoOperExoneradas += item.GetMtoBaseIgv(company);
            if (item.IgvSunat == "30") _mtoOperInafectas += item.GetMtoBaseIgv(company);
            _mtoIGV += item.GetIgv(company);
            _totalImpuestos += item.GetIgv(company); // IGV + ICBPER + ...
            _valorVenta += item.GetMtoValorVenta(company);
            _subTotal = (item.GetMtoValorVenta(company) + item.GetIgv(company)) + _subTotal;
            _mtoImpVenta = (item.GetMtoValorVenta(company) + _totalImpuestos) + _mtoImpVenta;
        });
    }

    public InvoiceSale GetInvoiceSale(Company company)
    {
        CalcularTotales(company);
        return new InvoiceSale
        {
            CompanyId = company.Id,
            InvoiceSerieId = Cabecera.InvoiceSerieId.Trim(),
            TipoDoc = Cabecera.TipoDoc.Trim(),
            FechaEmision = DateTime.Now.ToString("yyyy-MM-dd"),
            ContactId = Cabecera.ContactId.Trim(),
            Cliente = new Cliente()
            {
                TipoDoc = Cabecera.TipDocUsuario.Trim(),
                NumDoc = Cabecera.NumDocUsuario.Trim(),
                RznSocial = Cabecera.RznSocialUsuario.Trim(),
            },
            TipoMoneda = company.TipMoneda.Trim(),
            TipoOperacion = "0101",
            FecVencimiento = Cabecera.FecVencimiento.Trim(),
            FormaPago = FormaPago,
            Cuotas = Cuotas,
            MtoOperGravadas = _mtoOperGravadas,
            MtoOperInafectas = _mtoOperInafectas,
            MtoOperExoneradas = _mtoOperExoneradas,
            MtoIGV = _mtoIGV,
            TotalImpuestos = _totalImpuestos,
            ValorVenta = _valorVenta,
            SubTotal = _subTotal,
            MtoImpVenta = _mtoImpVenta,
            TotalEnLetras = new NumberToLetters(_mtoImpVenta).ToString(),
            Year = DateTime.Now.ToString("yyyy"),
            Month = DateTime.Now.ToString("MM"),
            Remark = Cabecera.Remark.Trim(),
            Anulada = false,
        };
    }

    public List<InvoiceSaleDetail> GetInvoiceSaleDetails(Company company, string invoiceId)
    {
        var invoiceSaleDetails = new List<InvoiceSaleDetail>();
        Detalle.ForEach(item =>
        {
            // Tributo: Afectación al IGV por ítem.
            string tipAfeIgv = "10";
            switch (item.IgvSunat)
            {
                case TipoIGV.Gravado:
                    tipAfeIgv = "10";
                    break;
                case TipoIGV.Exonerado:
                    tipAfeIgv = "20";
                    break;
                case TipoIGV.Inafecto:
                    tipAfeIgv = "30";
                    break;
            }

            invoiceSaleDetails.Add(new InvoiceSaleDetail()
            {
                CompanyId = company.Id,
                InvoiceSaleId = invoiceId,
                CajaDiariaId = Cabecera.CajaDiariaId,
                WarehouseId = item.WarehouseId.Trim(),
                TipoItem = item.TipoItem.Trim(),
                Unidad = item.CodUnidadMedida.Trim(),
                Cantidad = item.CtdUnidadItem,
                CodProducto = item.ProductId.Trim(),
                Description = item.Description.Trim(),
                MtoValorUnitario = item.GetMtoValorUnitario(company),
                MtoBaseIgv = item.GetMtoBaseIgv(company),
                PorcentajeIgv = item.IgvSunat == TipoIGV.Gravado ? company.PorcentajeIgv : 0,
                Igv = item.GetIgv(company),
                TipAfeIgv = tipAfeIgv,
                TotalImpuestos = item.GetIgv(company), // IGV + ICBPER + ...
                MtoPrecioUnitario = item.MtoPrecioVentaUnitario,
                MtoValorVenta = item.GetMtoValorVenta(company),
                RecordType = item.RecordType.Trim(),
                Year = DateTime.Now.ToString("yyyy"),
                Month = DateTime.Now.ToString("MM"),
            });
        });
        return invoiceSaleDetails;
    }

    public void GenerarSerieComprobante(ref InvoiceSerie invoiceSerie, ref InvoiceSale invoiceSale)
    {
        int numComprobante = 0;
        string THROW_MESSAGE = "Ingresa serie de comprobante!";
        switch (invoiceSale.TipoDoc)
        {
            case "01":
                invoiceSale.Serie = invoiceSerie.Factura;
                numComprobante = invoiceSerie.CounterFactura;
                if (numComprobante > 99999999)
                    throw new Exception(THROW_MESSAGE);
                numComprobante += 1;
                invoiceSerie.CounterFactura = numComprobante;
                break;
            case "03":
                invoiceSale.Serie = invoiceSerie.Boleta;
                numComprobante = invoiceSerie.CounterBoleta;
                if (numComprobante > 99999999)
                    throw new Exception(THROW_MESSAGE);
                numComprobante += 1;
                invoiceSerie.CounterBoleta = numComprobante;
                break;
            case "NOTA":
                invoiceSale.Serie = invoiceSerie.NotaDeVenta;
                numComprobante = invoiceSerie.CounterNotaDeVenta;
                if (numComprobante > 99999999)
                    throw new Exception(THROW_MESSAGE);
                numComprobante += 1;
                invoiceSerie.CounterNotaDeVenta = numComprobante;
                break;
        }

        invoiceSale.Correlativo = numComprobante.ToString();
    }
}

﻿using System;
using System.Collections.Generic;

namespace Nebula.Data.ViewModels
{
    /// <summary>
    /// modelo para el punto de venta.
    /// </summary>
    public class Sale
    {
        public int ClientId { get; set; }
        public string PaymentType { get; set; }
        public string DocType { get; set; }
        public DateTime EndDate { get; set; }
        public bool MontoCaja { get; set; }
        public decimal? MontoTotal { get; set; }
        public decimal? Vuelto { get; set; }
        public decimal? SumTotValVenta { get; set; }
        public decimal? SumTotTributos { get; set; }
        public decimal? SumImpVenta { get; set; }
        public string Remark { get; set; }
        public List<SaleDetail> Details { get; set; }
        public List<Cuota> Cuotas { get; set; }
    }
}

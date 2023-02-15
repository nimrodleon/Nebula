﻿using Nebula.Database.Models.Common;
using Nebula.Database.Models.Sales;

namespace Nebula.Database.Dto.Sales;

public class TicketDto
{
    public string DigestValue { get; set; } = string.Empty;
    public Configuration Configuration { get; set; } = new Configuration();
    public InvoiceSale InvoiceSale { get; set; } = new InvoiceSale();
}

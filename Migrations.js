// migraciones - v1.2.1
db.Product.updateMany({}, { $rename: { "Price1": "PrecioVentaUnitario" } })
db.Product.updateMany({}, { $rename: { "Price2": "ValorUnitario" } })
db.Product.updateMany({}, { $set: { "CodProductoSUNAT": "-", "ControlStock": "NONE" } })
db.Configuration.updateMany({}, { $set: { "SunatArchivos": "-", "AccessToken": "-", "SubscriptionId": "-" } })

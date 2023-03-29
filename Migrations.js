// creación del usuario admin
db.User.insertOne({
    UserName: 'admin',
    Email: 'admin@local.pe',
    PasswordHash: 'AGIvkKeU4dv4D1SAoqgxmse5AicrGjmMAOqjRJe4xwuJ1UhS93ZMmiL6SjfUmVWvww==',
    Role: 'Admin'
})

// migraciones - v1.2.1
db.Product.updateMany({}, { $rename: { "Price1": "PrecioVentaUnitario" } })
db.Product.updateMany({}, { $rename: { "Price2": "ValorUnitario" } })
db.Product.updateMany({}, { $set: { "CodProductoSUNAT": "-", "ControlStock": "NONE" } })
db.Configuration.updateMany({}, { $set: { "SunatArchivos": "-", "AccessToken": "-", "SubscriptionId": "-" } })

﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nebula.Data.Inventario.Material;

/// <summary>
/// Detalle - Item salida de materiales.
/// </summary>
[BsonIgnoreExtraElements]
public class MaterialDetail : IGenericDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del Material.
    /// </summary>
    public string MaterialId { get; set; } = string.Empty;

    /// <summary>
    /// Identificador del Almacén.
    /// </summary>
    public string WarehouseId { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del Almacén.
    /// </summary>
    public string WarehouseName { get; set; } = string.Empty;

    /// <summary>
    /// Cantidad de Salida.
    /// </summary>
    public int CantSalida { get; set; }

    /// <summary>
    /// Identificador del Producto.
    /// </summary>
    public string ProductId { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del Producto.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Cantidad de Retorno.
    /// </summary>
    public int CantRetorno { get; set; }

    /// <summary>
    /// Cantidad Usado.
    /// </summary>
    public int CantUsado { get; set; }
}
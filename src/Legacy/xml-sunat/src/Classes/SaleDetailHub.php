<?php

namespace App\Classes;

class SaleDetailHub
{
    public ?string $codProducto = null;
    public ?string $unidad = null;
    public float $cantidad = 0;
    public float $mtoValorUnitario = 0;
    public ?string $descripcion = null;
    public float $mtoBaseIgv = 0;
    public float $porcentajeIgv = 0;
    public float $igv = 0;
    public ?string $tipAfeIgv = null;
    public float $totalImpuestos = 0;
    public float $mtoValorVenta = 0;
    public float $mtoPrecioUnitario = 0;
}

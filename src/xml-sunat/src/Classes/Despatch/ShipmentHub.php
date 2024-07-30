<?php

namespace App\Classes\Despatch;

class ShipmentHub
{
    public ?string $codTraslado = null;
    public ?string $modTraslado = null;
    /** @var array|string[] */
    public $indicadores = [];
    public ?string $fecTraslado = null;
    public float $pesoTotal = 0;
    public ?string $undPesoTotal = null;
    public ?DirectionHub $llegada = null;
    public ?DirectionHub $partida = null;
    public ?TransportistaHub $transportista = null;
}
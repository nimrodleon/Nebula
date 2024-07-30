<?php

namespace App\Classes\Despatch;

use App\Classes\ClientHub;

class DespatchHub
{
    public ?string $version = null;
    public ?string $tipoDoc = null;
    public ?string $serie = null;
    public ?string $correlativo = null;
    public ?string $fechaEmision = null;
    public ?ClientHub $destinatario = null;
    public ?ShipmentHub $envio = null;
}
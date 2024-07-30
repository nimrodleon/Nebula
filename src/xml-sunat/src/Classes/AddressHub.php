<?php

namespace App\Classes;

class AddressHub
{
    public ?string $ubigueo = null;
    public ?string $departamento = null;
    public ?string $provincia = null;
    public ?string $distrito = null;
    public ?string $urbanizacion = null;
    public ?string $direccion = null;
    public ?string $codLocal = null;

    public static function createFromJson(string $json): self
    {
        $data = json_decode($json, true);

        $addressDto = new self();
        $addressDto->ubigueo = $data['ubigueo'] ?? null;
        $addressDto->departamento = $data['departamento'] ?? null;
        $addressDto->provincia = $data['provincia'] ?? null;
        $addressDto->distrito = $data['distrito'] ?? null;
        $addressDto->urbanizacion = $data['urbanizacion'] ?? null;
        $addressDto->direccion = $data['direccion'] ?? null;
        $addressDto->codLocal = $data['codLocal'] ?? null;

        return $addressDto;
    }
}
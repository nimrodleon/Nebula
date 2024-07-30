<?php

namespace App\Classes;

class EmpresaHub
{
    public ?string $ruc = null;
    public ?string $companyId = null;
    public ?string $razonSocial = null;
    public ?string $nombreComercial = null;
    public ?string $sunatEndpoint = null;
    public ?ClaveSolHub $claveSol = null;
    public ?AddressHub $address = null;

    public static function createFromJson(string $json): self
    {
        $data = json_decode($json, true);

        $empresaDto = new self();
        $empresaDto->ruc = $data['ruc'] ?? null;
        $empresaDto->companyId = $data['companyId'] ?? null;
        $empresaDto->razonSocial = $data['razonSocial'] ?? null;
        $empresaDto->nombreComercial = $data['nombreComercial'] ?? null;
        $empresaDto->sunatEndpoint = $data['sunatEndpoint'] ?? null;

        if (isset($data['claveSol'])) {
            $empresaDto->claveSol = ClaveSolHub::createFromJson(json_encode($data['claveSol']));
        }

        if (isset($data['address'])) {
            $empresaDto->address = AddressHub::createFromJson(json_encode($data['address']));
        }

        return $empresaDto;
    }
}
<?php

namespace App\Service;

use Greenter\See;
use Greenter\Ws\Services\SunatEndpoints;

class SeeConfigurator
{
    private EmpresaService $empresaService;

    public function __construct(EmpresaService $empresaService)
    {
        $this->empresaService = $empresaService;
    }

    public function getConfiguredSee(string $companyId): See
    {
        $see = new See();
        $empresaDto = $this->empresaService->getEmpresaData($companyId);
        $see->setCertificate($this->empresaService->getCertificateContent($companyId));
        if ($empresaDto->sunatEndpoint === "FE_BETA")
            $see->setService(SunatEndpoints::FE_BETA);
        if ($empresaDto->sunatEndpoint === "FE_PRODUCCION")
            $see->setService(SunatEndpoints::FE_PRODUCCION);
        $claveSol = $empresaDto->claveSol;
        $see->setClaveSOL($empresaDto->ruc, $claveSol->user, $claveSol->password);
        return $see;
    }
}
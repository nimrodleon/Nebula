<?php

namespace App\Service;

use Symfony\Component\HttpKernel\KernelInterface;

class DataDirProvider
{
    private KernelInterface $kernel;

    public function __construct(KernelInterface $kernel)
    {
        $this->kernel = $kernel;
    }

    public function getDataDir(): string
    {
        return $this->kernel->getProjectDir() . DIRECTORY_SEPARATOR . "data";
    }

    public function getCompanyDir(string $companyId): string
    {
        return $this->getDataDir() . DIRECTORY_SEPARATOR . trim($companyId);
    }

    public function getEmpresaJsonPath(string $companyId): string
    {
        return $this->getDataDir() . DIRECTORY_SEPARATOR . trim($companyId) . DIRECTORY_SEPARATOR . "empresa.json";
    }

    public function getCertificatePfxPath(string $companyId): string
    {
        return $this->getDataDir() . DIRECTORY_SEPARATOR . trim($companyId) . DIRECTORY_SEPARATOR . "CERT" . DIRECTORY_SEPARATOR . "certificate.pfx";
    }

    public function getCertificatePemPath(string $companyId): string
    {
        return $this->getDataDir() . DIRECTORY_SEPARATOR . trim($companyId) . DIRECTORY_SEPARATOR . "CERT" . DIRECTORY_SEPARATOR . "certificate.pem";
    }

    public function getXmlPath(string $companyId, string $fileName, string $year, string $month): string
    {
        return $this->getCompanyDir($companyId) . DIRECTORY_SEPARATOR
            . trim($year) . DIRECTORY_SEPARATOR . trim($month) . DIRECTORY_SEPARATOR
            . $fileName . ".xml";
    }

    public function getCdrPath(string $companyId, string $fileName, string $year, string $month): string
    {
        return $this->getCompanyDir($companyId) . DIRECTORY_SEPARATOR
            . trim($year) . DIRECTORY_SEPARATOR . trim($month) . DIRECTORY_SEPARATOR
            . "R-{$fileName}" . ".zip";
    }
}
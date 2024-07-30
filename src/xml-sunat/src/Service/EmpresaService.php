<?php

namespace App\Service;

use Exception;
use Predis\Client;
use App\Classes\EmpresaHub;
use Greenter\Model\Company\Address;
use Greenter\Model\Company\Company;
use Greenter\XMLSecLibs\Certificate\X509Certificate;
use Greenter\XMLSecLibs\Certificate\X509ContentType;
use Symfony\Component\Filesystem\Filesystem;

class EmpresaService
{
    private Client $redisClient;
    private Filesystem $filesystem;
    private DataDirProvider $dataDirProvider;
    private int $ttl = 14400;

    public function __construct(
        Client          $redisClient,
        Filesystem      $filesystem,
        DataDirProvider $dataDirProvider)
    {
        $this->filesystem = $filesystem;
        $this->redisClient = $redisClient;
        $this->dataDirProvider = $dataDirProvider;
    }

    public function registrarEmpresa(EmpresaHub $empresaDto): void
    {
        $companyId = $empresaDto->companyId;
        $companyDir = $this->dataDirProvider->getCompanyDir($companyId);

        $this->filesystem->mkdir([$companyDir, $companyDir . DIRECTORY_SEPARATOR . "CERT"]);

        $empresaJsonPath = $this->dataDirProvider->getEmpresaJsonPath($companyId);
        $empresaJsonContent = json_encode($empresaDto, JSON_PRETTY_PRINT);
        $this->filesystem->dumpFile($empresaJsonPath, $empresaJsonContent);

        $cacheKey = "invoice_hub_" . $companyId;
        $cachedEmpresa = $this->redisClient->get($cacheKey);
        if ($cachedEmpresa !== null) {
            $this->redisClient->del($cacheKey);
            $this->redisClient->setex($cacheKey, $this->ttl, $empresaJsonContent);
        }
    }

    /**
     * @throws Exception
     */
    public function subirCertificado(string $companyId, string $password, $certificate): void
    {
        $companyDir = $this->dataDirProvider->getCompanyDir($companyId);
        $this->filesystem->mkdir([$companyDir . DIRECTORY_SEPARATOR . "CERT"]);
        $certificatePfxPath = $this->dataDirProvider->getCertificatePfxPath($companyId);
        if (file_exists($certificatePfxPath)) unlink($certificatePfxPath);
        $certificate->move(dirname($certificatePfxPath), "certificate.pfx");
        // convertir formato Pfx a .PEM
        $pfx = file_get_contents($certificatePfxPath);
        $x509Certificate = new X509Certificate($pfx, $password);
        $pem = $x509Certificate->export(X509ContentType::PEM);
        $certificatePathPem = $this->dataDirProvider->getCertificatePemPath($companyId);
        if (file_exists($certificatePathPem)) unlink($certificatePathPem);
        file_put_contents($certificatePathPem, $pem);
    }

    public function getEmpresaData(string $companyId): EmpresaHub
    {
        $cacheKey = "invoice_hub_" . $companyId;
        $cachedData = $this->redisClient->get($cacheKey);

        if ($cachedData !== null) {
            $empresaHub = json_decode($cachedData, true);
        } else {
            $empresaJsonPath = $this->dataDirProvider->getEmpresaJsonPath($companyId);
            $empresaJsonContent = file_get_contents($empresaJsonPath);
            $empresaData = json_decode($empresaJsonContent, true);
            $empresaHub = EmpresaHub::createFromJson(json_encode($empresaData));
            // Guardar en Redis para futuras consultas
            $this->redisClient->setex($cacheKey, $this->ttl, json_encode($empresaHub));
        }

        return EmpresaHub::createFromJson(json_encode($empresaHub));
    }

    public function getCompany(string $companyId): Company
    {
        $empresa = $this->getEmpresaData($companyId);

        $address = (new Address())
            ->setUbigueo($empresa->address->ubigueo)
            ->setDepartamento($empresa->address->departamento)
            ->setProvincia($empresa->address->provincia)
            ->setDistrito($empresa->address->distrito)
            ->setUrbanizacion($empresa->address->urbanizacion)
            ->setDireccion($empresa->address->direccion)
            ->setCodLocal($empresa->address->codLocal);

        return (new Company())
            ->setRuc($empresa->ruc)
            ->setRazonSocial($empresa->razonSocial)
            ->setNombreComercial($empresa->nombreComercial)
            ->setAddress($address);
    }

    public function getCertificateContent(string $companyId): ?string
    {
        $certificatePath = $this->dataDirProvider->getCertificatePemPath($companyId);
        if ($this->filesystem->exists($certificatePath)) {
            return file_get_contents($certificatePath);
        }
        return null;
    }
}
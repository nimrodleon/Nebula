<?php

namespace App\Service;

use App\Util\XmlParam;
use App\Classes\InvoiceHub;
use JetBrains\PhpStorm\ArrayShape;

class InvoiceService
{
    private EmpresaService $empresaService;
    private SeeConfigurator $seeConfigurator;
    private XmlCdrSaver $xmlCdrSaver;
    private ApiResultBuilder $apiResultBuilder;

    public function __construct(
        EmpresaService   $empresaService,
        SeeConfigurator  $seeConfigurator,
        XmlCdrSaver      $xmlCdrSaver,
        ApiResultBuilder $apiResultBuilder,
    )
    {
        $this->empresaService = $empresaService;
        $this->seeConfigurator = $seeConfigurator;
        $this->xmlCdrSaver = $xmlCdrSaver;
        $this->apiResultBuilder = $apiResultBuilder;
    }

    #[ArrayShape([
        'success' => "",
        'hash' => "null|string",
        'cdrCode' => "mixed",
        'cdrDescription' => "mixed",
        'cdrNotes' => "array",
        'cdrId' => "mixed"
    ])]
    public function register(InvoiceHub $invoiceDto, string $companyId): array
    {
        $see = $this->seeConfigurator->getConfiguredSee($companyId);
        $invoice = $invoiceDto->createInvoice($this->empresaService->getCompany($companyId));
        $result = $see->send($invoice);
        $xml = $see->getFactory()->getLastXml();
        if ($result->isSuccess()) {
            $param = new XmlParam();
            $param->companyId = $companyId;
            $param->fileName = $invoice->getName();
            $param->xml = $xml;
            $param->cdr = $result->getCdrZip();
            $param->year = $invoice->getFechaEmision()->format("Y");
            $param->month = $invoice->getFechaEmision()->format("m");
            $this->xmlCdrSaver->saveXmlAndCdr($param);
        }
        return $this->apiResultBuilder->buildApiResultData($result, $xml);
    }
}
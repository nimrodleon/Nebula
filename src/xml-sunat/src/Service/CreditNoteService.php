<?php

namespace App\Service;

use App\Classes\CreditNoteHub;
use App\Util\XmlParam;
use JetBrains\PhpStorm\ArrayShape;

class CreditNoteService
{
    private EmpresaService $empresaService;
    private SeeConfigurator $seeConfigurator;
    private XmlCdrSaver $xmlCdrSaver;
    private ApiResultBuilder $apiResultBuilder;

    public function __construct(
        EmpresaService   $empresaService,
        SeeConfigurator  $seeConfigurator,
        XmlCdrSaver      $xmlCdrSaver,
        ApiResultBuilder $apiResultBuilder)
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
    public function register(CreditNoteHub $creditNoteDto, string $companyId): array
    {
        $see = $this->seeConfigurator->getConfiguredSee($companyId);
        $creditNote = $creditNoteDto->createNote($this->empresaService->getCompany($companyId));
        $result = $see->send($creditNote);
        $xml = $see->getFactory()->getLastXml();
        if ($result->isSuccess()) {
            $param = new XmlParam();
            $param->companyId = $companyId;
            $param->fileName = $creditNote->getName();
            $param->xml = $xml;
            $param->cdr = $result->getCdrZip();
            $param->year = $creditNote->getFechaEmision()->format("Y");
            $param->month = $creditNote->getFechaEmision()->format("m");
            $this->xmlCdrSaver->saveXmlAndCdr($param);
        }
        return $this->apiResultBuilder->buildApiResultData($result, $xml);
    }
}
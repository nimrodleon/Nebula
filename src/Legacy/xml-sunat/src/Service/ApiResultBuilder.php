<?php

namespace App\Service;

use App\Util\XmlUtils;
use JetBrains\PhpStorm\ArrayShape;

class ApiResultBuilder
{
    private XmlUtils $xmlUtils;

    public function __construct(XmlUtils $xmlUtils)
    {
        $this->xmlUtils = $xmlUtils;
    }

    #[ArrayShape([
        'success' => "",
        'hash' => "null|string",
        'cdrCode' => "mixed",
        'cdrDescription' => "mixed",
        'cdrNotes' => "array",
        'cdrId' => "mixed"
    ])]
    public function buildApiResultData($result, $xml): array
    {
        $cdrResponse = $result->getCdrResponse();

        $cdrCode = null;
        $cdrDescription = null;
        $cdrNotes = [];
        $cdrId = null;

        if ($cdrResponse !== null) {
            $cdrCode = $cdrResponse->getCode();
            $cdrDescription = $cdrResponse->getDescription();
            $cdrNotes = $cdrResponse->getNotes();
            $cdrId = $cdrResponse->getId();
        }

        return [
            'success' => $result->isSuccess(),
            'hash' => $this->xmlUtils->getHashSign($xml),
            'cdrCode' => $cdrCode,
            'cdrDescription' => $cdrDescription,
            'cdrNotes' => $cdrNotes,
            'cdrId' => $cdrId,
        ];
    }
}
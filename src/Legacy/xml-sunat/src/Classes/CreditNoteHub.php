<?php

namespace App\Classes;

use App\Util\NumberToLetters;
use Greenter\Model\Company\Company;
use Greenter\Model\Sale\Legend;
use Greenter\Model\Sale\Note;

class CreditNoteHub extends BaseDocumentHub
{
    public ?string $tipDocAfectado = null;
    public ?string $numDocAfectado = null;
    public ?string $codMotivo = null;
    public ?string $desMotivo = null;

    public function createNote(Company $company): Note
    {
        // Cliente
        $client = $this->createClient();

        // Crear los items de detalles
        $detailItems = $this->createDetailItems();

        // Nota de Crédito
        $note = (new Note())
            ->setUblVersion('2.1')
            ->setTipoDoc('07')
            ->setSerie($this->serie)
            ->setCorrelativo($this->correlativo)
            ->setFechaEmision(\DateTime::createFromFormat("Y-m-d", $this->fechaEmision))
            ->setTipDocAfectado($this->tipDocAfectado)
            ->setNumDocfectado($this->numDocAfectado)
            ->setCodMotivo($this->codMotivo)
            ->setDesMotivo($this->desMotivo)
            ->setTipoMoneda($this->tipoMoneda)
            ->setCompany($company)
            ->setClient($client)
            ->setMtoOperGravadas($this->operGravadas)
            ->setMtoOperInafectas($this->mtoOperInafectas)
            ->setMtoOperExoneradas($this->mtoOperExoneradas)
            ->setMtoIGV($this->mtoIGV)
            ->setTotalImpuestos($this->totalImpuestos)
            ->setValorVenta($this->valorVenta)
            ->setSubTotal($this->valorVenta + $this->mtoIGV)
            ->setMtoImpVenta($this->valorVenta + $this->totalImpuestos);

        $legend = (new Legend())
            ->setCode('1000')
            ->setValue(new NumberToLetters(($this->valorVenta + $this->totalImpuestos)));

        $note->setDetails($detailItems)->setLegends([$legend]);

        return $note;
    }
}
<?php

namespace App\Classes;

use App\Util\NumberToLetters;
use Greenter\Model\Company\Company;
use Greenter\Model\Sale\Cuota;
use Greenter\Model\Sale\Invoice;
use Greenter\Model\Sale\Legend;
use Greenter\Model\Sale\PaymentTerms;

class InvoiceHub extends BaseDocumentHub
{
    public ?string $tipoDoc = null;
    public ?string $tipoOperacion = null;
    public ?FormaPagoHub $formaPago = null;
    public ?string $fecVencimiento = null;

    /**
     * @var CuotaHub[]|null
     */
    public array|null $cuotas = [];

    public function createInvoice(Company $company): Invoice
    {
        // Cliente
        $client = $this->createClient();

        // Crear los items de detalles
        $detailItems = $this->createDetailItems();

        // configurar forma de pago.
        $formaPago = (new PaymentTerms())
            ->setMoneda($this->formaPago->moneda)
            ->setTipo($this->formaPago->tipo)
            ->setMonto($this->formaPago->monto);

        // Venta
        $invoice = (new Invoice())
            ->setUblVersion('2.1')
            ->setTipoOperacion($this->tipoOperacion)
            ->setTipoDoc($this->tipoDoc)
            ->setSerie($this->serie)
            ->setCorrelativo($this->correlativo)
            ->setFechaEmision(\DateTime::createFromFormat("Y-m-d", $this->fechaEmision))
            ->setFormaPago($formaPago)
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

        if ($this->formaPago->tipo === "Credito" && $this->tipoDoc === "01") {
            $invoice->setFecVencimiento(\DateTime::createFromFormat("Y-m-d", $this->fecVencimiento));
            $cuotas = [];
            foreach ($this->cuotas as $cuota) {
                $_cuota = (new Cuota())
                    ->setMoneda($cuota->moneda)
                    ->setMonto($cuota->monto)
                    ->setFechaPago(\DateTime::createFromFormat("Y-m-d", $cuota->fechaPago));
                $cuotas[] = $_cuota;
            }
            $invoice->setCuotas($cuotas);
        }

        $legend = (new Legend())
            ->setCode('1000')
            ->setValue(new NumberToLetters($this->valorVenta + $this->totalImpuestos));

        $invoice->setDetails($detailItems)->setLegends([$legend]);

        return $invoice;
    }
}
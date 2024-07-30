<?php

namespace App\Classes;

use Greenter\Model\Client\Client;
use Greenter\Model\Sale\SaleDetail;

abstract class BaseDocumentHub
{
    public ?string $ruc = null;
    public ?string $serie = null;
    public ?string $correlativo = null;
    public ?string $fechaEmision = null;
    public ?string $tipoMoneda = null;
    public ?ClientHub $client = null;
    /** @var SaleDetailHub[] */
    public array $details = [];

    // Totales y subtotales.
    protected float $operGravadas = 0;
    protected float $mtoOperInafectas = 0;
    protected float $mtoOperExoneradas = 0;
    protected float $mtoIGV = 0;
    protected float $totalImpuestos = 0;
    protected float $valorVenta = 0;

    protected function createClient(): Client
    {
        return (new Client())
            ->setTipoDoc($this->client->tipoDoc)
            ->setNumDoc($this->client->numDoc)
            ->setRznSocial($this->client->rznSocial);
    }

    protected function createDetailItems(): array
    {
        $detailItems = [];
        $this->operGravadas = 0;
        $this->mtoOperInafectas = 0;
        $this->mtoOperExoneradas = 0;
        $this->mtoIGV = 0;
        $this->totalImpuestos = 0;
        $this->valorVenta = 0;
        foreach ($this->details as $detailDto) {
            $detail = (new SaleDetail())
                ->setCodProducto($detailDto->codProducto)
                ->setUnidad($detailDto->unidad)
                ->setCantidad($detailDto->cantidad)
                ->setMtoValorUnitario($detailDto->mtoValorUnitario)
                ->setDescripcion($detailDto->descripcion)
                ->setMtoBaseIgv($detailDto->mtoBaseIgv)
                ->setPorcentajeIgv($detailDto->porcentajeIgv)
                ->setIgv($detailDto->igv)
                ->setTipAfeIgv($detailDto->tipAfeIgv)
                ->setTotalImpuestos($detailDto->totalImpuestos)
                ->setMtoValorVenta($detailDto->mtoValorVenta)
                ->setMtoPrecioUnitario($detailDto->mtoPrecioUnitario);
            $detailItems[] = $detail;
            if ($detailDto->tipAfeIgv === "10") {
                $this->operGravadas += $detailDto->mtoBaseIgv;
            }
            if ($detailDto->tipAfeIgv === "20") {
                $this->mtoOperExoneradas += $detailDto->mtoBaseIgv;
            }
            if ($detailDto->tipAfeIgv === "30") {
                $this->mtoOperInafectas += $detailDto->mtoBaseIgv;
            }
            $this->mtoIGV += $detailDto->igv;
            $this->totalImpuestos += $detailDto->totalImpuestos;
            $this->valorVenta += $detailDto->mtoValorVenta;
        }
        return $detailItems;
    }
}
<?php

namespace App\Util;

class NumberToLetters
{
    private float $value;

    public function __construct(float $value)
    {
        $this->value = round($value, 2);
    }

    public function __toString(): string
    {
        $entero = (int)floor($this->value);
        $decimales = (int)round(($this->value - $entero) * 100);

        return $this->convertirNumero($entero) . " CON " . number_format($decimales, 0, ',', '.') . "/100 SOLES";
    }

    private function convertirNumero(float $valor): string
    {
        $num2Text = '';
        $valor = (int)floor($valor);

        if ($valor == 0) {
            $num2Text = "CERO";
        } elseif ($valor == 1) {
            $num2Text = "UNO";
        } elseif ($valor == 2) {
            $num2Text = "DOS";
        } elseif ($valor == 3) {
            $num2Text = "TRES";
        } elseif ($valor == 4) {
            $num2Text = "CUATRO";
        } elseif ($valor == 5) {
            $num2Text = "CINCO";
        } elseif ($valor == 6) {
            $num2Text = "SEIS";
        } elseif ($valor == 7) {
            $num2Text = "SIETE";
        } elseif ($valor == 8) {
            $num2Text = "OCHO";
        } elseif ($valor == 9) {
            $num2Text = "NUEVE";
        } elseif ($valor == 10) {
            $num2Text = "DIEZ";
        } elseif ($valor == 11) {
            $num2Text = "ONCE";
        } elseif ($valor == 12) {
            $num2Text = "DOCE";
        } elseif ($valor == 13) {
            $num2Text = "TRECE";
        } elseif ($valor == 14) {
            $num2Text = "CATORCE";
        } elseif ($valor == 15) {
            $num2Text = "QUINCE";
        } elseif ($valor < 20) {
            $num2Text = "DIECI" . $this->convertirNumero($valor - 10);
        } elseif ($valor == 20) {
            $num2Text = "VEINTE";
        } elseif ($valor < 30) {
            $num2Text = "VEINTI" . $this->convertirNumero($valor - 20);
        } elseif ($valor == 30) {
            $num2Text = "TREINTA";
        } elseif ($valor == 40) {
            $num2Text = "CUARENTA";
        } elseif ($valor == 50) {
            $num2Text = "CINCUENTA";
        } elseif ($valor == 60) {
            $num2Text = "SESENTA";
        } elseif ($valor == 70) {
            $num2Text = "SETENTA";
        } elseif ($valor == 80) {
            $num2Text = "OCHENTA";
        } elseif ($valor == 90) {
            $num2Text = "NOVENTA";
        } elseif ($valor < 100) {
            $num2Text = $this->convertirNumero(floor($valor / 10) * 10) . " Y " . $this->convertirNumero($valor % 10);
        } elseif ($valor == 100) {
            $num2Text = "CIEN";
        } elseif ($valor < 200) {
            $num2Text = "CIENTO " . $this->convertirNumero($valor - 100);
        } elseif ($valor == 200 || $valor == 300 || $valor == 400 || $valor == 600 || $valor == 800) {
            $num2Text = $this->convertirNumero(floor($valor / 100)) . "CIENTOS";
        } elseif ($valor == 500) {
            $num2Text = "QUINIENTOS";
        } elseif ($valor == 700) {
            $num2Text = "SETECIENTOS";
        } elseif ($valor == 900) {
            $num2Text = "NOVECIENTOS";
        } elseif ($valor < 1000) {
            $num2Text = $this->convertirNumero(floor($valor / 100) * 100) . " " . $this->convertirNumero($valor % 100);
        } elseif ($valor == 1000) {
            $num2Text = "MIL";
        } elseif ($valor < 2000) {
            $num2Text = "MIL " . $this->convertirNumero($valor % 1000);
        } elseif ($valor < 1000000) {
            $num2Text = $this->convertirNumero(floor($valor / 1000)) . " MIL";
            if ($valor % 1000 > 0) {
                $num2Text .= " " . $this->convertirNumero($valor % 1000);
            }
        } elseif ($valor == 1000000) {
            $num2Text = "UN MILLÓN";
        } elseif ($valor < 2000000) {
            $num2Text = "UN MILLÓN " . $this->convertirNumero($valor % 1000000);
        } elseif ($valor < 1000000000000) {
            $num2Text = $this->convertirNumero(floor($valor / 1000000)) . " MILLONES ";
            if ($valor - floor($valor / 1000000) * 1000000 > 0) {
                $num2Text .= " " . $this->convertirNumero($valor - floor($valor / 1000000) * 1000000);
            }
        } elseif ($valor == 1000000000000) {
            $num2Text = "UN BILLÓN";
        } elseif ($valor < 2000000000000) {
            $num2Text = "UN BILLÓN " . $this->convertirNumero($valor - floor($valor / 1000000000000) * 1000000000000);
        } else {
            $num2Text = $this->convertirNumero(floor($valor / 1000000000000)) . " BILLONES";
            if ($valor - floor($valor / 1000000000000) * 1000000000000 > 0) {
                $num2Text .= " " . $this->convertirNumero($valor - floor($valor / 1000000000000) * 1000000000000);
            }
        }

        return $num2Text;
    }
}

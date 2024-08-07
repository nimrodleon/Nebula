<?php

namespace App\Classes;

class ClaveSolHub
{
    public ?string $user = null;
    public ?string $password = null;

    public static function createFromJson(string $json): self
    {
        $data = json_decode($json, true);

        $claveSolDto = new self();
        $claveSolDto->user = $data['user'] ?? null;
        $claveSolDto->password = $data['password'] ?? null;

        return $claveSolDto;
    }
}
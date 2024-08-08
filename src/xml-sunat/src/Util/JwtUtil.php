<?php

namespace App\Util;

use Firebase\JWT\JWT;
use Firebase\JWT\Key;

class JwtUtil
{
    private string $secretKey;

    public function __construct(string $secretKey)
    {
        $this->secretKey = $secretKey;
    }

    public function generateToken(array $payload): string
    {
        return JWT::encode($payload, $this->secretKey, 'HS256');
    }

    public function verifyToken(string $token): ?\stdClass
    {
        if (str_starts_with($token, 'Bearer ')) {
            $token = substr($token, 7);
        }
        try {
            return JWT::decode($token, new Key($this->secretKey, 'HS256'));
        } catch (\Exception $e) {
            // Manejar el error aquí, como lanzar una excepción personalizada o devolver un mensaje de error
            return null;
        }
    }
}
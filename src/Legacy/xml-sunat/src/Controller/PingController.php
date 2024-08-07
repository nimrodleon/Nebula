<?php

namespace App\Controller;

use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\Routing\Annotation\Route;

class PingController
{
    #[Route('/ping', methods: 'GET')]
    public function pingAction(): JsonResponse
    {
        return new JsonResponse(['status' => 'pong']);
    }
}
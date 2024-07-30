<?php

namespace App\EventListener;

use App\Util\JwtUtil;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\HttpKernel\Event\ControllerEvent;

class JwtAuthenticationListener
{
    private JwtUtil $jwtUtil;

    public function __construct(JwtUtil $jwtUtil)
    {
        $this->jwtUtil = $jwtUtil;
    }

    public function onKernelController(ControllerEvent $event)
    {
        $controller = $event->getController();

        if (!is_array($controller)) {
            return;
        }

        // Obtiene el token desde la cabecera "Authorization"
        $request = $event->getRequest();
        $token = $request->headers->get('Authorization');

        if (!$token) {
            // Enviar una respuesta JSON
            $response = new Response(json_encode(['error' => 'Token no proporcionado']), Response::HTTP_UNAUTHORIZED, ['Content-Type' => 'application/json']);
            $event->setController(fn() => $response);
            return;
        }

        // Verifica el token
        $decodedToken = $this->jwtUtil->verifyToken($token);

        if (!$decodedToken) {
            // Enviar una respuesta JSON
            $response = new Response(json_encode(['error' => 'Token inválido']), Response::HTTP_UNAUTHORIZED, ['Content-Type' => 'application/json']);
            $event->setController(fn() => $response);
            return;
        }
    }
}
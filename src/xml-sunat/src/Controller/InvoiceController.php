<?php

namespace App\Controller;

use App\Classes\InvoiceHub;
use App\Form\InvoiceType;
use App\Service\InvoiceService;
use Psr\Log\LoggerInterface;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;

#[Route('/api/invoice')]
class InvoiceController extends AbstractController
{
    private InvoiceService $invoiceService;
    private LoggerInterface $logger;

    public function __construct(InvoiceService $invoiceService, LoggerInterface $logger)
    {
        $this->invoiceService = $invoiceService;
        $this->logger = $logger;
    }

    #[Route('/send/{companyId}', methods: 'POST')]
    public function send(Request $request, string $companyId): JsonResponse
    {
        $dto = new InvoiceHub();
        $content = json_decode($request->getContent(), true);
        $this->logger->error("Origen de solicitud: " . $request->getHost());
        $this->logger->info("La información enviada es: " . $request->getContent());
        $form = $this->createForm(InvoiceType::class, $dto);
        $form->submit($content);
        if ($form->isSubmitted() && $form->isValid()) {
            $dto = $form->getData();
            // Aquí llamamos al servicio InvoiceService
            $result = $this->invoiceService->register($dto, $companyId);
            return $this->json($result);
        }
        return $this->json($form);
    }
}

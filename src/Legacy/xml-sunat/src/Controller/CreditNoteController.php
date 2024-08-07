<?php

namespace App\Controller;

use App\Classes\CreditNoteHub;
use App\Form\CreditNoteType;
use App\Service\CreditNoteService;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\Routing\Annotation\Route;

#[Route('/api/creditNote')]
class CreditNoteController extends AbstractController
{
    private CreditNoteService $creditNoteService;

    public function __construct(CreditNoteService $creditNoteService)
    {
        $this->creditNoteService = $creditNoteService;
    }

    #[Route('/send/{companyId}', methods: 'POST')]
    public function send(Request $request, string $companyId): JsonResponse
    {
        $dto = new CreditNoteHub();
        $content = json_decode($request->getContent(), true);
        $form = $this->createForm(CreditNoteType::class, $dto);
        $form->submit($content);
        if ($form->isSubmitted() && $form->isValid()) {
            $dto = $form->getData();
            // Aquí llamamos al servicio CreditNoteService
            $result = $this->creditNoteService->register($dto, $companyId);
            return $this->json($result);
        }
        return $this->json($form);
    }
}

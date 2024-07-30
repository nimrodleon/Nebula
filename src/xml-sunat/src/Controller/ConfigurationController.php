<?php

namespace App\Controller;

use Exception;
use App\Classes\EmpresaHub;
use App\Form\EmpresaType;
use App\Service\EmpresaService;
use Symfony\Bundle\FrameworkBundle\Controller\AbstractController;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\Routing\Annotation\Route;

#[Route('/api/configuration')]
class ConfigurationController extends AbstractController
{
    private EmpresaService $empresaService;

    public function __construct(EmpresaService $empresaService)
    {
        $this->empresaService = $empresaService;
    }

    #[Route('/registrarEmpresa', methods: 'POST')]
    public function registrarEmpresa(Request $request): JsonResponse
    {
        $empresaDto = new EmpresaHub();
        $form = $this->createForm(EmpresaType::class, $empresaDto);
        $form->submit(json_decode($request->getContent(), true));

        if (!$form->isValid()) {
            return new JsonResponse(['error' => 'Datos JSON no válidos'], Response::HTTP_BAD_REQUEST);
        }

        $this->empresaService->registrarEmpresa($empresaDto);

        return new JsonResponse(['message' => 'Empresa configurada con éxito'], Response::HTTP_CREATED);
    }

    /**
     * @throws Exception
     */
    #[Route('/subirCertificado', methods: 'POST')]
    public function subirCertificado(Request $request): JsonResponse
    {
        $companyId = $request->request->get('companyId');
        $certificate = $request->files->get('certificate');
        $password = $request->request->get('password');

        if (!$certificate) {
            return new JsonResponse(['error' => 'Archivo certificate no encontrado'], Response::HTTP_BAD_REQUEST);
        }

        $this->empresaService->subirCertificado($companyId, $password, $certificate);

        return new JsonResponse(['message' => 'Certificado subido con éxito'], Response::HTTP_CREATED);
    }

    #[Route('/datosEmpresa/{companyId}', methods: 'GET')]
    public function datosEmpresa(string $companyId): JsonResponse
    {
        $empresa = $this->empresaService->getEmpresaData($companyId);
        return new JsonResponse($empresa);
    }
}

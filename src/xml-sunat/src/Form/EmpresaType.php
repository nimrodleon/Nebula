<?php

namespace App\Form;

use App\Classes\EmpresaHub;
use Symfony\Component\Form\AbstractType;
use Symfony\Component\Form\Extension\Core\Type\TextType;
use Symfony\Component\Form\FormBuilderInterface;
use Symfony\Component\OptionsResolver\OptionsResolver;

class EmpresaType extends AbstractType
{
    public function buildForm(FormBuilderInterface $builder, array $options): void
    {
        $builder
            ->add('ruc', TextType::class)
            ->add('companyId', TextType::class)
            ->add('razonSocial', TextType::class)
            ->add('nombreComercial', TextType::class)
            ->add('sunatEndpoint', TextType::class)
            ->add('claveSol', ClaveSolType::class)
            ->add('address', AddressType::class);
    }

    public function configureOptions(OptionsResolver $resolver): void
    {
        $resolver->setDefaults([
            'data_class' => EmpresaHub::class,
        ]);
    }
}

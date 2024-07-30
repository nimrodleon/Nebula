<?php

namespace App\Form;

use App\Classes\SaleDetailHub;
use Symfony\Component\Form\AbstractType;
use Symfony\Component\Form\Extension\Core\Type\NumberType;
use Symfony\Component\Form\Extension\Core\Type\TextType;
use Symfony\Component\Form\FormBuilderInterface;
use Symfony\Component\OptionsResolver\OptionsResolver;

class SaleDetailType extends AbstractType
{
    public function buildForm(FormBuilderInterface $builder, array $options): void
    {
        $builder
            ->add('codProducto', TextType::class)
            ->add('unidad', TextType::class)
            ->add('cantidad', NumberType::class)
            ->add('mtoValorUnitario', NumberType::class)
            ->add('descripcion', TextType::class)
            ->add('mtoBaseIgv', NumberType::class)
            ->add('porcentajeIgv', NumberType::class)
            ->add('igv', NumberType::class)
            ->add('tipAfeIgv', TextType::class)
            ->add('totalImpuestos', NumberType::class)
            ->add('mtoValorVenta', NumberType::class)
            ->add('mtoPrecioUnitario', NumberType::class);
    }

    public function configureOptions(OptionsResolver $resolver): void
    {
        $resolver->setDefaults([
            'data_class' => SaleDetailHub::class
        ]);
    }
}

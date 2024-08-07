<?php

namespace App\Form;

use App\Classes\AddressHub;
use Symfony\Component\Form\AbstractType;
use Symfony\Component\Form\Extension\Core\Type\TextType;
use Symfony\Component\Form\FormBuilderInterface;
use Symfony\Component\OptionsResolver\OptionsResolver;

class AddressType extends AbstractType
{
    public function buildForm(FormBuilderInterface $builder, array $options): void
    {
        $builder
            ->add('ubigueo', TextType::class)
            ->add('departamento', TextType::class)
            ->add('provincia', TextType::class)
            ->add('distrito', TextType::class)
            ->add('urbanizacion', TextType::class)
            ->add('direccion', TextType::class)
            ->add('codLocal', TextType::class);
    }

    public function configureOptions(OptionsResolver $resolver): void
    {
        $resolver->setDefaults([
            'data_class' => AddressHub::class,
        ]);
    }
}

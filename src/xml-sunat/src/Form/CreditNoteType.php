<?php

namespace App\Form;

use App\Classes\CreditNoteHub;
use Symfony\Component\Form\AbstractType;
use Symfony\Component\Form\Extension\Core\Type\CollectionType;
use Symfony\Component\Form\Extension\Core\Type\TextType;
use Symfony\Component\Form\FormBuilderInterface;
use Symfony\Component\OptionsResolver\OptionsResolver;

class CreditNoteType extends AbstractType
{
    public function buildForm(FormBuilderInterface $builder, array $options): void
    {
        $builder
            ->add('ruc', TextType::class)
            ->add('serie', TextType::class)
            ->add('correlativo', TextType::class)
            ->add('fechaEmision', TextType::class)
            ->add('tipDocAfectado', TextType::class)
            ->add('numDocAfectado', TextType::class)
            ->add('codMotivo', TextType::class)
            ->add('desMotivo', TextType::class)
            ->add('tipoMoneda', TextType::class)
            ->add('client', ClientType::class) // Assuming you have a ClientType defined
            ->add('details', CollectionType::class, [
                'entry_type' => SaleDetailType::class,
                'allow_add' => true,
                'by_reference' => false
            ]);
    }

    public function configureOptions(OptionsResolver $resolver): void
    {
        $resolver->setDefaults([
            'data_class' => CreditNoteHub::class,
        ]);
    }
}

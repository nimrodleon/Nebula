import React, {Fragment} from 'react'
import {Button, Form, Modal} from 'react-bootstrap'
import {useForm} from 'react-hook-form'
import {useDispatch, useSelector} from 'react-redux'
import {AddTaxAction, btnCloseShowTaxModal, EditTaxAction} from '../../../actions/taxes'

/**
 * Formulario modal para editar impuestos.
 * @returns {JSX.Element}
 * @constructor
 */
const TaxModalForm = () => {
	const dispatch = useDispatch()
	const {register, handleSubmit, setValue} = useForm()

	// Variables de estado.
	const show = useSelector(state => state.taxes.showModal)
	const title = useSelector(state => state.taxes.title)
	const currentTax = useSelector(state => state.taxes.currentTax)
	const typeOperation = useSelector(state => state.taxes.typeOperation)

	// Acciones redux.
	const handleClose = () => dispatch(btnCloseShowTaxModal())
	const handleAddTax = (tax) => dispatch(AddTaxAction(tax))
	const handleEditTax = (tax) => dispatch(EditTaxAction(tax))

	// Guardar Cambios.
	const onSubmit = (f) => {
		if (typeOperation === 'ADD') {
			f.value = Number(f.value)
			handleAddTax(f)
		}
		if (typeOperation === 'EDIT') {
			f.value = Number(f.value)
			handleEditTax(f)
		}
	}

	// Cargar valores al formulario de impuestos.
	setValue('id', currentTax.id)
	setValue('name', currentTax.name)
	setValue('value', currentTax.value)

	// Render Component.
	return (
		<Fragment>
			<Modal show={show} onHide={handleClose} centered={true} animation={false}>
				<Modal.Header closeButton>
					<Modal.Title className="text-uppercase">{title}</Modal.Title>
				</Modal.Header>
				<Modal.Body>
					<Form onSubmit={handleSubmit(onSubmit)} id="TaxModalForm">
						<Form.Group>
							<Form.Label className="font-weight-bold">Nombre</Form.Label>
							<Form.Control type="text" {...register('name', {required: true})} placeholder="I.G.V"/>
						</Form.Group>
						<Form.Group>
							<Form.Label className="font-weight-bold">Valor</Form.Label>
							<Form.Control type="number" {...register('value')} placeholder="18"/>
						</Form.Group>
					</Form>
				</Modal.Body>
				<Modal.Footer>
					<Button variant="secondary" onClick={handleClose}>Cancelar</Button>
					<Button variant="primary" type={'submit'} form="TaxModalForm">Guardar Cambios</Button>
				</Modal.Footer>
			</Modal>
		</Fragment>
	)
}

export default TaxModalForm
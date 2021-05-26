import React, {Fragment} from 'react'
import {Button, Col, Form, Modal} from 'react-bootstrap'
import {useDispatch, useSelector} from 'react-redux'
import {AddContactAction, btnCloseShowContactModal, EditContactAction} from '../../../actions/contacts'
import {useForm} from 'react-hook-form'

/**
 * Registra los datos del Contacto.
 * @param props
 * @returns {JSX.Element}
 * @constructor
 */
const ContactModalForm = (props) => {
	const dispatch = useDispatch()
	const {register, handleSubmit, setValue} = useForm()

	// variables de estado del store.
	const show = useSelector(state => state.contacts.showModal)
	const title = useSelector(state => state.contacts.title)
	const currentContact = useSelector(state => state.contacts.currentContact)
	const typeOperation = useSelector(state => state.contacts.typeOperation)

	// Acciones redux.	
	const handleClose = () => dispatch(btnCloseShowContactModal())
	const handleAddContact = (contact) => dispatch(AddContactAction(contact))
	const handleEditContact = (contact) => dispatch(EditContactAction(contact))

	// Guardar cambios.
	const onSubmit = (f) => {
		if (typeOperation === 'ADD') {
			handleAddContact(f)
		}
		if (typeOperation === 'EDIT') {
			handleEditContact(f)
		}
	}

	// Cargar valores al formulario de contacto.
	setValue('id', currentContact.id)
	setValue('type_doc', currentContact.type_doc)
	setValue('document', currentContact.document)
	setValue('full_name', currentContact.full_name)
	setValue('address', currentContact.address)
	setValue('phone_number', currentContact.phone_number)
	setValue('email', currentContact.email)

	// Render Component.
	return (
		<Fragment>
			<Modal show={show} onHide={handleClose}
						 centered={true} animation={false}>
				<Modal.Header closeButton>
					<Modal.Title className="text-uppercase">{title}</Modal.Title>
				</Modal.Header>
				<Modal.Body>
					<Form
						onSubmit={handleSubmit(onSubmit)}
						id="ContactModalForm">
						<Form.Row>
							<Form.Group as={Col}>
								<Form.Label className="font-weight-bold">Tipo</Form.Label>
								<Form.Control as={'select'} {...register('type_doc', {required: true})}>
									<option value="1">D.N.I</option>
									<option value="6">R.U.C</option>
									<option value="0">SIN R.U.C</option>
								</Form.Control>
							</Form.Group>
							<Form.Group as={Col}>
								<Form.Label className="font-weight-bold">Documento</Form.Label>
								<Form.Control type="text" {...register('document', {required: true})} placeholder="D.N.I"/>
							</Form.Group>
						</Form.Row>
						<Form.Group>
							<Form.Label className="font-weight-bold">Nombres y/o Razón Social</Form.Label>
							<Form.Control type="text" {...register('full_name', {required: true})} placeholder="Empresa S.A.C."/>
						</Form.Group>
						<Form.Group>
							<Form.Label className="font-weight-bold">Dirección</Form.Label>
							<Form.Control type="text" {...register('address')} placeholder="Av. Perú S/N, Andahuaylas"/>
						</Form.Group>
						<Form.Row>
							<Form.Group as={Col} className="mb-0">
								<Form.Label className="font-weight-bold">E-Mail</Form.Label>
								<Form.Control type="email" {...register('email')} placeholder="contacto@local.pe"/>
							</Form.Group>
							<Form.Group as={Col} className="mb-0">
								<Form.Label className="font-weight-bold">Teléfono</Form.Label>
								<Form.Control type="text" {...register('phone_number')} placeholder="987-654-321"/>
							</Form.Group>
						</Form.Row>
					</Form>
				</Modal.Body>
				<Modal.Footer>
					<Button variant="secondary" onClick={handleClose}>Cancelar</Button>
					<Button variant="primary" type={'submit'} form="ContactModalForm">Guardar Cambios</Button>
				</Modal.Footer>
			</Modal>
		</Fragment>
	)
}

export default ContactModalForm
import React, {Fragment} from 'react'
import {Button, Col, Form, Modal} from 'react-bootstrap'
import {useDispatch, useSelector} from 'react-redux'
import {useForm} from 'react-hook-form'
import {AddUserAction, btnCloseShowUserModal, EditUserAction} from '../../../actions/users'

/**
 * modal registro de usuarios.
 * @returns {JSX.Element}
 * @constructor
 */
const UserModalForm = () => {
	const dispatch = useDispatch()
	const {register, handleSubmit, setValue} = useForm()

	// Variables de estado.
	const show = useSelector(state => state.users.showModal)
	const title = useSelector(state => state.users.title)
	const currentUser = useSelector(state => state.users.currentUser)
	const typeOperation = useSelector(state => state.users.typeOperation)

	// Acciones redux.
	const handleClose = () => dispatch(btnCloseShowUserModal())
	const handleAddUser = (user) => dispatch(AddUserAction(user))
	const handleEditUser = (user) => dispatch(EditUserAction(user))

	// Guardar Cambios.
	const onSubmit = (f) => {
		if (typeOperation === 'ADD') {
			// valor por defecto de suspended es false.
			f.suspended = false
			handleAddUser(f)
		}
		if (typeOperation === 'EDIT') {
			f.user_name = currentUser.user_name
			f.suspended = currentUser.suspended
			handleEditUser(f)
		}
	}

	// Cargar valores por defecto.
	setValue('id', currentUser.id)
	setValue('full_name', currentUser.full_name)
	setValue('address', currentUser.address)
	setValue('phone_number', currentUser.phone_number)
	setValue('user_name', currentUser.user_name)
	setValue('permission', currentUser.permission)
	setValue('email', currentUser.email)

	// Render Component.
	return (
		<Fragment>
			<Modal show={show} onHide={handleClose} centered={true} animation={false}>
				<Modal.Header closeButton>
					<Modal.Title className="text-uppercase">{title}</Modal.Title>
				</Modal.Header>
				<Modal.Body>
					<Form onSubmit={handleSubmit(onSubmit)} id="UserModalForm">
						<Form.Group>
							<Form.Label className="font-weight-bold">Nombres</Form.Label>
							<Form.Control type="text" {...register('full_name', {required: true})} placeholder="Nombres..."/>
						</Form.Group>
						<Form.Row>
							<Form.Group as={Col}>
								<Form.Label className="font-weight-bold">Permiso</Form.Label>
								<Form.Control as={'select'} {...register('permission', {required: true})}>
									<option value="ROLE_ADMIN">ROLE_ADMIN</option>
									<option value="ROLE_CASH">ROLE_CASH</option>
									<option value="ROLE_USER">ROLE_USER</option>
								</Form.Control>
							</Form.Group>
							<Form.Group as={Col}>
								<Form.Label className="font-weight-bold">Username</Form.Label>
								<Form.Control type="text"
															{...register('user_name', {required: true})}
															placeholder="@username" disabled={typeOperation === 'EDIT'}/>
							</Form.Group>
						</Form.Row>
						<Form.Group>
							<Form.Label className="font-weight-bold">Dirección</Form.Label>
							<Form.Control type="text" {...register('address', {required: true})} placeholder="Dirección..."/>
						</Form.Group>
						<Form.Row>
							<Form.Group as={Col} className="mb-0">
								<Form.Label className="font-weight-bold">E-Mail</Form.Label>
								<Form.Control type="email" {...register('email', {required: true})} placeholder="user@local.pe"/>
							</Form.Group>
							<Form.Group as={Col} className="mb-0">
								<Form.Label className="font-weight-bold">Teléfono</Form.Label>
								<Form.Control type="text" {...register('phone_number', {required: true})} placeholder="987-654-321"/>
							</Form.Group>
						</Form.Row>
					</Form>
				</Modal.Body>
				<Modal.Footer>
					<Button variant="secondary" onClick={handleClose}>Cancelar</Button>
					<Button variant="primary" type={'submit'} form="UserModalForm">Guardar Cambios</Button>
				</Modal.Footer>
			</Modal>
		</Fragment>
	)
}

export default UserModalForm
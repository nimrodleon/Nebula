import React, {Fragment} from 'react'
import {useDispatch} from 'react-redux'
import {btnEditShowUserModal, ChangeStatusUserAccountAction, DeleteUserAction} from '../../../actions/users'
import {SwalConfirmDialog, TdBtn} from '../../common/util'
import {Button, ButtonGroup, Dropdown} from 'react-bootstrap'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faCheckCircle, faEdit, faTimesCircle, faTrashAlt} from '@fortawesome/free-solid-svg-icons'
import axios from '../../../axios'
import Swal from 'sweetalert2'

/**
 * Item usuario.
 * @param props
 * @returns {JSX.Element}
 * @constructor
 */
const UserItem = (props) => {
	const {data} = props
	const dispatch = useDispatch()

	// Editar usuario.
	const handleEdit = () => {
		dispatch(btnEditShowUserModal(data))
	}

	// Borrar usuario.
	const handleDelete = () => {
		SwalConfirmDialog().then(result => {
			if (result.isConfirmed) {
				dispatch(DeleteUserAction(data.id))
			}
		})
	}

	// Cambiar Contraseña.
	const handleChangePassword = () => {
		Swal.fire({
			title: 'Nueva Contraseña!',
			input: 'password',
			inputAttributes: {
				autocapitalize: 'off'
			},
			showCancelButton: true,
			confirmButtonText: 'Guardar',
			cancelButtonText: 'Cancelar',
			showLoaderOnConfirm: true,
			preConfirm: (value) => {
				if (value.length <= 6) {
					Swal.showValidationMessage('Ingrese una contraseña valida!')
				}
			},
			allowOutsideClick: () => !Swal.isLoading()
		}).then((result) => {
			if (result.isConfirmed) {
				axios.patch(`users/password_change/${data.id}`,
					{password: result.value}).then(() => {
					Swal.fire(
						'Password!',
						'Contraseña Cambiada.',
						'success'
					)
				})
			}
		})
	}

	// Activar Cuenta.
	const handleActivateAccount = (e) => {
		e.preventDefault()
		Swal.fire({
			title: '¿Estás seguro?',
			text: 'que quieres activar esta cuenta!',
			icon: 'warning',
			showCancelButton: true,
			confirmButtonColor: '#3085d6',
			cancelButtonColor: '#d33',
			confirmButtonText: 'Sí, Activar!',
			cancelButtonText: 'Cancelar'
		}).then((result) => {
			if (result.isConfirmed) {
				let user = data
				user.suspended = false
				dispatch(ChangeStatusUserAccountAction(user))
				// console.log('Activar Cuenta', data)
			}
		})
	}

	// Suspender Cuenta.
	const handleSuspendAccount = (e) => {
		e.preventDefault()
		Swal.fire({
			title: '¿Estás seguro?',
			text: 'que quieres suspender esta cuenta!',
			icon: 'warning',
			showCancelButton: true,
			confirmButtonColor: '#3085d6',
			cancelButtonColor: '#d33',
			confirmButtonText: 'Sí, Suspender!',
			cancelButtonText: 'Cancelar'
		}).then((result) => {
			if (result.isConfirmed) {
				let user = data
				user.suspended = true
				dispatch(ChangeStatusUserAccountAction(user))
				// console.log('Suspender Cuenta', data)
			}
		})
	}

	// Render Component.
	return (
		<Fragment>
			<tr>
				<td>{data.full_name}</td>
				<td>{data.user_name}</td>
				<td>{data.permission}</td>
				<td>
					<Dropdown as={ButtonGroup}>
						<Button variant="info" size={'sm'} onClick={handleChangePassword}>Contraseña</Button>
						<Dropdown.Toggle split variant="info" id="dropdown-split-basic"/>
						<Dropdown.Menu>
							{
								data.suspended === true &&
								<Dropdown.Item href="#" onClick={handleActivateAccount}>Activar Cuenta</Dropdown.Item>
							}
							{
								data.suspended === false &&
								<Dropdown.Item href="#" onClick={handleSuspendAccount}>Suspender Cuenta</Dropdown.Item>
							}
						</Dropdown.Menu>
					</Dropdown>
				</td>
				<td>{data.email}</td>
				<td>{data.phone_number}</td>
				<td>
					{/*{data.suspended.toString()}*/}
					{data.suspended === true && <FontAwesomeIcon icon={faTimesCircle} size={'lg'} className="text-danger"/>}
					{data.suspended === false && <FontAwesomeIcon icon={faCheckCircle} size={'lg'} className="text-success"/>}
				</td>
				<TdBtn>
					<Button variant={'primary'} onClick={handleEdit} className="mr-2" size={'sm'}>
						<FontAwesomeIcon icon={faEdit}/>
					</Button>
					<Button variant={'danger'} onClick={handleDelete} size={'sm'}>
						<FontAwesomeIcon icon={faTrashAlt}/>
					</Button>
				</TdBtn>
			</tr>
		</Fragment>
	)
}

export default UserItem
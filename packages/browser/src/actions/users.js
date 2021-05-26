import * as type from '../types'
import withReactContent from 'sweetalert2-react-content'
import Swal from 'sweetalert2'
import axios from '../axios'

const mySwal = withReactContent(Swal)

// botón agregar usuario.
export function btnAddShowUserModal() {
	return async (dispatch) => {
		dispatch(dispatchAddShowUserModal())
	}
}

const dispatchAddShowUserModal = () => ({
	type: type.ADD_SHOW_USER_MODAL
})

// Cerrar ventana modal.
// ==================================================
export function btnCloseShowUserModal() {
	return async (dispatch) => {
		dispatch(dispatchCloseShowUserModal())
	}
}

const dispatchCloseShowUserModal = () => ({
	type: type.CLOSE_SHOW_USER_MODAL
})

// botón editar usuario.
// ==================================================
export function btnEditShowUserModal(user) {
	return async (dispatch) => {
		dispatch(dispatchEditShowUserModal(user))
	}
}

const dispatchEditShowUserModal = (user) => ({
	type: type.EDIT_SHOW_USER_MODAL,
	payload: user
})

// Cargar usuarios.
// ==================================================
export function LoadUsersAction(page = 1, search = '') {
	return async (dispatch) => {
		dispatch(dispatchLoadUsers())
		try {
			await axios.get(`users?page=${page}&search=${search}`).then(({data}) => {
				dispatch(dispatchLoadUsersSuccessfully(data))
			})
		} catch (err) {
			console.log(err)
			dispatch(dispatchLoadUsersError())
		}
	}
}

const dispatchLoadUsers = () => ({
	type: type.LOAD_USERS
})

const dispatchLoadUsersSuccessfully = (users) => ({
	type: type.LOAD_USERS_SUCCESSFULLY,
	payload: users
})

const dispatchLoadUsersError = () => ({
	type: type.LOAD_USERS_ERROR
})

// Crear nuevos usuarios.
// ==================================================
export function AddUserAction(user) {
	return async (dispatch) => {
		dispatch(dispatchAddUser())
		try {
			await axios.post('users', user).then(({data}) => {
				user.id = data
				dispatch(dispatchAddUserSuccessfully(user))
			})
		} catch (err) {
			console.log(err)
			dispatch(dispatchAddUserError())
			await mySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const dispatchAddUser = () => ({
	type: type.ADD_USER
})

const dispatchAddUserSuccessfully = user => ({
	type: type.ADD_USER_SUCCESSFULLY,
	payload: user
})

const dispatchAddUserError = () => ({
	type: type.ADD_USER_ERROR
})

// Editar usuarios.
// ==================================================
export function EditUserAction(user) {
	return async (dispatch) => {
		dispatch(dispatchEditUser())
		try {
			await axios.put(`users/${user.id}`, user)
			dispatch(dispatchEditUserSuccessfully(user))
		} catch (err) {
			console.log(err)
			dispatch(dispatchEditUserError())
			await mySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const dispatchEditUser = () => ({
	type: type.EDIT_USER
})

const dispatchEditUserSuccessfully = (user) => ({
	type: type.EDIT_USER_SUCCESSFULLY,
	payload: user
})

const dispatchEditUserError = () => ({
	type: type.EDIT_USER_ERROR
})

// Borrar usuarios.
// ==================================================
export function DeleteUserAction(id) {
	return async (dispatch) => {
		dispatch(dispatchDeleteUser())
		try {
			await axios.delete(`users/${id}`)
			dispatch(dispatchDeleteUserSuccessfully(id))
		} catch (err) {
			console.log(err)
			dispatch(dispatchDeleteUserError())
			await mySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const dispatchDeleteUser = () => ({
	type: type.DELETE_USER
})

const dispatchDeleteUserSuccessfully = (id) => ({
	type: type.DELETE_USER_SUCCESSFULLY,
	payload: id
})

const dispatchDeleteUserError = () => ({
	type: type.DELETE_USER_ERROR
})

// Cambiar estado activo/suspendido usuario.
// ==================================================
export function ChangeStatusUserAccountAction(user) {
	return async (dispatch) => {
		dispatch(dispatchEditUser())
		try {
			const id = user.id
			const status = user.suspended
			await axios.patch(`users/change_status/${id}/${status}`)
			dispatch(dispatchChangeStatusUserAccount(user))
		} catch (err) {
			console.log(err)
			dispatch(dispatchEditUserError())
			await mySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const dispatchChangeStatusUserAccount = (user) => ({
	type: type.CHANGE_STATUS_USER_ACCOUNT,
	payload: user
})
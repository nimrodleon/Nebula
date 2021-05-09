import * as type from '../types'
import Swal from 'sweetalert2'
import withReactContent from 'sweetalert2-react-content'
import axios from '../axios'

const MySwal = withReactContent(Swal)

// botón agregar contacto.
export function btnAddShowContactModal() {
	return async (dispatch) => {
		dispatch(dispatchAddShowContactModal())
	}
}

const dispatchAddShowContactModal = () => ({
	type: type.ADD_SHOW_CONTACT_MODAL
})

// Cerrar ventana modal.
// ==================================================
export function btnCloseShowContactModal() {
	return async (dispatch) => {
		dispatch(dispatchCloseShowContactModal())
	}
}

const dispatchCloseShowContactModal = () => ({
	type: type.CLOSE_SHOW_CONTACT_MODAL
})

// botón editar contacto.
// ==================================================
export function btnEditShowContactModal(contact) {
	return async (dispatch) => {
		dispatch(dispatchEditShowContactModal(contact))
	}
}

const dispatchEditShowContactModal = (contact) => ({
	type: type.EDIT_SHOW_CONTACT_MODAL,
	payload: contact
})

// Cargar contactos.
// ==================================================
export function LoadContactsAction(page = 1, search = '') {
	return async (dispatch) => {
		dispatch(dispatchLoadContacts())
		try {
			await axios.get(`contacts?page=${page}&search=${search}`).then(({data}) => {
				dispatch(dispatchLoadContactsSuccessfully(data))
			})
		} catch (err) {
			console.log(err)
			dispatch(dispatchLoadContactsError())
		}
	}
}

const dispatchLoadContacts = () => ({
	type: type.LOAD_CONTACTS
})

const dispatchLoadContactsSuccessfully = (contacts) => ({
	type: type.LOAD_CONTACTS_SUCCESSFULLY,
	payload: contacts
})

const dispatchLoadContactsError = () => ({
	type: type.LOAD_CONTACTS_ERROR
})

// Crear nuevos contactos.
// ==================================================
export function AddContactAction(contact) {
	return async (dispatch) => {
		dispatch(addContact())
		try {
			// enviar query a la api rest.
			await axios.post('contacts', contact).then(({data}) => {
				contact.id = data
				dispatch(addContactSuccessfully(contact))
				dispatch(dispatchCloseShowContactModal())
			})
		} catch (err) {
			console.log(err)
			// Si hay un error cambiar el state.
			dispatch(addContactError())
			// Alerta de error.
			await MySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const addContact = () => ({
	type: type.ADD_CONTACT
})

// si el contacto se guarda en la base de datos.
const addContactSuccessfully = contact => ({
	type: type.ADD_CONTACT_SUCCESSFULLY,
	payload: contact
})

// si hubo un error.
const addContactError = () => ({
	type: type.ADD_CONTACT_ERROR
})

// Editar Contactos.
// ==================================================
export function EditContactAction(contact) {
	return async (dispatch) => {
		dispatch(dispatchEditContact())
		try {
			await axios.put(`contacts/${contact.id}`, contact)
			dispatch(dispatchEditContactSuccessfully(contact))
		} catch (err) {
			console.log(err)
			dispatch(dispatchEditContactError())
			await MySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const dispatchEditContact = () => ({
	type: type.EDIT_CONTACT
})

const dispatchEditContactSuccessfully = (contact) => ({
	type: type.EDIT_CONTACT_SUCCESSFULLY,
	payload: contact
})

const dispatchEditContactError = () => ({
	type: type.EDIT_CONTACT_ERROR
})

// Borrar Contactos.
// ==================================================
export function DeleteContactAction(id) {
	return async (dispatch) => {
		dispatch(dispatchDeleteContact())
		try {
			await axios.delete(`contacts/${id}`)
			dispatch(dispatchDeleteContactSuccessfully(id))
		} catch (err) {
			console.log(err)
			dispatch(dispatchDeleteContactError())
			await MySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const dispatchDeleteContact = () => ({
	type: type.DELETE_CONTACT
})

const dispatchDeleteContactSuccessfully = (id) => ({
	type: type.DELETE_CONTACT_SUCCESSFULLY,
	payload: id
})

const dispatchDeleteContactError = () => ({
	type: type.DELETE_CONTACT_ERROR
})

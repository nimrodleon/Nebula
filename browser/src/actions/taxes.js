import * as type from '../types'
import withReactContent from 'sweetalert2-react-content'
import Swal from 'sweetalert2'
import axios from '../axios'

const mySwal = withReactContent(Swal)

// botón agregar impuesto.
export function btnAddShowTaxModal() {
	return async (dispatch) => {
		dispatch(dispatchAddShowTaxModal())
	}
}

const dispatchAddShowTaxModal = () => ({
	type: type.ADD_SHOW_TAX_MODAL
})

// Cerrar ventana modal.
// ==================================================
export function btnCloseShowTaxModal() {
	return async (dispatch) => {
		dispatch(dispatchCloseShowTaxModal())
	}
}

const dispatchCloseShowTaxModal = () => ({
	type: type.CLOSE_SHOW_TAX_MODAL
})

// botón editar impuesto.
// ==================================================
export function btnEditShowTaxModal(tax) {
	return async (dispatch) => {
		dispatch(dispatchEditShowTaxModal(tax))
	}
}

const dispatchEditShowTaxModal = (tax) => ({
	type: type.EDIT_SHOW_TAX_MODAL,
	payload: tax
})

// Cargar impuestos.
// ==================================================
export function LoadTaxesAction(page = 1, search = '') {
	return async (dispatch) => {
		dispatch(dispatchLoadTaxes())
		try {
			await axios.get(`taxes?page=${page}&search=${search}`).then(({data}) => {
				dispatch(dispatchLoadTaxesSuccessfully(data))
			})
		} catch (err) {
			console.log(err)
			dispatch(dispatchLoadTaxesError())
		}
	}
}

const dispatchLoadTaxes = () => ({
	type: type.LOAD_TAXES
})

const dispatchLoadTaxesSuccessfully = (taxes) => ({
	type: type.LOAD_TAXES_SUCCESSFULLY,
	payload: taxes
})

const dispatchLoadTaxesError = () => ({
	type: type.LOAD_TAXES_ERROR
})

// Crear nuevos impuestos.
// ==================================================
export function AddTaxAction(tax) {
	return async (dispatch) => {
		dispatch(dispatchAddTax())
		try {
			await axios.post('taxes', tax).then(({data}) => {
				tax.id = data
				dispatch(dispatchAddTaxSuccessfully(tax))
			})
		} catch (err) {
			console.log(err)
			dispatch(dispatchAddTaxError())
			await mySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const dispatchAddTax = () => ({
	type: type.ADD_TAX
})

const dispatchAddTaxSuccessfully = tax => ({
	type: type.ADD_TAX_SUCCESSFULLY,
	payload: tax
})

const dispatchAddTaxError = () => ({
	type: type.ADD_TAX_ERROR
})

// Editar impuestos.
// ==================================================
export function EditTaxAction(tax) {
	return async (dispatch) => {
		dispatch(dispatchEditTax())
		try {
			await axios.put(`taxes/${tax.id}`, tax)
			dispatch(dispatchEditTaxSuccessfully(tax))
		} catch (err) {
			console.log(err)
			dispatch(dispatchEditTaxError())
			await mySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const dispatchEditTax = () => ({
	type: type.EDIT_TAX
})

const dispatchEditTaxSuccessfully = (tax) => ({
	type: type.EDIT_TAX_SUCCESSFULLY,
	payload: tax
})

const dispatchEditTaxError = () => ({
	type: type.EDIT_TAX_ERROR
})

// Borrar impuestos.
// ==================================================
export function DeleteTaxAction(id) {
	return async (dispatch) => {
		dispatch(dispatchDeleteTax())
		try {
			await axios.delete(`taxes/${id}`)
			dispatch(dispatchDeleteTaxSuccessfully(id))
		} catch (err) {
			console.log(err)
			dispatch(dispatchDeleteTaxError())
			await mySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const dispatchDeleteTax = () => ({
	type: type.DELETE_TAX
})

const dispatchDeleteTaxSuccessfully = (id) => ({
	type: type.DELETE_TAX_SUCCESSFULLY,
	payload: id
})

const dispatchDeleteTaxError = () => ({
	type: type.DELETE_TAX_ERROR
})

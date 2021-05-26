import * as type from '../types'
import withReactContent from 'sweetalert2-react-content'
import Swal from 'sweetalert2'
import axios from '../axios'

const mySwal = withReactContent(Swal)

// botón agregar almacén.
export function btnAddShowWarehouseModal() {
	return async (dispatch) => {
		dispatch(dispatchAddShowWarehouseModal())
	}
}

const dispatchAddShowWarehouseModal = () => ({
	type: type.ADD_SHOW_WAREHOUSE_MODAL
})

// Cerrar ventana modal.
// ==================================================
export function btnCloseShowWarehouseModal() {
	return async (dispatch) => {
		dispatch(dispatchCloseShowWarehouseModal())
	}
}

const dispatchCloseShowWarehouseModal = () => ({
	type: type.CLOSE_SHOW_WAREHOUSE_MODAL
})

// botón editar almacén.
// ==================================================
export function btnEditShowWarehouseModal(warehouse) {
	return async (dispatch) => {
		dispatch(dispatchEditShowWarehouseModal(warehouse))
	}
}

const dispatchEditShowWarehouseModal = (warehouse) => ({
	type: type.EDIT_SHOW_WAREHOUSE_MODAL,
	payload: warehouse
})

// Cargar almacenes.
// ==================================================
export function LoadWarehousesAction(page = 1, search = '') {
	return async (dispatch) => {
		dispatch(dispatchLoadWarehouses())
		try {
			await axios.get(`warehouses?page=${page}&search=${search}`).then(({data}) => {
				dispatch(dispatchLoadWarehousesSuccessfully(data))
			})
		} catch (err) {
			console.log(err)
			dispatch(dispatchLoadWarehousesError())
		}
	}
}

const dispatchLoadWarehouses = () => ({
	type: type.LOAD_WAREHOUSES
})

const dispatchLoadWarehousesSuccessfully = (warehouses) => ({
	type: type.LOAD_WAREHOUSES_SUCCESSFULLY,
	payload: warehouses
})

const dispatchLoadWarehousesError = () => ({
	type: type.LOAD_WAREHOUSES_ERROR
})

// Crear nuevos almacenes.
// ==================================================
export function AddWarehouseAction(warehouse) {
	return async (dispatch) => {
		dispatch(dispatchAddWarehouse())
		try {
			await axios.post('warehouses', warehouse).then(({data}) => {
				warehouse.id = data
				dispatch(dispatchAddWarehouseSuccessfully(warehouse))
			})
		} catch (err) {
			console.log(err)
			dispatch(dispatchAddWarehouseError())
			await mySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const dispatchAddWarehouse = () => ({
	type: type.ADD_WAREHOUSE
})

const dispatchAddWarehouseSuccessfully = warehouse => ({
	type: type.ADD_WAREHOUSE_SUCCESSFULLY,
	payload: warehouse
})

const dispatchAddWarehouseError = () => ({
	type: type.ADD_WAREHOUSE_ERROR
})

// Editar almacenes.
// ==================================================
export function EditWarehouseAction(warehouse) {
	return async (dispatch) => {
		dispatch(dispatchEditWarehouse())
		try {
			await axios.put(`warehouses/${warehouse.id}`, warehouse)
			dispatch(dispatchEditWarehouseSuccessfully(warehouse))
		} catch (err) {
			console.log(err)
			dispatch(dispatchEditWarehouseError())
			await mySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const dispatchEditWarehouse = () => ({
	type: type.EDIT_WAREHOUSE
})

const dispatchEditWarehouseSuccessfully = (warehouse) => ({
	type: type.EDIT_WAREHOUSE_SUCCESSFULLY,
	payload: warehouse
})

const dispatchEditWarehouseError = () => ({
	type: type.EDIT_WAREHOUSE_ERROR
})

// Borrar almacén.
// ==================================================
export function DeleteWarehouseAction(id) {
	return async (dispatch) => {
		dispatch(dispatchDeleteWarehouse())
		try {
			await axios.delete(`warehouses/${id}`)
			dispatch(dispatchDeleteWarehouseSuccessfully(id))
		} catch (err) {
			console.log(err)
			dispatch(dispatchDeleteWarehouseError())
			await mySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const dispatchDeleteWarehouse = () => ({
	type: type.DELETE_WAREHOUSE
})

const dispatchDeleteWarehouseSuccessfully = (id) => ({
	type: type.DELETE_WAREHOUSE_SUCCESSFULLY,
	payload: id
})

const dispatchDeleteWarehouseError = () => ({
	type: type.DELETE_WAREHOUSE_ERROR
})

import * as type from '../types'
import withReactContent from 'sweetalert2-react-content'
import Swal from 'sweetalert2'
import axios from '../axios'

const mySwal = withReactContent(Swal)

// Cargar ordenes de reparación.
// ==================================================
export function LoadOrderRepairsAction(page = 1, search = '') {
	return async (dispatch) => {
		dispatch(dispatchLoadOrderRepairs())
		try {
			await axios.get(`order_repairs?page=${page}&search=${search}`).then(({data}) => {
				dispatch(dispatchLoadOrderRepairsSuccessfully(data))
			})
		} catch (err) {
			console.log(err)
			dispatch(dispatchLoadOrderRepairsError())
		}
	}
}

const dispatchLoadOrderRepairs = () => ({
	type: type.LOAD_ORDER_REPAIRS
})

const dispatchLoadOrderRepairsSuccessfully = (orders) => ({
	type: type.LOAD_ORDER_REPAIRS_SUCCESSFULLY,
	payload: orders
})

const dispatchLoadOrderRepairsError = () => ({
	type: type.LOAD_ORDER_REPAIRS_ERROR
})

// Crear nuevas ordenes de reparación.
// ==================================================
export function AddOrderRepairAction(order) {
	return async (dispatch) => {
		dispatch(dispatchAddOrderRepair())
		try {
			await axios.post('order_repairs', order).then(({data}) => {
				order.id = data
				dispatch(dispatchAddOrderRepairSuccessfully(order))
			})
		} catch (err) {
			console.log(err)
			dispatch(dispatchAddOrderRepairError())
			await mySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const dispatchAddOrderRepair = () => ({
	type: type.ADD_ORDER_REPAIR
})

// dispatchAddOrderRepairSuccessfully agregar una orden al la lista de estados.
// el parámetro order es innecesario porque el formulario de registro se ejecuta en una ruta diferente a la lista de ordenes de reparación,
// cada vez que se redirige a al lista de ordenes de reparación se ejecuta [LoadOrderRepairsAction] volviendo cargar todas las ordenes. 
const dispatchAddOrderRepairSuccessfully = order => ({
	type: type.ADD_ORDER_REPAIR_SUCCESSFULLY,
	payload: order // argumento innecesario.
})

const dispatchAddOrderRepairError = () => ({
	type: type.ADD_ORDER_REPAIR_ERROR
})

// Editar ordenes de reparación.
// ==================================================
export function EditOrderRepairAction(order) {
	return async (dispatch) => {
		dispatch(dispatchEditOrderRepairType())
		try {
			await axios.put(`order_repairs/${order.id}`, order)
			dispatch(dispatchEditOrderRepairSuccessfully(order))
		} catch (err) {
			console.log(err)
			dispatch(dispatchEditOrderRepairError())
			await mySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const dispatchEditOrderRepairType = () => ({
	type: type.EDIT_ORDER_REPAIR
})

const dispatchEditOrderRepairSuccessfully = (order) => ({
	type: type.EDIT_ORDER_REPAIR_SUCCESSFULLY,
	payload: order
})

const dispatchEditOrderRepairError = () => ({
	type: type.EDIT_ORDER_REPAIR_ERROR
})

// Borrar orden de reparación.
// ==================================================
export function DeleteOrderRepairAction(id) {
	return async (dispatch) => {
		dispatch(dispatchDeleteOrderRepair())
		try {
			await axios.delete(`order_repairs/${id}`)
			dispatch(dispatchDeleteOrderRepairSuccessfully(id))
		} catch (err) {
			console.log(err)
			dispatch(dispatchDeleteOrderRepairError())
			await mySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const dispatchDeleteOrderRepair = () => ({
	type: type.DELETE_ORDER_REPAIR
})

const dispatchDeleteOrderRepairSuccessfully = (id) => ({
	type: type.DELETE_ORDER_REPAIR_SUCCESSFULLY,
	payload: id
})

const dispatchDeleteOrderRepairError = () => ({
	type: type.DELETE_ORDER_REPAIR_ERROR
})

// Acciones de botones.
// ==================================================

// botón agregar orden de reparación.
export function btnAddOrderRepair() {
	return async (dispatch) => {
		dispatch(dispatchBtnAddOrderRepair())
	}
}

const dispatchBtnAddOrderRepair = () => ({
	type: type.BTN_ADD_ORDER_REPAIR
})

// botón editar orden de reparación.
export function editFormOrderRepair(id) {
	return async (dispatch) => {
		axios.get(`order_repairs/${id}`).then(({data}) => {
			dispatch(dispatchEditFormOrderRepair(data))
		})
	}
}

const dispatchEditFormOrderRepair = (order) => ({
	type: type.EDIT_FORM_ORDER_REPAIR,
	payload: order
})
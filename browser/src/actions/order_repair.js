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

const dispatchAddOrderRepairSuccessfully = order => ({
	type: type.ADD_ORDER_REPAIR_SUCCESSFULLY,
	payload: order
})

const dispatchAddOrderRepairError = () => ({
	type: type.ADD_ORDER_REPAIR_ERROR
})

// Editar ordenes de reparación.
// ==================================================
export function EditOrderRepairAction(order) {
	return async (dispatch) => {
		dispatch(dispatchOrderRepairType())
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

const dispatchOrderRepairType = () => ({
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

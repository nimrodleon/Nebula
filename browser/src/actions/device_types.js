import * as type from '../types'
import withReactContent from 'sweetalert2-react-content'
import Swal from 'sweetalert2'
import axios from '../axios'

const mySwal = withReactContent(Swal)

// botón agregar tipo de equipo.
export function btnAddShowDeviceTypeModal() {
	return async (dispatch) => {
		dispatch(dispatchAddShowDeviceTypeModal())
	}
}

const dispatchAddShowDeviceTypeModal = () => ({
	type: type.ADD_SHOW_DEVICE_TYPE_MODAL
})

// Cerrar ventana modal.
// ==================================================
export function btnCloseShowDeviceTypeModal() {
	return async (dispatch) => {
		dispatch(dispatchCloseShowDeviceTypeModal())
	}
}

const dispatchCloseShowDeviceTypeModal = () => ({
	type: type.CLOSE_SHOW_DEVICE_TYPE_MODAL
})

// botón editar tipo de equipo.
// ==================================================
export function btnEditShowDeviceTypeModal(deviceType) {
	return async (dispatch) => {
		dispatch(dispatchEditShowDeviceTypeModal(deviceType))
	}
}

const dispatchEditShowDeviceTypeModal = (deviceType) => ({
	type: type.EDIT_SHOW_DEVICE_TYPE_MODAL,
	payload: deviceType
})

// Cargar tipos de equipo.
// ==================================================
export function LoadDeviceTypesAction(page = 1, search = '') {
	return async (dispatch) => {
		dispatch(dispatchLoadDeviceTypes())
		try {
			await axios.get(`device_types?page=${page}&search=${search}`).then(({data}) => {
				dispatch(dispatchLoadDeviceTypesSuccessfully(data))
			})
		} catch (err) {
			console.log(err)
			dispatch(dispatchLoadDeviceTypesError())
		}
	}
}

const dispatchLoadDeviceTypes = () => ({
	type: type.LOAD_DEVICE_TYPES
})

const dispatchLoadDeviceTypesSuccessfully = (deviceTypes) => ({
	type: type.LOAD_DEVICES_TYPES_SUCCESSFULLY,
	payload: deviceTypes
})

const dispatchLoadDeviceTypesError = () => ({
	type: type.LOAD_DEVICE_TYPES_ERROR
})

// Crear nuevos tipos de equipo.
// ==================================================
export function AddDeviceTypeAction(deviceType) {
	return async (dispatch) => {
		dispatch(dispatchAddDeviceType())
		try {
			await axios.post('device_types', deviceType).then(({data}) => {
				deviceType.id = data
				dispatch(dispatchAddDeviceTypeSuccessfully(deviceType))
			})
		} catch (err) {
			console.log(err)
			dispatch(dispatchAddDeviceTypeError())
			await mySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const dispatchAddDeviceType = () => ({
	type: type.ADD_DEVICE_TYPE
})

const dispatchAddDeviceTypeSuccessfully = deviceType => ({
	type: type.ADD_DEVICE_TYPE_SUCCESSFULLY,
	payload: deviceType
})

const dispatchAddDeviceTypeError = () => ({
	type: type.ADD_DEVICE_TYPE_ERROR
})

// Editar tipos de equipo.
// ==================================================
export function EditDeviceTypeAction(deviceType) {
	return async (dispatch) => {
		dispatch(dispatchEditDeviceType())
		try {
			await axios.put(`device_types/${deviceType.id}`, deviceType)
			dispatch(dispatchEditDeviceTypeSuccessfully(deviceType))
		} catch (err) {
			console.log(err)
			dispatch(dispatchEditDeviceTypeError())
			await mySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const dispatchEditDeviceType = () => ({
	type: type.EDIT_DEVICE_TYPE
})

const dispatchEditDeviceTypeSuccessfully = (deviceType) => ({
	type: type.EDIT_DEVICE_TYPE_SUCCESSFULLY,
	payload: deviceType
})

const dispatchEditDeviceTypeError = () => ({
	type: type.EDIT_DEVICE_TYPE_ERROR
})

// Borrar tipos de equipo.
// ==================================================
export function DeleteDeviceTypeAction(id) {
	return async (dispatch) => {
		dispatch(dispatchDeleteDeviceType())
		try {
			await axios.delete(`device_types/${id}`)
			dispatch(dispatchDeleteDeviceTypeSuccessfully(id))
		} catch (err) {
			console.log(err)
			dispatch(dispatchDeleteDeviceTypeError())
			await mySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const dispatchDeleteDeviceType = () => ({
	type: type.DELETE_DEVICE_TYPE
})

const dispatchDeleteDeviceTypeSuccessfully = (id) => ({
	type: type.DELETE_DEVICE_TYPE_SUCCESSFULLY,
	payload: id
})

const dispatchDeleteDeviceTypeError = () => ({
	type: type.DELETE_DEVICE_TYPE_ERROR
})

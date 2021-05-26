import * as type from '../types'

const initialState = {
	deviceTypes: [],
	currentDeviceType: {},
	showModal: false,
	title: '',
	typeOperation: '',
	error: false,
	loading: false
}

export default function deviceTypesReducer(state = initialState, action) {
	switch (action.type) {
		case type.CLOSE_SHOW_DEVICE_TYPE_MODAL:
			return {
				...state,
				showModal: false
			}
		case type.ADD_SHOW_DEVICE_TYPE_MODAL:
			return {
				...state,
				currentDeviceType: {},
				title: 'Agregar Tipo de Equipo',
				typeOperation: 'ADD',
				showModal: true
			}
		case type.EDIT_SHOW_DEVICE_TYPE_MODAL:
			return {
				...state,
				currentDeviceType: action.payload,
				title: 'Editar Tipo de Equipo',
				typeOperation: 'EDIT',
				showModal: true
			}
		// Crud Tax Actions.
		case type.LOAD_DEVICE_TYPES:
		case type.ADD_DEVICE_TYPE:
		case type.EDIT_DEVICE_TYPE:
		case type.DELETE_DEVICE_TYPE:
			return {
				...state,
				loading: true,
				error: false
			}
		case type.LOAD_DEVICES_TYPES_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				deviceTypes: action.payload
			}
		case type.ADD_DEVICE_TYPE_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				deviceTypes: [action.payload, ...state.deviceTypes],
				showModal: false
			}
		case type.EDIT_DEVICE_TYPE_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				currentDeviceType: {},
				deviceTypes: state.deviceTypes.map(d => d.id === action.payload.id ? d = action.payload : d),
				showModal: false
			}
		case type.DELETE_DEVICE_TYPE_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				deviceTypes: state.deviceTypes.filter(e => e.id !== action.payload)
			}
		case type.LOAD_DEVICE_TYPES_ERROR:
		case type.ADD_DEVICE_TYPE_ERROR:
		case type.EDIT_DEVICE_TYPE_ERROR:
		case type.DELETE_DEVICE_TYPE_ERROR:
			return {
				...state,
				loading: false,
				error: true
			}
		default:
			return state
	}
}
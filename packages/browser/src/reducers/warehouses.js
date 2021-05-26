import * as type from '../types'

const initialState = {
	warehouses: [],
	currentWarehouse: {},
	showModal: false,
	title: '',
	typeOperation: '',
	error: false,
	loading: false
}

export default function warehousesReducer(state = initialState, action) {
	switch (action.type) {
		case type.CLOSE_SHOW_WAREHOUSE_MODAL:
			return {
				...state,
				showModal: false
			}
		case type.ADD_SHOW_WAREHOUSE_MODAL:
			return {
				...state,
				currentWarehouse: {},
				title: 'Agregar Almacén',
				typeOperation: 'ADD',
				showModal: true
			}
		case type.EDIT_SHOW_WAREHOUSE_MODAL:
			return {
				...state,
				currentWarehouse: action.payload,
				title: 'Editar Almacén',
				typeOperation: 'EDIT',
				showModal: true
			}
		// Crud Tax Actions.
		case type.LOAD_WAREHOUSES:
		case type.ADD_WAREHOUSE:
		case type.EDIT_WAREHOUSE:
		case type.DELETE_WAREHOUSE:
			return {
				...state,
				loading: true,
				error: false
			}
		case type.LOAD_WAREHOUSES_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				warehouses: action.payload
			}
		case type.ADD_WAREHOUSE_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				warehouses: [action.payload, ...state.warehouses],
				showModal: false
			}
		case type.EDIT_WAREHOUSE_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				currentWarehouse: {},
				warehouses: state.warehouses.map(w => w.id === action.payload.id ? w = action.payload : w),
				showModal: false
			}
		case type.DELETE_WAREHOUSE_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				warehouses: state.warehouses.filter(w => w.id !== action.payload)
			}
		case type.LOAD_WAREHOUSES_ERROR:
		case type.ADD_WAREHOUSE_ERROR:
		case type.EDIT_WAREHOUSE_ERROR:
		case type.DELETE_WAREHOUSE_ERROR:
			return {
				...state,
				loading: false,
				error: true
			}
		default:
			return state
	}
}
import * as type from '../types'

const initialState = {
	orderRepairs: [],
	currentOrderRepair: {},
	typeOperation: '',
	error: false,
	loading: false
}

export default function orderRepairReducer(state = initialState, action) {
	switch (action.type) {
		// Crud Tax Actions.
		case type.LOAD_ORDER_REPAIRS:
		case type.ADD_ORDER_REPAIR:
		case type.EDIT_ORDER_REPAIR:
		case type.DELETE_ORDER_REPAIR:
			return {
				...state,
				loading: true,
				error: false
			}
		case type.LOAD_ORDER_REPAIRS_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				orderRepairs: action.payload
			}
		case type.ADD_ORDER_REPAIR_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				orderRepairs: [action.payload, ...state.orderRepairs]
			}
		case type.EDIT_ORDER_REPAIR_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				currentOrderRepair: {},
				orderRepairs: state.orderRepairs.map(o => o.id === action.payload.id ? o = action.payload : o)
			}
		case type.DELETE_ORDER_REPAIR_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				orderRepairs: state.orderRepairs.filter(o => o.id !== action.payload)
			}
		case type.LOAD_ORDER_REPAIRS_ERROR:
		case type.ADD_ORDER_REPAIR_ERROR:
		case type.EDIT_ORDER_REPAIR_ERROR:
		case type.DELETE_ORDER_REPAIR_ERROR:
			return {
				...state,
				loading: false,
				error: true
			}
		default:
			return state
	}
}
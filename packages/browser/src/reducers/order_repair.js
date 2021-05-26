import * as type from '../types'

const initialState = {
	orderRepairs: [],
	currentOrderRepair: {},
	showModal: false,
	typeOperation: '',
	error: false,
	loading: false
}

export default function orderRepairReducer(state = initialState, action) {
	switch (action.type) {
		case type.BTN_ADD_ORDER_REPAIR:
			return {
				...state,
				currentOrderRepair: {},
				typeOperation: 'ADD'
			}
		case type.EDIT_FORM_ORDER_REPAIR:
			return {
				...state,
				currentOrderRepair: action.payload,
				typeOperation: 'EDIT'
			}
		case type.OPEN_MODAL_ORDER_REPAIR:
			return {
				...state,
				currentOrderRepair: action.payload,
				showModal: true,
			}
		case type.CLOSE_MODAL_ORDER_REPAIR:
			return {
				...state,
				showModal: false
			}
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
		case  type.ADD_ORDER_REPAIR_SUCCESSFULLY:
		case type.EDIT_ORDER_REPAIR_SUCCESSFULLY:
		case type.DELETE_ORDER_REPAIR_SUCCESSFULLY:
			return {
				...state,
				loading: false
			}
		// case type.ADD_ORDER_REPAIR_SUCCESSFULLY:
		// 	return {
		// 		...state,
		// 		loading: false,
		// 		// [orderRepairs] => argumento innecesario.
		// 		// orderRepairs: [action.payload, ...state.orderRepairs]
		// 	}
		// case type.EDIT_ORDER_REPAIR_SUCCESSFULLY:
		// 	return {
		// 		...state,
		// 		loading: false,
		// 		// currentOrderRepair: {},
		// 		// [orderRepairs] => argumento innecesario.
		// 		// orderRepairs: state.orderRepairs.map(o => o.id === action.payload.id ? o = action.payload : o)
		// 	}
		// case type.DELETE_ORDER_REPAIR_SUCCESSFULLY:
		// 	return {
		// 		...state,
		// 		loading: false,
		// 		// [orderRepairs] => argumento innecesario.
		// 		// orderRepairs: state.orderRepairs.filter(o => o.id !== action.payload)
		// 	}
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
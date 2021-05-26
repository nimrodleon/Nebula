import * as type from '../types'

const initialState = {
	taxes: [],
	currentTax: {},
	showModal: false,
	title: '',
	typeOperation: '',
	error: false,
	loading: false
}

export default function taxesReducer(state = initialState, action) {
	switch (action.type) {
		case type.CLOSE_SHOW_TAX_MODAL:
			return {
				...state,
				showModal: false
			}
		case type.ADD_SHOW_TAX_MODAL:
			return {
				...state,
				currentTax: {},
				title: 'Agregar Impuesto',
				typeOperation: 'ADD',
				showModal: true
			}
		case type.EDIT_SHOW_TAX_MODAL:
			return {
				...state,
				currentTax: action.payload,
				title: 'Editar Impuesto',
				typeOperation: 'EDIT',
				showModal: true
			}
		// Crud Tax Actions.
		case type.LOAD_TAXES:
		case type.ADD_TAX:
		case type.EDIT_TAX:
		case type.DELETE_TAX:
			return {
				...state,
				loading: true,
				error: false
			}
		case type.LOAD_TAXES_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				taxes: action.payload
			}
		case type.ADD_TAX_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				taxes: [action.payload, ...state.taxes],
				showModal: false
			}
		case type.EDIT_TAX_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				currentTax: {},
				taxes: state.taxes.map(t => t.id === action.payload.id ? t = action.payload : t),
				showModal: false
			}
		case type.DELETE_TAX_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				taxes: state.taxes.filter(t => t.id !== action.payload)
			}
		case type.LOAD_TAXES_ERROR:
		case type.ADD_TAX_ERROR:
		case type.EDIT_TAX_ERROR:
		case type.DELETE_TAX_ERROR:
			return {
				...state,
				loading: false,
				error: true
			}
		default:
			return state
	}
}
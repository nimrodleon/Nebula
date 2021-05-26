import * as type from '../types'

const initialState = {
	users: [],
	currentUser: {},
	showModal: false,
	title: '',
	typeOperation: '',
	error: false,
	loading: false
}

export default function usersReducer(state = initialState, action) {
	switch (action.type) {
		case type.CLOSE_SHOW_USER_MODAL:
			return {
				...state,
				showModal: false
			}
		case type.ADD_SHOW_USER_MODAL:
			return {
				...state,
				currentUser: {},
				title: 'Agregar Usuario',
				typeOperation: 'ADD',
				showModal: true
			}
		case type.EDIT_SHOW_USER_MODAL:
			return {
				...state,
				currentUser: action.payload,
				title: 'Editar Usuario',
				typeOperation: 'EDIT',
				showModal: true
			}
		// Crud Tax Actions.
		case type.LOAD_USERS:
		case type.ADD_USER:
		case type.EDIT_USER:
		case type.DELETE_USER:
			return {
				...state,
				loading: true,
				error: false
			}
		case type.LOAD_USERS_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				users: action.payload
			}
		case type.ADD_USER_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				users: [action.payload, ...state.users],
				showModal: false
			}
		case type.EDIT_USER_SUCCESSFULLY:
		case type.CHANGE_STATUS_USER_ACCOUNT:
			return {
				...state,
				loading: false,
				currentUser: {},
				users: state.users.map(u => u.id === action.payload.id ? u = action.payload : u),
				showModal: false
			}
		case type.DELETE_USER_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				users: state.users.filter(u => u.id !== action.payload)
			}
		case type.LOAD_USERS_ERROR:
		case type.ADD_USER_ERROR:
		case type.EDIT_USER_ERROR:
		case type.DELETE_USER_ERROR:
			return {
				...state,
				loading: false,
				error: true
			}
		default:
			return state
	}
}
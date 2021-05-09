import * as type from '../types'

const initialState = {
	contacts: [],
	currentContact: {},
	showModal: false,
	title: '',
	typeOperation: '',
	error: false,
	loading: false
}

export default function contactsReducer(state = initialState, action) {
	switch (action.type) {
		case type.ADD_SHOW_CONTACT_MODAL:
			return {
				...state,
				showModal: true,
				currentContact: {},
				title: 'Agregar Contacto',
				typeOperation: 'ADD',
				error: false
			}
		case type.CLOSE_SHOW_CONTACT_MODAL:
			return {
				...state,
				showModal: false
			}
		case type.EDIT_SHOW_CONTACT_MODAL:
			return {
				...state,
				showModal: true,
				currentContact: action.payload,
				title: 'Editar Contacto',
				typeOperation: 'EDIT',
				error: false
			}
		case type.LOAD_CONTACTS:
		case type.ADD_CONTACT:
		case type.EDIT_CONTACT:
		case type.DELETE_CONTACT:
			return {
				...state,
				loading: true
			}
		case type.LOAD_CONTACTS_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				contacts: action.payload,
				error: false
			}
		case type.ADD_CONTACT_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				contacts: [action.payload, ...state.contacts],
				showModal: false
			}
		case type.EDIT_CONTACT_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				currentContact: {},
				contacts: state.contacts.map(c => c.id === action.payload.id ? c = action.payload : c),
				showModal: false
			}
		case type.DELETE_CONTACT_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				contacts: state.contacts.filter(c => c.id !== action.payload)
			}
		case type.LOAD_CONTACTS_ERROR:
		case type.ADD_CONTACT_ERROR:
		case type.EDIT_CONTACT_ERROR:
			return {
				...state,
				loading: false,
				error: true
			}
		default:
			return state
	}
}
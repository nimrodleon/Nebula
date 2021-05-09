import * as type from '../types'

const initialState = {
	articles: [],
	currentArticle: {},
	showModal: false,
	title: '',
	typeOperation: '',
	error: false,
	loading: false
}

export default function articlesReducer(state = initialState, action) {
	switch (action.type) {
		case type.CLOSE_SHOW_ARTICLE_MODAL:
			return {
				...state,
				showModal: false
			}
		case type.ADD_SHOW_ARTICLE_MODAL:
			return {
				...state,
				currentArticle: {},
				title: 'Agregar Artículo',
				typeOperation: 'ADD',
				showModal: true
			}
		case type.EDIT_SHOW_ARTICLE_MODAL:
			return {
				...state,
				currentArticle: action.payload,
				title: 'Editar Artículo',
				typeOperation: 'EDIT',
				showModal: true
			}
		// Crud Articles Actions.
		case type.LOAD_ARTICLES:
		case type.ADD_ARTICLE:
		case type.EDIT_ARTICLE:
		case type.DELETE_ARTICLE:
			return {
				...state,
				loading: true,
				error: false
			}
		case type.LOAD_ARTICLES_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				articles: action.payload
			}
		case type.ADD_ARTICLE_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				articles: [action.payload, ...state.articles],
				showModal: false
			}
		case type.EDIT_ARTICLE_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				currentArticle: {},
				articles: state.articles.map(a => a.id === action.payload.id ? a = action.payload : a),
				showModal: false
			}
		case type.DELETE_ARTICLE_SUCCESSFULLY:
			return {
				...state,
				loading: false,
				articles: state.articles.filter(a => a.id !== action.payload)
			}
		case type.LOAD_ARTICLES_ERROR:
		case type.ADD_ARTICLE_ERROR:
		case type.EDIT_ARTICLE_ERROR:
		case type.DELETE_ARTICLE_ERROR:
			return {
				...state,
				loading: false,
				error: true
			}
		default:
			return state
	}
}

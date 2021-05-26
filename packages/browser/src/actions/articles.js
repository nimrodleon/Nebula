import * as type from '../types'
import withReactContent from 'sweetalert2-react-content'
import Swal from 'sweetalert2'
import axios from '../axios'
import {LoadTaxesAction} from './taxes'

const mySwal = withReactContent(Swal)

// botón agregar artículo.
export function btnAddShowArticleModal() {
	return async (dispatch) => {
		dispatch(dispatchAddShowArticleModal())
		// Cargar impuestos al estado global.
		dispatch(LoadTaxesAction(1, ''))
	}
}

const dispatchAddShowArticleModal = () => ({
	type: type.ADD_SHOW_ARTICLE_MODAL
})

// Cerrar ventana modal.
// ==================================================
export function btnCloseShowArticleModal() {
	return async (dispatch) => {
		dispatch(dispatchCloseShowArticleModal())
	}
}

const dispatchCloseShowArticleModal = () => ({
	type: type.CLOSE_SHOW_ARTICLE_MODAL
})

// botón editar artículo.
// ==================================================
export function btnEditShowArticleModal(article) {
	return async (dispatch) => {
		dispatch(dispatchEditShowArticleModal(article))
		// Cargar impuestos al estado global.
		dispatch(LoadTaxesAction(1, ''))
	}
}

const dispatchEditShowArticleModal = (article) => ({
	type: type.EDIT_SHOW_ARTICLE_MODAL,
	payload: article
})

// Cargar artículos.
// ==================================================
export function LoadArticlesAction(page = 1, search = '') {
	return async (dispatch) => {
		dispatch(dispatchLoadArticles())
		try {
			await axios.get(`articles?page=${page}&search=${search}`).then(({data}) => {
				dispatch(dispatchLoadArticlesSuccessfully(data))
			})
		} catch (err) {
			console.log(err)
			dispatch(dispatchLoadArticlesError())
		}
	}
}

const dispatchLoadArticles = () => ({
	type: type.LOAD_ARTICLES
})

const dispatchLoadArticlesSuccessfully = (articles) => ({
	type: type.LOAD_ARTICLES_SUCCESSFULLY,
	payload: articles
})

const dispatchLoadArticlesError = () => ({
	type: type.LOAD_ARTICLES_ERROR
})

// Crear nuevo artículo.
// ==================================================
export function AddArticleAction(article) {
	return async (dispatch) => {
		dispatch(dispatchAddArticle())
		try {
			await axios.post('articles', article).then(({data}) => {
				article.id = data
				dispatch(dispatchAddArticleSuccessfully(article))
			})
		} catch (err) {
			console.log(err)
			dispatch(dispatchAddArticleError())
			await mySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const dispatchAddArticle = () => ({
	type: type.ADD_ARTICLE
})

const dispatchAddArticleSuccessfully = article => ({
	type: type.ADD_ARTICLE_SUCCESSFULLY,
	payload: article
})

const dispatchAddArticleError = () => ({
	type: type.ADD_ARTICLE_ERROR
})

// Editar artículo.
// ==================================================
export function EditArticleAction(article) {
	return async (dispatch) => {
		dispatch(dispatchEditArticle())
		try {
			await axios.put(`articles/${article.id}`, article)
			dispatch(dispatchEditArticleSuccessfully(article))
		} catch (err) {
			console.log(err)
			dispatch(dispatchEditArticleError())
			await mySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const dispatchEditArticle = () => ({
	type: type.EDIT_ARTICLE
})

const dispatchEditArticleSuccessfully = (article) => ({
	type: type.EDIT_ARTICLE_SUCCESSFULLY,
	payload: article
})

const dispatchEditArticleError = () => ({
	type: type.EDIT_ARTICLE_ERROR
})

// Borrar artículo.
// ==================================================
export function DeleteArticleAction(id) {
	return async (dispatch) => {
		dispatch(dispatchDeleteArticle())
		try {
			await axios.delete(`articles/${id}`)
			dispatch(dispatchDeleteArticleSuccessfully(id))
		} catch (err) {
			console.log(err)
			dispatch(dispatchDeleteArticleError())
			await mySwal.fire({
				icon: 'error',
				title: 'Hubo un error',
				text: 'Hubo un error, intenta de nuevo'
			})
		}
	}
}

const dispatchDeleteArticle = () => ({
	type: type.DELETE_ARTICLE
})

const dispatchDeleteArticleSuccessfully = (id) => ({
	type: type.DELETE_ARTICLE_SUCCESSFULLY,
	payload: id
})

const dispatchDeleteArticleError = () => ({
	type: type.DELETE_ARTICLE_ERROR
})

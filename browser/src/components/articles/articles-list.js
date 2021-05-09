import React, {Fragment, useEffect, useState} from 'react'
import {WrapBody, WrapContainer, WrapHeader, WrapHeaderItem} from '../common/util'
import {Button, Form, Table} from 'react-bootstrap'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faPlus, faSearch} from '@fortawesome/free-solid-svg-icons'
import ArticleModalForm from './modal-form'
import {useDispatch, useSelector} from 'react-redux'
import {btnAddShowArticleModal, LoadArticlesAction} from '../../actions/articles'
import ArticleItem from './article-item'

/**
 * Lista de Artículos.
 * @returns {JSX.Element}
 * @constructor
 */
const ArticlesList = () => {
	const dispatch = useDispatch()

	// Variable para almacenar el texto a buscar.
	const [search, setSearch] = useState('')
	
	useEffect(() => {
		handleLoadArticles()
		// eslint-disable-next-line
	},[])

	// Variables de estado.
	const articles = useSelector(state => state.articles.articles)

	// Acciones redux.
	const handleAddArticle = () => dispatch(btnAddShowArticleModal())
	const handleLoadArticles = (page = 1, search = '') => dispatch(LoadArticlesAction(page, search))

	// Formulario buscar artículos.
	const handleSearch = (e) => {
		e.preventDefault()
		handleLoadArticles(1, search)
	}

	// Render Component.
	return (
		<Fragment>
			<WrapContainer>
				<WrapHeader>
					<WrapHeaderItem>
						<h4 className="text-uppercase mb-0 mr-2">Lista de Artículos</h4>
						<Button variant={'success'} onClick={handleAddArticle}>
							<FontAwesomeIcon icon={faPlus}/>
							<span className="text-uppercase ml-1">Artículo</span>
						</Button>
					</WrapHeaderItem>
					<WrapHeaderItem className="justify-content-end">
						<Form inline={true} onSubmit={handleSearch}>
							<Form.Control type="search" style={{minWidth: '340px'}}
														value={search} onChange={e => setSearch(e.target.value)}
														placeholder="ARTÍCULO..." className="mr-2"/>
							<Button variant={'dark'} type={'submit'}>
								<FontAwesomeIcon icon={faSearch}/>
								<span className="text-uppercase ml-1">Buscar</span>
							</Button>
						</Form>
					</WrapHeaderItem>
				</WrapHeader>
				<WrapBody>
					<Table striped={true} size={'sm'} className="mb-0">
						<thead className="thead-light">
						<tr className="text-uppercase">
							<th>Tipo</th>
							<th>Cod.Barras</th>
							<th>Nombre</th>
							<th>Precio</th>
							<th></th>
						</tr>
						</thead>
						<tbody>
						{articles && articles.map(data => <ArticleItem key={data.id} data={data}/>)}
						</tbody>
					</Table>
				</WrapBody>
			</WrapContainer>
			<ArticleModalForm/>
		</Fragment>
	)
}

export default ArticlesList
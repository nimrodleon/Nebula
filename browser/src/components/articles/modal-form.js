import React, {Fragment} from 'react'
import {Button, Col, Form, Modal} from 'react-bootstrap'
import {useDispatch, useSelector} from 'react-redux'
import {useForm} from 'react-hook-form'
import {AddArticleAction, btnCloseShowArticleModal, EditArticleAction} from '../../actions/articles'

/**
 * Formulario de edición artículos.
 * @returns {JSX.Element}
 * @constructor
 */
const ArticleModalForm = () => {
	const dispatch = useDispatch()
	const {register, handleSubmit, setValue} = useForm()

	// Variables de estado.
	const show = useSelector(state => state.articles.showModal)
	const title = useSelector(state => state.articles.title)
	const currentArticle = useSelector(state => state.articles.currentArticle)
	const typeOperation = useSelector(state => state.articles.typeOperation)
	const taxes = useSelector(state => state.taxes.taxes)

	//Acciones redux.
	const handleClose = () => dispatch(btnCloseShowArticleModal())
	const handleAddArticle = (article) => dispatch(AddArticleAction(article))
	const handleEditArticle = (article) => dispatch(EditArticleAction(article))

	// Guardar Cambios.
	const onSubmit = (f) => {
		if (typeOperation === 'ADD') {
			f.price_1 = Number(f.price_1)
			handleAddArticle(f)
		}
		if (typeOperation === 'EDIT') {
			f.price_1 = Number(f.price_1)
			handleEditArticle(f)
		}
	}

	// Cargar valores al form de artículo.
	setValue('id', currentArticle.id)
	setValue('name', currentArticle.name)
	setValue('type', currentArticle.type)
	setValue('bar_code', currentArticle.bar_code)
	setValue('tax_id', currentArticle.tax_id)
	setValue('price_1', currentArticle.price_1)
	setValue('remark', currentArticle.remark)

	// Render Component.
	return (
		<Fragment>
			<Modal show={show} onHide={handleClose} centered={true} animation={false}>
				<Modal.Header closeButton>
					<Modal.Title className="text-uppercase">{title}</Modal.Title>
				</Modal.Header>
				<Modal.Body>
					<Form onSubmit={handleSubmit(onSubmit)} id="ArticleModalForm">
						<Form.Group>
							<Form.Label className="font-weight-bold">Nombre</Form.Label>
							<Form.Control type="text" {...register('name', {required: true})} placeholder="Descripción..."/>
						</Form.Group>
						<Form.Row>
							<Form.Group as={Col}>
								<Form.Label className="font-weight-bold">Tipo</Form.Label>
								<Form.Control as={'select'} {...register('type', {required: true})}>
									<option value="PRODUCTO">PRODUCTO</option>
									<option value="SERVICIO">SERVICIO</option>
								</Form.Control>
							</Form.Group>
							<Form.Group as={Col}>
								<Form.Label className="font-weight-bold">Cod. Barras</Form.Label>
								<Form.Control type="text" {...register('bar_code')} placeholder="978020137962"/>
							</Form.Group>
						</Form.Row>
						<Form.Row>
							<Form.Group as={Col}>
								<Form.Label className="font-weight-bold">Impuesto</Form.Label>
								<Form.Control as={'select'} {...register('tax_id', {required: true})}>
									{taxes && taxes.map(data => <option key={data.id} value={data.id}>{data.name}</option>)}
								</Form.Control>
							</Form.Group>
							<Form.Group as={Col}>
								<Form.Label className="font-weight-bold">Precio(Inc.)</Form.Label>
								<Form.Control type="number" {...register('price_1', {required: true})} step="0.01" placeholder="0.01"/>
							</Form.Group>
						</Form.Row>
						<Form.Group className="mb-0">
							<Form.Label className="font-weight-bold">Descripción</Form.Label>
							<Form.Control as={'textarea'} {...register('remark')}
														placeholder="detalle la descripción del producto..."/>
						</Form.Group>
					</Form>
				</Modal.Body>
				<Modal.Footer>
					<Button variant="secondary" onClick={handleClose}>Cancelar</Button>
					<Button variant="primary" type={'submit'} form="ArticleModalForm">Guardar Cambios</Button>
				</Modal.Footer>
			</Modal>
		</Fragment>
	)
}

export default ArticleModalForm
import React, {Fragment} from 'react'
import {useDispatch, useSelector} from 'react-redux'
import {closeModalOrderRepair} from '../../actions/order_repair'
import {Button, Card, Col, Form, Modal, Row} from 'react-bootstrap'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faAlignJustify, faList, faNewspaper, faPrint, faSignOutAlt} from '@fortawesome/free-solid-svg-icons'
import Jdenticon from 'react-jdenticon'

/**
 * Detalle Orden de Reparación.
 * @returns {JSX.Element}
 * @constructor
 */
const OrderRepairDetail = () => {
	const dispatch = useDispatch()

	// Variables de estado.
	const show = useSelector(state => state.orderRepair.showModal)

	// Acciones redux.
	const handleClose = () => dispatch(closeModalOrderRepair())

	// Render Component.
	return (
		<Fragment>
			<Modal show={show} onHide={handleClose} size={'lg'}
						 centered={true} scrollable={true} animation={false}>
				<Modal.Header closeButton>
					<Modal.Title>
						<FontAwesomeIcon icon={faNewspaper} className="mr-2"/>
						<span className="text-uppercase">Laptop Hp Core I5 c241241</span>
					</Modal.Title>
				</Modal.Header>
				<Modal.Body>
					<Row>
						<Col xs={8}>
							<Form>
								<Form.Group>
									<Form.Label className="font-weight-bold">
										<FontAwesomeIcon icon={faAlignJustify} className="mr-2"/>
										<span className="text-uppercase">Falla del Equipo y/o Tarea</span>
									</Form.Label>
									<Form.Control as={'textarea'}/>
								</Form.Group>
								<Form.Group>
									<Button type={'submit'} variant={'success'}
													className="text-uppercase mr-2" size={'sm'}>Guardar</Button>
									<Button type={'submit'} variant={'danger'}
													className="text-uppercase" size={'sm'}>Cancelar</Button>
								</Form.Group>
							</Form>
							<hr/>
							<Form>
								<Form.Group>
									<Form.Label className="font-weight-bold">
										<FontAwesomeIcon icon={faList} className="mr-2"/>
										<span className="text-uppercase">Actividades</span>
									</Form.Label>
									<div className="d-flex">
										<div className="mr-2">
											<Jdenticon value={'user001'} size="48"/>
										</div>
										<div className="d-flex flex-column  w-100">
											<Form.Control as={'textarea'} placeholder={'Escriba un comentario...'}/>
											<div className="mt-2">
												<Button type={'submit'} variant={'success'}
																className="text-uppercase mr-2" size={'sm'}>Guardar</Button>
												<Button type={'submit'} variant={'danger'}
																className="text-uppercase" size={'sm'}>Cancelar</Button>
											</div>
										</div>
									</div>
								</Form.Group>
							</Form>
							{/* Lista de Actividades	*/}
							<div className="d-flex">
								<div className="mr-2">
									<Jdenticon value={'user002'} size="48"/>
								</div>
								<div className="d-flex flex-column w-100">
									<div>
										<span className="font-weight-bold mr-2">Anibal Chirca</span>
										<span className="font-weight-lighter">hace 1 minuto</span>
									</div>
									<Card>
										<Card.Body className="p-1">
											Cada comentario debe indicar la fecha registrada.
										</Card.Body>
									</Card>
									<div>
										<a href={`!#EDIT`} className="mr-2">Editar</a>
										<a href={`!#DELETE`}>Borrar</a>
									</div>
								</div>
							</div>
							<div className="d-flex">
								<div className="mr-2">
									<Jdenticon value={'user003'} size="48"/>
								</div>
								<div className="d-flex flex-column w-100">
									<div>
										<span className="font-weight-bold mr-2">Anibal Chirca</span>
										<span className="font-weight-lighter">hace 1 minuto</span>
									</div>
									<Card>
										<Card.Body className="p-1">
											Cada comentario debe indicar la fecha registrada.
										</Card.Body>
									</Card>
									<div>
										<a href={`!#EDIT`} className="mr-2">Editar</a>
										<a href={`!#DELETE`}>Borrar</a>
									</div>
								</div>
							</div>
							<div className="d-flex">
								<div className="mr-2">
									<Jdenticon value={'user004'} size="48"/>
								</div>
								<div className="d-flex flex-column w-100">
									<div>
										<span className="font-weight-bold mr-2">Anibal Chirca</span>
										<span className="font-weight-lighter">hace 1 minuto</span>
									</div>
									<Card>
										<Card.Body className="p-1">
											Cada comentario debe indicar la fecha registrada.
										</Card.Body>
									</Card>
									<div>
										<a href={`!#EDIT`} className="mr-2">Editar</a>
										<a href={`!#DELETE`}>Borrar</a>
									</div>
								</div>
							</div>
						</Col>
						<Col xs={4}>
							<div>
								<span className="badge badge-primary mr-1">SIN REVISAR</span>
								<span className="badge badge-success">ENTREGADO(S)</span>
							</div>
							<hr className="m-1"/>
							<div>
								<Button type={'submit'} variant={'warning'} className="mr-2" size={'sm'}>
									<FontAwesomeIcon icon={faPrint} className="mr-1"/>
									<span className="text-uppercase">Imprimir</span>
								</Button>
								<Button type={'submit'} variant={'info'} size={'sm'}>
									<FontAwesomeIcon icon={faSignOutAlt} className="mr-1"/>
									<span className="text-uppercase">Entregar</span>
								</Button>
							</div>
							<hr className="m-1"/>
							<div>
								<a href={`!#REPUESTO`}>Agregar Repuesto</a>
							</div>
							<div>
								<a href={`!#MANO_DE_OBRA`}>Agregar Mano de Obra</a>
							</div>
							<hr className="m-1"/>

						</Col>
					</Row>
				</Modal.Body>
				{/*<Modal.Footer>
					<Button variant="secondary" onClick={handleClose}>
						Close
					</Button>
					<Button variant="primary" onClick={handleClose}>
						Save Changes
					</Button>
				</Modal.Footer>*/}
			</Modal>
		</Fragment>
	)
}

export default OrderRepairDetail
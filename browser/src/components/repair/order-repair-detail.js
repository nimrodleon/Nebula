import React, {Fragment} from 'react'
import {useDispatch, useSelector} from 'react-redux'
import {closeModalOrderRepair} from '../../actions/order_repair'
import {Col, Modal, Row} from 'react-bootstrap'

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
					<Modal.Title className="text-uppercase">Detalle Orden de Reparación</Modal.Title>
				</Modal.Header>
				<Modal.Body>
					<Row>
						<Col xs={8}>
							hola
						</Col>
						<Col xs={4}>
							como estas
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
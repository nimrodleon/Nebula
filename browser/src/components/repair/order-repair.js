import React, {Fragment, useEffect} from 'react'
import {Button, Card, Col, Form} from 'react-bootstrap'
import {accessToken, baseURL} from '../../axios'

/**
 * Orden de Reparación.
 * @returns {JSX.Element}
 * @constructor
 */
const OrderRepair = () => {

	useEffect(() => {
		// Integrantes que recepcionan los equipos.
		window.$('#user_0').select2({
			theme: 'bootstrap4',
			placeholder: 'Seleccione una opción',
			ajax: {
				headers: {
					Authorization: accessToken
				},
				url: baseURL + 'users/select2/q',
			}
		})
		// Buscador de Clientes.
		window.$('#clients').select2({
			theme: 'bootstrap4',
			placeholder: 'Seleccione una opción',
			ajax: {
				headers: {
					Authorization: accessToken
				},
				url: baseURL + 'contacts/select2/q',
			}
		})
		//Buscador de tipos de equipo.
		window.$('#device_types').select2({
			theme: 'bootstrap4',
			placeholder: 'Seleccione una opción',
			ajax: {
				headers: {
					Authorization: accessToken
				},
				url: baseURL + 'device_types/select2/q',
			}
		})
		// Buscador de almacenes.
		window.$('#warehouses').select2({
			theme: 'bootstrap4',
			placeholder: 'Seleccione una opción',
			ajax: {
				headers: {
					Authorization: accessToken
				},
				url: baseURL + 'warehouses/REPARACIÓN/select2/q',
			}
		})
		// Técnico asignado a la reparación.
		window.$('#user_1').select2({
			theme: 'bootstrap4',
			placeholder: 'Seleccione una opción',
			ajax: {
				headers: {
					Authorization: accessToken
				},
				url: baseURL + 'users/select2/q',
			}
		})
	})

	// Render Component.
	return (
		<Fragment>
			<div className="bg-white w-100 m-2">
				<div className="p-4">
					<h4 className="text-uppercase">Orden de Reparación</h4>
					<hr/>
					<Form>
						<Form.Row className="w-75">
							<Form.Group as={Col} md={3}>
								<Form.Label className="text-uppercase font-weight-bold">Fecha Recepción</Form.Label>
								<Form.Control type="date"/>
							</Form.Group>
							<Form.Group as={Col} md={2}>
								<Form.Label className="text-uppercase font-weight-bold">Hora</Form.Label>
								<Form.Control type="time"/>
							</Form.Group>
							<Form.Group as={Col}>
								<Form.Label className="text-uppercase font-weight-bold">Integrante que recepciona el Equipo</Form.Label>
								<Form.Control as={'select'} id="user_0"/>
							</Form.Group>
						</Form.Row>
						<Card>
							<Card.Header>
								<Card.Title className="text-uppercase mb-0">Información del Cliente y Equipo</Card.Title>
							</Card.Header>
							<Card.Body>
								<Form.Row>
									<Form.Group as={Col} md={4}>
										<Form.Label className="text-uppercase font-weight-bold">Nombre Cliente</Form.Label>
										<div className="d-flex">
											<Form.Control as={'select'} id="clients"/>
											<Button variant={'info'} className="ml-2">
												<span className="text-uppercase">Agregar</span>
											</Button>
										</div>
									</Form.Group>
									<Form.Group as={Col} md={4}>
										<Form.Label className="text-uppercase font-weight-bold">Tipo Equipo</Form.Label>
										<div className="d-flex">
											<Form.Control as={'select'} id="device_types"/>
											<Button variant={'info'} className="ml-2">
												<span className="text-uppercase">Agregar</span>
											</Button>
										</div>
									</Form.Group>
									<Form.Group as={Col} md={4}>
										<Form.Label className="text-uppercase font-weight-bold">Almacén</Form.Label>
										<Form.Control as={'select'} id="warehouses"/>
									</Form.Group>
								</Form.Row>
								<Form.Group>
									<Form.Label className="text-uppercase font-weight-bold">Información del Equipo</Form.Label>
									<Form.Control type="text" placeholder="Detalle del Equipo..."/>
								</Form.Group>
								<Form.Group className="mb-0">
									<Form.Label className="text-uppercase font-weight-bold">Falla del Equipo</Form.Label>
									<Form.Control as={'textarea'}/>
								</Form.Group>
							</Card.Body>
						</Card>
						<Form.Row className="mt-2 w-75">
							<Form.Group as={Col} md={3}>
								<Form.Label className="text-uppercase font-weight-bold">Fecha Prometido</Form.Label>
								<Form.Control type="date"/>
							</Form.Group>
							<Form.Group as={Col} md={2}>
								<Form.Label className="text-uppercase font-weight-bold">Hora</Form.Label>
								<Form.Control type="time"/>
							</Form.Group>
							<Form.Group as={Col}>
								<Form.Label className="text-uppercase font-weight-bold">Técnico Asignado a la Reparación</Form.Label>
								<Form.Control as={'select'} id="user_1"/>
							</Form.Group>
						</Form.Row>
						<Form.Group className="mb-0">
							<Button variant={'dark'} className="mr-2">
								<span className="text-uppercase">Guardar</span>
							</Button>
							<Button variant={'primary'} className="mr-2">
								<span className="text-uppercase">Guardar & Imprimir</span>
							</Button>
							<Button variant={'danger'}>
								<span className="text-uppercase">Cancelar</span>
							</Button>
						</Form.Group>
					</Form>
				</div>
			</div>
		</Fragment>
	)
}

export default OrderRepair
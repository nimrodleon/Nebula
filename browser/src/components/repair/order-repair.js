import React, {Fragment, useEffect} from 'react'
import {useHistory, useParams} from 'react-router-dom'
import {Button, Card, Col, Form} from 'react-bootstrap'
import axios, {accessToken, baseURL} from '../../axios'
import {useForm} from 'react-hook-form'
import {useDispatch, useSelector} from 'react-redux'
import {AddOrderRepairAction, editFormOrderRepair, EditOrderRepairAction} from '../../actions/order_repair'

/**
 * Orden de Reparación.
 * @returns {JSX.Element}
 * @constructor
 */
const OrderRepair = () => {
	let history = useHistory()
	const dispatch = useDispatch()
	const {orderRepairID} = useParams()
	const {register, handleSubmit, setValue} = useForm()

	// Variables de estado.
	const currentOrderRepair = useSelector(state => state.orderRepair.currentOrderRepair)
	const typeOperation = useSelector(state => state.orderRepair.typeOperation)

	useEffect(() => {
		// Orden de reparación.
		console.log(orderRepairID)
		if (orderRepairID !== undefined) {
			dispatch(editFormOrderRepair(orderRepairID))
		}
	}, [orderRepairID, dispatch])

	// Configuración de select2.
	useEffect(() => {
		// Integrantes que recepcionan los equipos.
		const userMemberSelect = window.$('#user_0')
		userMemberSelect.select2({
			theme: 'bootstrap4',
			placeholder: 'Seleccione una opción',
			ajax: {
				headers: {
					Authorization: accessToken
				},
				url: baseURL + 'users/select2/q',
			}
		}).on('select2:select', (e) => {
			console.log('[member_user_id]', e.target.value)
			setValue('member_user_id', e.target.value)
		})
		// Buscador de Clientes.
		const clientSelect = window.$('#clients')
		clientSelect.select2({
			theme: 'bootstrap4',
			placeholder: 'Seleccione una opción',
			ajax: {
				headers: {
					Authorization: accessToken
				},
				url: baseURL + 'contacts/select2/q',
			}
		}).on('select2:select', (e) => {
			console.log('[client_id]', e.target.value)
			setValue('client_id', e.target.value)
		})
		//Buscador de tipos de equipo.
		const deviceTypeSelect = window.$('#device_types')
		deviceTypeSelect.select2({
			theme: 'bootstrap4',
			placeholder: 'Seleccione una opción',
			ajax: {
				headers: {
					Authorization: accessToken
				},
				url: baseURL + 'device_types/select2/q',
			}
		}).on('select2:select', (e) => {
			console.log('[device_type_id]', e.target.value)
			setValue('device_type_id', e.target.value)
		})
		// Buscador de almacenes.
		const warehousesSelect = window.$('#warehouses')
		warehousesSelect.select2({
			theme: 'bootstrap4',
			placeholder: 'Seleccione una opción',
			ajax: {
				headers: {
					Authorization: accessToken
				},
				url: baseURL + 'warehouses/REPARACIÓN/select2/q',
			}
		}).on('select2:select', (e) => {
			console.log('[warehouse_id]', e.target.value)
			setValue('warehouse_id', e.target.value)
		})
		// Técnico asignado a la reparación.
		const technicalUserSelect = window.$('#user_1')
		technicalUserSelect.select2({
			theme: 'bootstrap4',
			placeholder: 'Seleccione una opción',
			ajax: {
				headers: {
					Authorization: accessToken
				},
				url: baseURL + 'users/select2/q',
			}
		}).on('select2:select', (e) => {
			console.log('[technical_user_id]', e.target.value)
			setValue('technical_user_id', e.target.value)
		})
		// Cargar valores por defecto select2.
		// ========================================
		if (orderRepairID !== undefined && typeOperation === 'EDIT') {
			// Integrante que recepciona el equipo.
			axios.get(`users/${currentOrderRepair.member_user_id}`).then(({data}) => {
				userMemberSelect.append(new Option(data.full_name, data.id, true, true)).trigger('change')
			})
			// Cliente.
			axios.get(`contacts/${currentOrderRepair.client_id}`).then(({data}) => {
				clientSelect.append(new Option(data.document + ' - ' + data.full_name,
					data.id, true, true)).trigger('change')
			})
			// Tipo equipo.
			axios.get(`device_types/${currentOrderRepair.device_type_id}`).then(({data}) => {
				deviceTypeSelect.append(new Option(data.name, data.id, true, true)).trigger('change')
			})
			// Almacén
			axios.get(`warehouses/${currentOrderRepair.warehouse_id}`).then(({data}) => {
				warehousesSelect.append(new Option(data.name, data.id, true, true)).trigger('change')
			})
			// Técnico asignado a la reparación.
			axios.get(`users/${currentOrderRepair.technical_user_id}`).then(({data}) => {
				technicalUserSelect.append(new Option(data.full_name, data.id, true, true)).trigger('change')
			})
		}
	})

	// Guardar Cambios.
	const onSubmit = async (f) => {
		if (typeOperation === 'ADD') {
			await dispatch(AddOrderRepairAction(f))
			history.push('/repair')
		}
		if (typeOperation === 'EDIT') {
			await dispatch(EditOrderRepairAction(f))
			history.push('/repair')
		}
	}

	// Cancelar equipo.
	const handleCancel = () => {
		history.push('/repair')
	}

	// Cargar Valores por defecto.
	if (orderRepairID !== undefined && typeOperation === 'EDIT') {
		setValue('id', currentOrderRepair.id)
		setValue('reception_date', currentOrderRepair.reception_date)
		setValue('reception_time', currentOrderRepair.reception_time)
		setValue('member_user_id', currentOrderRepair.member_user_id)
		setValue('client_id', currentOrderRepair.client_id)
		setValue('device_type_id', currentOrderRepair.device_type_id)
		setValue('warehouse_id', currentOrderRepair.warehouse_id)
		setValue('device_info', currentOrderRepair.device_info)
		setValue('failure', currentOrderRepair.failure)
		setValue('promised_date', currentOrderRepair.promised_date)
		setValue('promised_time', currentOrderRepair.promised_time)
		setValue('technical_user_id', currentOrderRepair.technical_user_id)
	}

	// Render Component.
	return (
		<Fragment>
			<div className="bg-white w-100 m-2">
				<div className="p-4">
					<h4 className="text-uppercase">Orden de Reparación</h4>
					<hr/>
					<Form onSubmit={handleSubmit(onSubmit)}>
						<Form.Row className="w-75">
							<Form.Group as={Col} md={3}>
								<Form.Label className="text-uppercase font-weight-bold">Fecha Recepción</Form.Label>
								<Form.Control type="date" {...register('reception_date', {required: true})}/>
							</Form.Group>
							<Form.Group as={Col} md={2}>
								<Form.Label className="text-uppercase font-weight-bold">Hora</Form.Label>
								<Form.Control type="time" {...register('reception_time', {required: true})}/>
							</Form.Group>
							<Form.Group as={Col}>
								<Form.Label className="text-uppercase font-weight-bold">Integrante que recepciona el Equipo</Form.Label>
								<Form.Control as={'select'} id="user_0" {...register('member_user_id', {required: true})}/>
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
											<Form.Control as={'select'} id="clients" {...register('client_id', {required: true})}/>
											<Button variant={'info'} className="ml-2">
												<span className="text-uppercase">Agregar</span>
											</Button>
										</div>
									</Form.Group>
									<Form.Group as={Col} md={4}>
										<Form.Label className="text-uppercase font-weight-bold">Tipo Equipo</Form.Label>
										<div className="d-flex">
											<Form.Control as={'select'} id="device_types" {...register('device_type_id', {required: true})}/>
											<Button variant={'info'} className="ml-2">
												<span className="text-uppercase">Agregar</span>
											</Button>
										</div>
									</Form.Group>
									<Form.Group as={Col} md={4}>
										<Form.Label className="text-uppercase font-weight-bold">Almacén</Form.Label>
										<Form.Control as={'select'} id="warehouses" {...register('warehouse_id', {required: true})}/>
									</Form.Group>
								</Form.Row>
								<Form.Group>
									<Form.Label className="text-uppercase font-weight-bold">Información del Equipo</Form.Label>
									<Form.Control type="text" {...register('device_info', {required: true})}
																placeholder="Detalle del Equipo..."/>
								</Form.Group>
								<Form.Group className="mb-0">
									<Form.Label className="text-uppercase font-weight-bold">Falla del Equipo</Form.Label>
									<Form.Control as={'textarea'} {...register('failure', {required: true})}/>
								</Form.Group>
							</Card.Body>
						</Card>
						<Form.Row className="mt-2 w-75">
							<Form.Group as={Col} md={3}>
								<Form.Label className="text-uppercase font-weight-bold">Fecha Prometido</Form.Label>
								<Form.Control type="date" {...register('promised_date', {required: true})}/>
							</Form.Group>
							<Form.Group as={Col} md={2}>
								<Form.Label className="text-uppercase font-weight-bold">Hora</Form.Label>
								<Form.Control type="time" {...register('promised_time', {required: true})}/>
							</Form.Group>
							<Form.Group as={Col}>
								<Form.Label className="text-uppercase font-weight-bold">Técnico Asignado a la Reparación</Form.Label>
								<Form.Control as={'select'} id="user_1" {...register('technical_user_id', {required: true})}/>
							</Form.Group>
						</Form.Row>
						<Form.Group className="mb-0">
							<Button variant={'dark'} type={'submit'} className="mr-2">
								<span className="text-uppercase">Guardar</span>
							</Button>
							<Button variant={'danger'} onClick={handleCancel}>
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
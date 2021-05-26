import React, {Fragment} from 'react'
import {Button, Form, Modal} from 'react-bootstrap'
import {useDispatch, useSelector} from 'react-redux'
import {useForm} from 'react-hook-form'
import {AddDeviceTypeAction, btnCloseShowDeviceTypeModal, EditDeviceTypeAction} from '../../../actions/device_types'

/**
 * modal Tipo de equipo.
 * @returns {JSX.Element}
 * @constructor
 */
const DeviceTypeModalForm = () => {
	const dispatch = useDispatch()
	const {register, handleSubmit, setValue} = useForm()

	// Variables de estado.
	const show = useSelector(state => state.deviceTypes.showModal)
	const title = useSelector(state => state.deviceTypes.title)
	const currentDeviceType = useSelector(state => state.deviceTypes.currentDeviceType)
	const typeOperation = useSelector(state => state.deviceTypes.typeOperation)

	// Acciones redux.
	const handleClose = () => dispatch(btnCloseShowDeviceTypeModal())
	const handleAddDeviceType = (deviceType) => dispatch(AddDeviceTypeAction(deviceType))
	const handleEditDeviceType = (deviceType) => dispatch(EditDeviceTypeAction(deviceType))

	// Guardar Cambios.
	const onSubmit = (f) => {
		if (typeOperation === 'ADD') {
			handleAddDeviceType(f)
		}
		if (typeOperation === 'EDIT') {
			handleEditDeviceType(f)
		}
	}

	// Cargar valores por defecto.
	setValue('id', currentDeviceType.id)
	setValue('name', currentDeviceType.name)

	// Render Component.
	return (
		<Fragment>
			<Modal show={show} onHide={handleClose} centered={true} animation={false}>
				<Modal.Header closeButton>
					<Modal.Title className="text-uppercase">{title}</Modal.Title>
				</Modal.Header>
				<Modal.Body>
					<Form onSubmit={handleSubmit(onSubmit)} id="DeviceTypeModalForm">
						<Form.Group>
							<Form.Label className="font-weight-bold">Nombre</Form.Label>
							<Form.Control type="text" {...register('name', {required: true})} placeholder="Tipo Equipo..."/>
						</Form.Group>
					</Form>
				</Modal.Body>
				<Modal.Footer>
					<Button variant="secondary" onClick={handleClose}>Cancelar</Button>
					<Button variant="primary" type={'submit'} form="DeviceTypeModalForm">Guardar Cambios</Button>
				</Modal.Footer>
			</Modal>
		</Fragment>
	)
}

export default DeviceTypeModalForm
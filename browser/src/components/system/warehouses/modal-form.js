import React, {Fragment} from 'react'
import {Button, Form, Modal} from 'react-bootstrap'
import {useDispatch, useSelector} from 'react-redux'
import {useForm} from 'react-hook-form'
import {AddWarehouseAction, btnCloseShowWarehouseModal, EditWarehouseAction} from '../../../actions/warehouses'

/**
 * modal accesorios.
 * @returns {JSX.Element}
 * @constructor
 */
const WarehouseModalForm = () => {
	const dispatch = useDispatch()
	const {register, handleSubmit, setValue} = useForm()

	// Variables de estado.
	const show = useSelector(state => state.warehouses.showModal)
	const title = useSelector(state => state.warehouses.title)
	const currentWarehouse = useSelector(state => state.warehouses.currentWarehouse)
	const typeOperation = useSelector(state => state.warehouses.typeOperation)

	// Acciones redux.
	const handleClose = () => dispatch(btnCloseShowWarehouseModal())
	const handleAddWarehouse = (warehouse) => dispatch(AddWarehouseAction(warehouse))
	const handleEditWarehouse = (warehouse) => dispatch(EditWarehouseAction(warehouse))

	// Guardar Cambios.
	const onSubmit = (f) => {
		if (typeOperation === 'ADD') {
			handleAddWarehouse(f)
		}
		if (typeOperation === 'EDIT') {
			handleEditWarehouse(f)
		}
	}

	// Cargar valores por defecto.
	setValue('id', currentWarehouse.id)
	setValue('type', currentWarehouse.type)
	setValue('name', currentWarehouse.name)

	// Render Component.
	return (
		<Fragment>
			<Modal show={show} onHide={handleClose} centered={true} animation={false}>
				<Modal.Header closeButton>
					<Modal.Title className="text-uppercase">{title}</Modal.Title>
				</Modal.Header>
				<Modal.Body>
					<Form onSubmit={handleSubmit(onSubmit)} id="WarehouseModalForm">
						<Form.Group>
							<Form.Label className="font-weight-bold">Tipo</Form.Label>
							<Form.Control as={'select'} {...register('type', {required: true})}>
								<option value="PRODUCTO">PRODUCTO</option>
								<option value="CHATARRAS">CHATARRAS</option>
								<option value="REPARACIÓN">REPARACIÓN</option>
							</Form.Control>
						</Form.Group>
						<Form.Group className="mb-0">
							<Form.Label className="font-weight-bold">Nombre</Form.Label>
							<Form.Control type="text" {...register('name', {required: true})} placeholder="Almacén..."/>
						</Form.Group>
					</Form>
				</Modal.Body>
				<Modal.Footer>
					<Button variant="secondary" onClick={handleClose}>Cancelar</Button>
					<Button variant="primary" type={'submit'} form="WarehouseModalForm">Guardar Cambios</Button>
				</Modal.Footer>
			</Modal>
		</Fragment>
	)
}

export default WarehouseModalForm
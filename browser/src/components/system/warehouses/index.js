import React, {Fragment, useEffect, useState} from 'react'
import {Button, Form, Table} from 'react-bootstrap'
import {FontAwesomeIcon} from '@fortawesome/react-fontawesome'
import {faPlus, faSearch} from '@fortawesome/free-solid-svg-icons'
import {WrapBody, WrapContainer, WrapHeader, WrapHeaderItem} from '../../common/util'
import {useDispatch, useSelector} from 'react-redux'
import {btnAddShowWarehouseModal, LoadWarehousesAction} from '../../../actions/warehouses'
import WarehouseItem from './warehouse-item'
import WarehouseModalForm from './modal-form'

/**
 * Lista de Accesorios.
 * @returns {JSX.Element}
 * @constructor
 */
const WarehousesList = () => {
	const dispatch = useDispatch()

	// Variable para almacenar el texto a buscar.
	const [search, setSearch] = useState('')

	useEffect(() => {
		handleLoadWarehouses()
		// eslint-disable-next-line
	}, [])

	// Variables de estado.
	const warehouses = useSelector(state => state.warehouses.warehouses)

	// Acciones redux.
	const handleAddWarehouse = () => dispatch(btnAddShowWarehouseModal())
	const handleLoadWarehouses = (page = 1, search = '') => dispatch(LoadWarehousesAction(page, search))

	// Formulario de búsqueda.
	const handleSearch = (e) => {
		e.preventDefault()
		handleLoadWarehouses(1, search)
	}

	// Render Component.
	return (
		<Fragment>
			<WrapContainer>
				<WrapHeader>
					<WrapHeaderItem>
						<h4 className="text-uppercase mb-0 mr-2">Lista de Almacenes</h4>
						<Button variant={'success'} onClick={handleAddWarehouse}>
							<FontAwesomeIcon icon={faPlus}/>
							<span className="text-uppercase ml-1">Almacén</span>
						</Button>
					</WrapHeaderItem>
					<WrapHeaderItem className="justify-content-end">
						<Form inline={true} onSubmit={handleSearch}>
							<Form.Control type="search" style={{minWidth: '340px'}}
														value={search} onChange={e => setSearch(e.target.value)}
														placeholder="ALMACENES..." className="mr-2"/>
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
							<th>Nombre</th>
							<th></th>
						</tr>
						</thead>
						<tbody>
						{warehouses && warehouses.map(data => <WarehouseItem key={data.id} data={data}/>)}
						</tbody>
					</Table>
				</WrapBody>
			</WrapContainer>
			<WarehouseModalForm/>
		</Fragment>
	)
}

export default WarehousesList